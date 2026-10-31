import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';

export interface FormaPago {
  idFormaPago: number;
  nombre: string;
  descripcion: string | null;
  estado: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class FormasPagoService
  extends GenericCrudService<FormaPago> {

  constructor(http: HttpClient) {
    super(
      http,
      'http://localhost:5230/FormasPago'
    );
  }

  getFormasPago(
    pageNumber: number = 1,
    pageSize: number = 100,
    filter: string = ''
  ): Observable<PagedResult<FormaPago>> {

    return this.getPaged(
      pageNumber,
      pageSize,
      filter
    );
  }

  getFormaPagoById(
    idFormaPago: number
  ): Observable<FormaPago> {

    return this.getById(
      idFormaPago
    );
  }

  addFormaPago(
    formaPago: FormaPago
  ): Observable<FormaPago> {

    return this.http.post<FormaPago>(
      this.apiUrl,
      {
        nombre: formaPago.nombre,
        descripcion: formaPago.descripcion,
        estado: formaPago.estado
      }
    );
  }

  updateFormaPago(
    formaPago: FormaPago
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${formaPago.idFormaPago}`,
      {
        nombre: formaPago.nombre,
        descripcion: formaPago.descripcion,
        estado: formaPago.estado
      }
    );
  }

  deleteFormaPago(
    idFormaPago: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${idFormaPago}`
    );
  }
}