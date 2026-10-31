import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';


export interface Factura {
  idFactura: number;
  idPaciente: number;
  fechaFactura: string;
  estado: string;
}


@Injectable({
  providedIn: 'root'
})
export class FacturasService
  extends GenericCrudService<Factura> {

  constructor(
    http: HttpClient
  ) {
    super(
      http,
      'http://localhost:5230/Facturas'
    );
  }


  getFacturas(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<Factura>> {

    return this.getPaged(
      pageNumber,
      pageSize,
      filter
    );
  }


  getFacturaById(
    idFactura: number
  ): Observable<Factura> {

    return this.getById(
      idFactura
    );
  }


  addFactura(
    factura: Factura
  ): Observable<Factura> {

    return this.http.post<Factura>(
      this.apiUrl,
      {
        idPaciente:
          factura.idPaciente,

        fechaFactura:
          factura.fechaFactura,

        estado:
          factura.estado
      }
    );
  }


  updateFactura(
    factura: Factura
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${factura.idFactura}`,
      {
        idPaciente:
          factura.idPaciente,

        fechaFactura:
          factura.fechaFactura,

        estado:
          factura.estado
      }
    );
  }


  deleteFactura(
    idFactura: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${idFactura}`
    );
  }
}