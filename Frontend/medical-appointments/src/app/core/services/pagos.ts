import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';

export interface Pago {
  idPago: number;
  idFactura: number;
  idFormaPago: number;
  monto: number;
  fechaPago: string;
}

@Injectable({
  providedIn: 'root'
})
export class PagosService
  extends GenericCrudService<Pago> {

  constructor(http: HttpClient) {
    super(
      http,
      'http://localhost:5230/Pagos'
    );
  }

  getPagos(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<Pago>> {

    return this.getPaged(
      pageNumber,
      pageSize,
      filter
    );
  }

  getPagosFactura(
    idFactura: number
  ): Observable<PagedResult<Pago>> {

    return this.getPaged(
      1,
      100,
      `IdFactura == ${idFactura}`
    );
  }

  getPagoById(
    idPago: number
  ): Observable<Pago> {

    return this.getById(
      idPago
    );
  }

  addPago(
    pago: Pago
  ): Observable<Pago> {

    return this.http.post<Pago>(
      this.apiUrl,
      {
        idFactura: pago.idFactura,
        idFormaPago: pago.idFormaPago,
        monto: pago.monto,
        fechaPago: pago.fechaPago
      }
    );
  }

  updatePago(
    pago: Pago
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${pago.idPago}`,
      {
        idFactura: pago.idFactura,
        idFormaPago: pago.idFormaPago,
        monto: pago.monto,
        fechaPago: pago.fechaPago
      }
    );
  }

  deletePago(
    idPago: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${idPago}`
    );
  }
}