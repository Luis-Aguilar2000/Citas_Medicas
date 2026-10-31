import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';

export interface Servicio {
  idServicio: number;
  nombre: string;
  descripcion: string | null;
  precio: number;
  estado: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ServiciosService
  extends GenericCrudService<Servicio> {

  constructor(http: HttpClient) {
    super(
      http,
      'http://localhost:5230/Servicios'
    );
  }

  getServicios(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<Servicio>> {
    return this.getPaged(
      pageNumber,
      pageSize,
      filter
    );
  }

  getServicioById(
    idServicio: number
  ): Observable<Servicio> {
    return this.getById(idServicio);
  }

  addServicio(
    servicio: Servicio
  ): Observable<Servicio> {
    return this.http.post<Servicio>(
      this.apiUrl,
      {
        nombre: servicio.nombre,
        descripcion: servicio.descripcion,
        precio: servicio.precio,
        estado: servicio.estado
      }
    );
  }

  updateServicio(
    servicio: Servicio
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${servicio.idServicio}`,
      {
        nombre: servicio.nombre,
        descripcion: servicio.descripcion,
        precio: servicio.precio,
        estado: servicio.estado
      }
    );
  }

  deleteServicio(
    idServicio: number
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${idServicio}`
    );
  }
}