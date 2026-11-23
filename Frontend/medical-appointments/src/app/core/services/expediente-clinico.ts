
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

// Modelo de datos de Expedientes Clínicos
export interface ExpedienteClinico {
  expedienteId: number;
  pacienteId: number;
  numeroExpediente: string;
  fechaApertura: string;
  estado: string;
  creadoEn: string;
  actualizadoEn: string;
}

@Injectable({
  providedIn: 'root'
})
export class ExpedientesClinicosService extends GenericCrudService<ExpedienteClinico> {

  constructor(http: HttpClient) {
    // Sustituir el puerto por el que utiliza tu API
    super(http, 'http://localhost:PUERTO/ExpedientesClinicos');
  }

  // Obtiene los expedientes clínicos paginados
  getExpedientes(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<ExpedienteClinico>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene un expediente por su ID
  getExpedienteById(id: number): Observable<ExpedienteClinico> {
    return this.getById(id);
  }

  // Registra un expediente clínico
  addExpediente(expediente: ExpedienteClinico): Observable<boolean> {
    return this.add(expediente);
  }

  // Actualiza un expediente clínico
  updateExpediente(expediente: ExpedienteClinico): Observable<boolean> {
    return this.update(expediente.expedienteId, expediente);
  }

  // Elimina un expediente clínico
  deleteExpediente(id: number): Observable<boolean> {
    return this.delete(id);
  }
}
