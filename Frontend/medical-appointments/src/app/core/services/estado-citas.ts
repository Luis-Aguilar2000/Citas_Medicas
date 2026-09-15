import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';


export interface EstadoCita {

  idEstadoCita: number;

  nombreEstado: string;

  descripcion: string | null;

}


@Injectable({
  providedIn: 'root'
})
export class EstadoCitasService
  extends GenericCrudService<EstadoCita> {

  constructor(http: HttpClient) {

    super(
      http,
      'https://localhost:7250/EstadoCitas'
    );

  }


  // =========================================
  // LISTAR ESTADOS PAGINADOS
  // =========================================

  getEstadosCitas(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<EstadoCita>> {

    return this.getPaged(
      pageNumber,
      pageSize
    );

  }


  // =========================================
  // OBTENER ESTADO POR ID
  // =========================================

  getEstadoCitaById(
    idEstadoCita: number
  ): Observable<EstadoCita> {

    return this.getById(
      idEstadoCita
    );

  }


  // =========================================
  // AGREGAR
  // =========================================

  addEstadoCita(
    estado: EstadoCita
  ): Observable<boolean> {

    return this.add(
      estado
    );

  }


  // =========================================
  // ACTUALIZAR
  // =========================================

  updateEstadoCita(
    estado: EstadoCita
  ): Observable<boolean> {

    return this.update(
      estado.idEstadoCita,
      estado
    );

  }


  // =========================================
  // ELIMINAR
  // =========================================

  deleteEstadoCita(
    idEstadoCita: number
  ): Observable<boolean> {

    return this.delete(
      idEstadoCita
    );

  }

}