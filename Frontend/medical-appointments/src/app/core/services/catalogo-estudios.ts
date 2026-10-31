import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';

export interface CatalogoEstudio {
  idCatalogoEstudio: number;
  codigo: string;
  nombreEstudio: string;
  categoria: string;
  descripcion: string;
  precio: number;
  activo: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class CatalogoEstudiosService
  extends GenericCrudService<CatalogoEstudio> {

  constructor(http: HttpClient) {
    super(
      http,
      'http://localhost:5085/CatalogoEstudio'
    );
  }

  getCatalogoEstudios(
    pageNumber: number = 1,
    pageSize: number = 100,
    filter: string = ''
  ): Observable<PagedResult<CatalogoEstudio>> {
    return this.getPaged(
      pageNumber,
      pageSize,
      filter
    );
  }

  getCatalogoEstudioById(
    idCatalogoEstudio: number
  ): Observable<CatalogoEstudio> {
    return this.getById(idCatalogoEstudio);
  }
}