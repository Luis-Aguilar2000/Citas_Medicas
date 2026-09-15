import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService } from '../generics/generic-crud.service';

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
    super(http, 'https://localhost:7250/Pacientes');
  }

  getPacientes(): Observable<Paciente[]> {
    return this.getAll();
  }

  getPacienteById(idPaciente: number): Observable<Paciente> {
    return this.getById(idPaciente);
  }

  addPaciente(paciente: Paciente): Observable<boolean> {
    return this.add(paciente);
  }

  updatePaciente(paciente: Paciente): Observable<boolean> {
    return this.update(paciente.idPaciente, paciente);
  }

  deletePaciente(idPaciente: number): Observable<boolean> {
    return this.delete(idPaciente);
  }

}