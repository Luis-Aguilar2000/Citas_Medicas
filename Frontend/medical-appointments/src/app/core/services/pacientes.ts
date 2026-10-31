import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

// Modelo de datos para la entidad Paciente
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

@Injectable({
  providedIn: 'root'
})
export class PacientesService extends GenericCrudService<Paciente> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5031/Pacientes');
  }

  // Obtiene el listado paginado de pacientes con opción de filtro
  getPacientes(pageNumber: number = 1, pageSize: number = 10, filter: string = ''): Observable<PagedResult<Paciente>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  // Obtiene un paciente específico por su ID
  getPacienteById(idPaciente: number): Observable<Paciente> {
    return this.getById(idPaciente);
  }

  // Registra un nuevo paciente
  addPaciente(paciente: Paciente): Observable<boolean> {
    return this.add(paciente);
  }

  // Actualiza un paciente existente
  updatePaciente(paciente: Paciente): Observable<boolean> {
    return this.update(paciente.idPaciente, paciente);
  }

  // Elimina un paciente por su ID
  deletePaciente(idPaciente: number): Observable<boolean> {
    return this.delete(idPaciente);
  }
}