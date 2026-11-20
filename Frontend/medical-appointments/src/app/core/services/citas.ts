import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

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

@Injectable({
  providedIn: 'root'
})
export class CitasService extends GenericCrudService<Cita> {
  constructor(http: HttpClient) {
    super(http, 'http://localhost:5031/Citas');
  }

  getCitas(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<Cita>> {
    return this.getPaged(pageNumber, pageSize);
  }

  getCitaById(idCita: number): Observable<Cita> {
    return this.getById(idCita);
  }

  addCita(cita: Cita): Observable<boolean> {
    return this.add(cita);
  }

  updateCita(cita: Cita): Observable<boolean> {
    return this.update(cita.idCita, cita);
  }

  deleteCita(idCita: number): Observable<boolean> {
    return this.delete(idCita);
  }
}