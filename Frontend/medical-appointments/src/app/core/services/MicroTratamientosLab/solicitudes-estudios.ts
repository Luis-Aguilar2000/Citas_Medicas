import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../../generics/generic-crud.service';

export interface SolicitudEstudio {
  idSolicitudEstudio: number;
  idPaciente: number;
  idMedicoSolicitante: number;
  idCita: number | null;
  fechaSolicitud: string;
  prioridad: string;
  estado: string;
  diagnosticoPresuntivo: string;
}

@Injectable({
  providedIn: 'root'
})
export class SolicitudesEstudiosService
  extends GenericCrudService<SolicitudEstudio> {

  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/SolicitudEstudio');
  }

  getSolicitudesEstudios(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<SolicitudEstudio>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  getSolicitudEstudioById(
    idSolicitudEstudio: number
  ): Observable<SolicitudEstudio> {
    return this.getById(idSolicitudEstudio);
  }
}
