import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

export interface PrescripcionDetalle {
  idPrescripcionDetalle: number;
  idPrescripcion: number;
  idMedicamento: number;
  dosis: number;
  unidadMedida: string;
  frecuencia: string;
  duracionDias: number;
  indicacionesEspeciales: string;
}

export interface CrearPrescripcionDetalle {
  idPrescripcion: number;
  idMedicamento: number;
  dosis: number;
  unidadMedida: string;
  frecuencia: string;
  duracionDias: number;
  indicacionesEspeciales: string;
}

@Injectable({
  providedIn: 'root'
})
export class PrescripcionDetallesService extends GenericCrudService<PrescripcionDetalle> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/PrescripDetalle');
  }

  // Obtiene el listado paginado de detalles
  getPrescripcionDetalles(
    pageNumber: number = 1,
    pageSize: number = 100,
    filter: string = ''
  ): Observable<PagedResult<PrescripcionDetalle>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene un detalle especÃ­fico por su ID
  getPrescripcionDetalleById(
    idPrescripcionDetalle: number
  ): Observable<PrescripcionDetalle> {
    return this.getById(idPrescripcionDetalle);
  }

  // Registra un nuevo detalle de prescripciÃ³n
  addPrescripcionDetalle(
    detalle: CrearPrescripcionDetalle
  ): Observable<boolean> {
    return this.http.post<boolean>(this.apiUrl, detalle);
  }

  // Actualiza un detalle existente
  updatePrescripcionDetalle(
    detalle: PrescripcionDetalle
  ): Observable<boolean> {
    return this.update(detalle.idPrescripcionDetalle, detalle);
  }

  // Elimina un detalle por su ID
  deletePrescripcionDetalle(
    idPrescripcionDetalle: number
  ): Observable<boolean> {
    return this.delete(idPrescripcionDetalle);
  }
}
