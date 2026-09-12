import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


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

export class PacientesService {

  /* =========================================
     URL API
  ========================================= */

  private apiUrl =
    'https://localhost:7250/Pacientes';


  constructor(
    private http: HttpClient
  ) {}


  /* =========================================
     GET - LISTAR PACIENTES
  ========================================= */

  getPacientes(): Observable<Paciente[]> {

    return this.http.get<Paciente[]>(
      this.apiUrl
    );

  }


  /* =========================================
     GET - PACIENTE POR ID
  ========================================= */

  getPacienteById(
    idPaciente: number
  ): Observable<Paciente> {

    return this.http.get<Paciente>(
      `${this.apiUrl}/${idPaciente}`
    );

  }


  /* =========================================
     POST - AGREGAR PACIENTE
  ========================================= */

  addPaciente(
    paciente: Paciente
  ): Observable<boolean> {

    return this.http.post<boolean>(
      this.apiUrl,
      paciente
    );

  }


  /* =========================================
     PUT - ACTUALIZAR PACIENTE
  ========================================= */

  updatePaciente(
    paciente: Paciente
  ): Observable<boolean> {

    return this.http.put<boolean>(
      `${this.apiUrl}/${paciente.idPaciente}`,
      paciente
    );

  }


  /* =========================================
     DELETE - ELIMINAR PACIENTE
  ========================================= */

  deletePaciente(
    idPaciente: number
  ): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.apiUrl}/${idPaciente}`
    );

  }

}