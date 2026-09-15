import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenericCrudService } from '../generics/generic-crud.service';

export interface Consultorio {
  idConsultorio: number;
  nombre: string;
  numeroConsultorio: string;
  piso: string;
  ubicacion: string;
  descripcion: string;
  estado: boolean;
  fechaRegistro: string;
}

@Injectable({
  providedIn: 'root'
})
export class ConsultoriosService extends GenericCrudService<Consultorio> {

  constructor(http: HttpClient) {
    super(http, 'https://localhost:7250/Consultorios');
  }

  getConsultorios(): Observable<Consultorio[]> {
    return this.getAll();
  }

  getConsultorioById(idConsultorio: number): Observable<Consultorio> {
    return this.getById(idConsultorio);
  }

  addConsultorio(consultorio: Consultorio): Observable<boolean> {
    return this.add(consultorio);
  }

  updateConsultorio(consultorio: Consultorio): Observable<boolean> {
    return this.update(consultorio.idConsultorio, consultorio);
  }

  deleteConsultorio(idConsultorio: number): Observable<boolean> {
    return this.delete(idConsultorio);
  }

}