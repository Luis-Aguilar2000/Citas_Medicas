import {
  HttpClient,
  HttpParams
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';


/* =========================================
   RESULTADO PAGINADO
========================================= */

export interface PagedResult<T> {

  data: T[];

  totalRecords: number;

  pageSize: number;

  currentPage: number;

  totalPages: number;

}


/* =========================================
   SERVICIO CRUD GENÉRICO
========================================= */

export abstract class GenericCrudService<T> {


  // =========================================
  // CONSTRUCTOR
  // =========================================

  protected constructor(

    protected http: HttpClient,

    protected apiUrl: string

  ) {}


  // =========================================
  // OBTENER TODOS
  // =========================================

  getAll():
    Observable<T[]> {

    return this.http.get<T[]>(
      this.apiUrl
    );

  }


  // =========================================
  // OBTENER PAGINADO
  // =========================================

  getPaged(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<T>> {

    let params =
      new HttpParams()
        .set(
          'pageNumber',
          pageNumber.toString()
        )
        .set(
          'pageSize',
          pageSize.toString()
        );


    // =========================================
    // FILTRO
    // =========================================

    if (
      filter.trim() !== ''
    ) {

      params = params.set(
        'filter',
        filter
      );

    }


    return this.http.get<PagedResult<T>>(
      this.apiUrl,
      {
        params
      }
    );

  }


  // =========================================
  // OBTENER POR ID
  // =========================================

  getById(
    id: number
  ): Observable<T> {

    return this.http.get<T>(
      `${this.apiUrl}/${id}`
    );

  }


  // =========================================
  // AGREGAR
  // =========================================

  add(
    entity: T
  ): Observable<boolean> {

    return this.http.post<boolean>(
      this.apiUrl,
      entity
    );

  }


  // =========================================
  // ACTUALIZAR
  // =========================================

  update(
    id: number,
    entity: T
  ): Observable<boolean> {

    return this.http.put<boolean>(
      `${this.apiUrl}/${id}`,
      entity
    );

  }


  // =========================================
  // ELIMINAR
  // =========================================

  delete(
    id: number
  ): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.apiUrl}/${id}`
    );

  }

}