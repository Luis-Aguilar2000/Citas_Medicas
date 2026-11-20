import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  GenericCrudService,
  PagedResult
} from '../generics/generic-crud.service';

export interface Medicamento {
  idMedicamento: number;
  nombreComercial: string;
  nombreGenerico: string;
  presentacion: string;
  concentracion: string;
  viaAdministracion: string;
  precio: number;
  activo: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MedicamentosService
  extends GenericCrudService<Medicamento> {

  constructor(http: HttpClient) {
    super(http, 'http://localhost:5085/Medicamento');
  }

  getMedicamentos(
    pageNumber: number = 1,
    pageSize: number = 10,
    filter: string = ''
  ): Observable<PagedResult<Medicamento>> {
    return this.getPaged(
      pageNumber,
      pageSize,
      filter
    );
  }

  getMedicamentoById(
    idMedicamento: number
  ): Observable<Medicamento> {
    return this.getById(idMedicamento);
  }

  addMedicamento(
    medicamento: Medicamento
  ): Observable<boolean> {
    return this.add(medicamento);
  }

  updateMedicamento(
    medicamento: Medicamento
  ): Observable<boolean> {
    return this.update(
      medicamento.idMedicamento,
      medicamento
    );
  }

  deleteMedicamento(
    idMedicamento: number
  ): Observable<boolean> {
    return this.delete(idMedicamento);
  }
}