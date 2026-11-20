import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

export interface Prescripcion {
  idPrescripcion: number;
  idPaciente: number;
  idMedico: number;
  idCita: number | null;
  fechaEmision: string;
  observaciones: string;
  estado: string;
}

export interface CrearPrescripcion {
  idPaciente: number;
  idMedico: number;
  idCita: number | null;
  fechaEmision: string;
  observaciones: string;
  estado: string;
}

@Injectable({
  providedIn: 'root'
})
export class PrescripcionesService extends GenericCrudService<Prescripcion> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/Prescripcion');
  }

  // Obtiene el listado paginado de prescripciones
  getPrescripciones(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<Prescripcion>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene una prescripción específica por su ID
  getPrescripcionById(idPrescripcion: number): Observable<Prescripcion> {
    return this.getById(idPrescripcion);
  }

  // Registra una nueva prescripción y devuelve el ID generado
  addPrescripcion(prescripcion: CrearPrescripcion): Observable<number> {
    return this.http.post<number>(this.apiUrl, prescripcion);
  }

  // Actualiza una prescripción existente
  updatePrescripcion(prescripcion: Prescripcion): Observable<boolean> {
    return this.update(prescripcion.idPrescripcion, prescripcion);
  }

  // Elimina una prescripción por su ID
  deletePrescripcion(idPrescripcion: number): Observable<boolean> {
    return this.delete(idPrescripcion);
  }
}