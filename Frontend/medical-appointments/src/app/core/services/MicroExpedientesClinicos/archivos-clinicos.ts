import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../../generics/generic-crud.service';

export interface ArchivoClinico {
  idArchivoClinico: number;
  idResultadoEstudio: number | null;
  idPaciente: number | null;
  nombreArchivo: string;
  extension: string;
  tipoMime: string;
  rutaAlmacenamiento: string;
  tamanoBytes: number;
  fechaSubida: string;
}

@Injectable({
  providedIn: 'root'
})
export class ArchivosClinicosService
  extends GenericCrudService<ArchivoClinico> {

  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/ArchClinico');
  }

  getArchivosClinicos(
    pageNumber: number = 1,
    pageSize: number = 100,
    filter: string = ''
  ): Observable<PagedResult<ArchivoClinico>> {
    return this.getPaged(pageNumber, pageSize, filter);
  }

  getArchivoClinicoById(
    idArchivoClinico: number
  ): Observable<ArchivoClinico> {
    return this.getById(idArchivoClinico);
  }
}
