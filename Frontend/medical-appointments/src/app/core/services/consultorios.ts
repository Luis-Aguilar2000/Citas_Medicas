import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

// Modelo de datos para la entidad Consultorio
export interface Consultorio {
  idConsultorio: number;
  nombre: string;
  numeroConsultorio: string;
  piso: string;
  ubicacion: string;
  descripcion: string;
  estado: boolean;
  fechaRegistro: string;
}

@Injectable({
  providedIn: 'root'
})
export class ConsultoriosService extends GenericCrudService<Consultorio> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5031/Consultorios');
  }

  // Obtiene el listado paginado de consultorios
  getConsultorios(pageNumber: number = 1, pageSize: number = 10): Observable<PagedResult<Consultorio>> {
    return this.getPaged(pageNumber, pageSize);
  }

  // Obtiene un consultorio específico por su ID
  getConsultorioById(idConsultorio: number): Observable<Consultorio> {
    return this.getById(idConsultorio);
  }

  // Registra un nuevo consultorio
  addConsultorio(consultorio: Consultorio): Observable<boolean> {
    return this.add(consultorio);
  }

  // Actualiza un consultorio existente
  updateConsultorio(consultorio: Consultorio): Observable<boolean> {
    return this.update(consultorio.idConsultorio, consultorio);
  }

  // Elimina un consultorio por su ID
  deleteConsultorio(idConsultorio: number): Observable<boolean> {
    return this.delete(idConsultorio);
  }
}