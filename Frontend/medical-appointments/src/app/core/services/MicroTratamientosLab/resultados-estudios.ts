import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../../generics/generic-crud.service';

export interface ResultadoEstudio {
  idResultadoEstudio: number;
  idSolicitudEstudioDetalle: number;
  idTecnicoOMedico: number;
  fechaResultado: string;
  hallazgos: string;
  conclusion: string;
  valoresReferencia: string;
}

@Injectable({
  providedIn: 'root'
})
export class ResultadosEstudiosService
  extends GenericCrudService<ResultadoEstudio> {

  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/ResultEstudios');
  }

  getResultadosEstudios(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<ResultadoEstudio>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  getResultadoEstudioById(
    idResultadoEstudio: number
  ): Observable<ResultadoEstudio> {
    return this.getById(idResultadoEstudio);
  }
}
