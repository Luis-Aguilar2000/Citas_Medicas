import {
  HttpClient
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';


export abstract class GenericCrudService<T> {

  /* =========================================
     CONSTRUCTOR
  ========================================= */

  protected constructor(

    protected http: HttpClient,

    protected apiUrl: string

  ) {}


  /* =========================================
     OBTENER TODOS
  ========================================= */

  getAll():
    Observable<T[]> {

    return this.http.get<T[]>(
      this.apiUrl
    );

  }


  /* =========================================
     OBTENER POR ID
  ========================================= */

  getById(
    id: number
  ): Observable<T> {

    return this.http.get<T>(
      `${this.apiUrl}/${id}`
    );

  }


  /* =========================================
     AGREGAR
  ========================================= */

  add(
    entity: T
  ): Observable<boolean> {

    return this.http.post<boolean>(
      this.apiUrl,
      entity
    );

  }


  /* =========================================
     ACTUALIZAR
  ========================================= */

  update(
    id: number,
    entity: T
  ): Observable<boolean> {

    return this.http.put<boolean>(
      `${this.apiUrl}/${id}`,
      entity
    );

  }


  /* =========================================
     ELIMINAR
  ========================================= */

  delete(
    id: number
  ): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.apiUrl}/${id}`
    );

  }

}