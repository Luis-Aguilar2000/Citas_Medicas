import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';


/* =========================================
   INTERFAZ PACIENTE
========================================= */

export interface Paciente {

  idPaciente: number;

  nombres: string;

  apellidos: string;

  fechaNacimiento: string;

  sexo: string;

  dui: string;

  telefono: string;

  correo: string;

  direccion: string;

  fechaRegistro: string;

}


/* =========================================
   SERVICIO PACIENTES
========================================= */

@Injectable({
  providedIn: 'root'
})
export class PacientesService
  extends GenericCrudService<Paciente> {


  /* =========================================
     CONSTRUCTOR
  ========================================= */

  constructor(
    http: HttpClient
  ) {

    super(
      http,
      'https://localhost:7250/Pacientes'
    );

  }


  /* =========================================
     GET - LISTAR PACIENTES PAGINADOS
  ========================================= */

  getPacientes(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<Paciente>> {

    return this.getPaged(
      pageNumber,
      pageSize
    );

  }


  /* =========================================
     GET - PACIENTE POR ID
  ========================================= */

  getPacienteById(
    idPaciente: number
  ): Observable<Paciente> {

    return this.getById(
      idPaciente
    );

  }


  /* =========================================
     POST - AGREGAR PACIENTE
  ========================================= */

  addPaciente(
    paciente: Paciente
  ): Observable<boolean> {

    return this.add(
      paciente
    );

  }


  /* =========================================
     PUT - ACTUALIZAR PACIENTE
  ========================================= */

  updatePaciente(
    paciente: Paciente
  ): Observable<boolean> {

    return this.update(
      paciente.idPaciente,
      paciente
    );

  }


  /* =========================================
     DELETE - ELIMINAR PACIENTE
  ========================================= */

  deletePaciente(
    idPaciente: number
  ): Observable<boolean> {

    return this.delete(
      idPaciente
    );

  }

}