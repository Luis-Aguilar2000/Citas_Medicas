import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

// Modelo de datos para la entidad EstadoCita
export interface EstadoCita {
  idEstadoCita: number;
  nombreEstado: string;
  descripcion: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class EstadoCitasService extends GenericCrudService<EstadoCita> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5031/EstadoCitas');
  }

  // Obtiene el listado paginado de estados de citas
  getEstadosCitas(pageNumber: number = 1, pageSize: number = 10): Observable<PagedResult<EstadoCita>> {
    return this.getPaged(pageNumber, pageSize);
  }

  // Obtiene un estado de cita especÃ­fico por su ID
  getEstadoCitaById(idEstadoCita: number): Observable<EstadoCita> {
    return this.getById(idEstadoCita);
  }

  // Registra un nuevo estado de cita
  addEstadoCita(estado: EstadoCita): Observable<boolean> {
    return this.add(estado);
  }

  // Actualiza un estado de cita existente
  updateEstadoCita(estado: EstadoCita): Observable<boolean> {
    return this.update(estado.idEstadoCita, estado);
  }

  // Elimina un estado de cita por su ID
  deleteEstadoCita(idEstadoCita: number): Observable<boolean> {
    return this.delete(idEstadoCita);
  }
}
