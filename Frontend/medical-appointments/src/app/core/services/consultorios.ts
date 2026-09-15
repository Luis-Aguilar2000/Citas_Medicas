import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';


/* =========================================
   MODELO CONSULTORIO
========================================= */

export interface Consultorio {

  idConsultorio: number;

  nombre: string;

  numeroConsultorio: string;

  piso: string;

  ubicacion: string;

  descripcion: string;

  estado: boolean;

  fechaRegistro: string;

}


/* =========================================
   SERVICIO CONSULTORIOS
========================================= */

@Injectable({
  providedIn: 'root'
})
export class ConsultoriosService
  extends GenericCrudService<Consultorio> {


  constructor(
    http: HttpClient
  ) {

    super(
      http,
      'https://localhost:7250/Consultorios'
    );

  }


  /* =========================================
     OBTENER CONSULTORIOS PAGINADOS
  ========================================= */

  getConsultorios(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<Consultorio>> {

    return this.getPaged(
      pageNumber,
      pageSize
    );

  }


  /* =========================================
     OBTENER CONSULTORIO POR ID
  ========================================= */

  getConsultorioById(
    idConsultorio: number
  ): Observable<Consultorio> {

    return this.getById(
      idConsultorio
    );

  }


  /* =========================================
     AGREGAR CONSULTORIO
  ========================================= */

  addConsultorio(
    consultorio: Consultorio
  ): Observable<boolean> {

    return this.add(
      consultorio
    );

  }


  /* =========================================
     ACTUALIZAR CONSULTORIO
  ========================================= */

  updateConsultorio(
    consultorio: Consultorio
  ): Observable<boolean> {

    return this.update(
      consultorio.idConsultorio,
      consultorio
    );

  }


  /* =========================================
     ELIMINAR CONSULTORIO
  ========================================= */

  deleteConsultorio(
    idConsultorio: number
  ): Observable<boolean> {

    return this.delete(
      idConsultorio
    );

  }

}