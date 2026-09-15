import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService } from '../generics/generic-crud.service';

export interface HorarioMedico {
  idHorario: number;
  idMedico: number;
  idConsultorio: number;
  diaSemana: string;
  horaInicio: string;
  horaFin: string;
  estado: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class HorarioMedicoService extends GenericCrudService<HorarioMedico> {

  constructor(http: HttpClient) {
    super(http, 'https://localhost:7250/HorariosMedicos');
  }

  getHorarios(): Observable<HorarioMedico[]> {
    return this.getAll();
  }

  getHorarioById(idHorario: number): Observable<HorarioMedico> {
    return this.getById(idHorario);
  }

  addHorario(horario: HorarioMedico): Observable<boolean> {
    return this.add(horario);
  }

  updateHorario(horario: HorarioMedico): Observable<boolean> {
    return this.update(horario.idHorario, horario);
  }

  deleteHorario(idHorario: number): Observable<boolean> {
    return this.delete(idHorario);
  }

}