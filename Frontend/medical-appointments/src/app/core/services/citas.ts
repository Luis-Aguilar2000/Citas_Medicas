import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';


/* =========================================
   MODELO CITA
========================================= */

export interface Cita {
  idCita: number;
  idPaciente: number;
  idMedico: number;
  idConsultorio: number;
  idEstadoCita: number;

  fechaCita: string;

  horaInicio: string;
  horaFin: string;

  motivoConsulta: string | null;
  observaciones: string | null;

  fechaRegistro: string;
}


/* =========================================
   SERVICIO
========================================= */

@Injectable({
  providedIn: 'root'
})
export class CitasService
  extends GenericCrudService<Cita> {

  constructor(http: HttpClient) {
    super(
      http,
      'https://localhost:7250/Citas'
    );
  }


  /* =========================================
     LISTAR CITAS PAGINADAS
  ========================================= */

  getCitas(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<Cita>> {

    return this.getPaged(
      pageNumber,
      pageSize
    );
  }


  /* =========================================
     OBTENER CITA POR ID
  ========================================= */

  getCitaById(
    idCita: number
  ): Observable<Cita> {

    return this.getById(
      idCita
    );
  }


  /* =========================================
     AGREGAR CITA
  ========================================= */

  addCita(
    cita: Cita
  ): Observable<boolean> {

    return this.add(
      cita
    );
  }


  /* =========================================
     ACTUALIZAR CITA
  ========================================= */

  updateCita(
    cita: Cita
  ): Observable<boolean> {

    return this.update(
      cita.idCita,
      cita
    );
  }


  /* =========================================
     ELIMINAR CITA
  ========================================= */

  deleteCita(
    idCita: number
  ): Observable<boolean> {

    return this.delete(
      idCita
    );
  }
}