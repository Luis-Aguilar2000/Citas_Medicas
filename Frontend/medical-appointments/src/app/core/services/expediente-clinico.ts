
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

// Modelo de datos de expedientes clínicos
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
    super(http, 'http://localhost:5000/ExpClinicos');
  }

  // Listar expedientes clínicos con paginación y filtro
  getExpedientes(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<ExpedienteClinico>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Consultar expediente por ID
  getExpedienteById(id: number): Observable<ExpedienteClinico> {
    return this.getById(id);
  }

  // Registrar expediente
  addExpediente(expediente: ExpedienteClinico): Observable<boolean> {
    return this.add(expediente);
  }

  // Actualizar expediente
  updateExpediente(expediente: ExpedienteClinico): Observable<boolean> {
    return this.update(expediente.expedienteId, expediente);
  }

  // Eliminar expediente
  deleteExpediente(id: number): Observable<boolean> {
    return this.delete(id);
  }
}
