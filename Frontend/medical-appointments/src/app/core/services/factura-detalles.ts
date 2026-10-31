import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';

export interface FacturaDetalle {
  idDetalle: number;
  idFactura: number;
  tipoItem: string;
  idItem: number;
  descripcion: string;
  cantidad: number;
  precioUnitario: number;
}

@Injectable({
  providedIn: 'root'
})
export class FacturaDetallesService
  extends GenericCrudService<FacturaDetalle> {

  constructor(http: HttpClient) {
    super(
      http,
      'http://localhost:5230/FacturaDetalle'
    );
  }

  getDetalles(
    pageNumber: number = 1,
    pageSize: number = 100,
    filter: string = ''
  ): Observable<PagedResult<FacturaDetalle>> {

    return this.getPaged(
      pageNumber,
      pageSize,
      filter
    );
  }

  getDetallesFactura(
    idFactura: number
  ): Observable<PagedResult<FacturaDetalle>> {

    return this.getPaged(
      1,
      100,
      `IdFactura == ${idFactura}`
    );
  }

  getDetalleById(
    id: number
  ): Observable<FacturaDetalle> {

    return this.getById(id);
  }

  agregarDetalle(
    detalle: FacturaDetalle
  ): Observable<FacturaDetalle> {

    return this.http.post<FacturaDetalle>(
      this.apiUrl,
      {
        idFactura: detalle.idFactura,
        tipoItem: detalle.tipoItem,
        idItem: detalle.idItem,
        descripcion: detalle.descripcion,
        cantidad: detalle.cantidad,
        precioUnitario: detalle.precioUnitario
      }
    );
  }

  actualizarDetalle(
    detalle: FacturaDetalle
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${detalle.idDetalle}`,
      {
        idFactura: detalle.idFactura,
        tipoItem: detalle.tipoItem,
        idItem: detalle.idItem,
        descripcion: detalle.descripcion,
        cantidad: detalle.cantidad,
        precioUnitario: detalle.precioUnitario
      }
    );
  }

  eliminarDetalle(
    id: number
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}