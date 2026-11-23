import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

export interface Especialidad {
  idEspecialidad: number;
  nombreEspecialidad?: string;
  nombre?: string;
  descripcion?: string;
  estado?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class EspecialidadesService {

  private readonly apiUrl = 'http://localhost:5144/Especialidades';

  constructor(private http: HttpClient) {}

  getEspecialidades(
    pageNumber: number = 1,
    pageSize: number = 1000
  ): Observable<PagedResult<Especialidad>> {
    return this.http.get<PagedResult<Especialidad>>(
      `${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }
}
