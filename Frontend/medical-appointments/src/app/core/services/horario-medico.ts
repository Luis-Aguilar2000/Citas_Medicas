import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../generics/generic-crud.service';

// Modelo de datos para la entidad HorarioMedico
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
    super(http, 'http://localhost:5031/HorariosMedicos');
  }

  // Obtiene el listado paginado de horarios médicos
  getHorarios(pageNumber: number = 1, pageSize: number = 10): Observable<PagedResult<HorarioMedico>> {
    return this.getPaged(pageNumber, pageSize);
  }

  // Obtiene un horario específico por su ID
  getHorarioById(idHorario: number): Observable<HorarioMedico> {
    return this.getById(idHorario);
  }

  // Registra un nuevo horario médico
  addHorario(horario: HorarioMedico): Observable<boolean> {
    return this.add(horario);
  }

  // Actualiza un horario médico existente
  updateHorario(horario: HorarioMedico): Observable<boolean> {
    return this.update(horario.idHorario, horario);
  }

  // Elimina un horario médico por su ID
  deleteHorario(idHorario: number): Observable<boolean> {
    return this.delete(idHorario);
  }
}