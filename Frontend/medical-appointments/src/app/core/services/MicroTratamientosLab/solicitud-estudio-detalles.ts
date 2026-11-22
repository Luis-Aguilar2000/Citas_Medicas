import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../../generics/generic-crud.service';

export interface SolicitudEstudioDetalle {
  idSolicitudEstudioDetalle: number;
  idSolicitudEstudio: number;
  idCatalogoEstudio: number;
  indicaciones: string;
  estado: string;
}

@Injectable({
  providedIn: 'root'
})
export class SolicitudEstudioDetallesService
  extends GenericCrudService<SolicitudEstudioDetalle> {

  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/SolicEstudDetalle');
  }

  getSolicitudEstudioDetalles(
    pageNumber: number = 1,
    pageSize: number = 100,
    filter: string = ''
  ): Observable<PagedResult<SolicitudEstudioDetalle>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  getSolicitudEstudioDetalleById(
    idSolicitudEstudioDetalle: number
  ): Observable<SolicitudEstudioDetalle> {
    return this.getById(idSolicitudEstudioDetalle);
  }
}
