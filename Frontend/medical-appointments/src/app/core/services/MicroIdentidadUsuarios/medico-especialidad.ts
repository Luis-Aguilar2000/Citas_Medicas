import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService, PagedResult } from '../../generics/generic-crud.service';

export interface MedicoEspecialidad {
  idMedico: number;
  idEspecialidad: number;
}

@Injectable({
  providedIn: 'root'
})
export class MedicoEspecialidadesService {

  private readonly apiUrl =
    'http://localhost:5144/MedicoEspecialidades';

  constructor(private http: HttpClient) {}

  getMedicoEspecialidades(
    pageNumber: number = 1,
    pageSize: number = 1000
  ): Observable<PagedResult<MedicoEspecialidad>> {
    return this.http.get<PagedResult<MedicoEspecialidad>>(
      `${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }
}
