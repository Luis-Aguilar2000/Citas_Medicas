
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Medico {
  idMedico: number;
  idUsuario: number;
  nombres: string;
  apellidos: string;
  numeroJVPM: string;
  estado: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MedicosService {

  private readonly apiUrl = 'http://localhost:5031/Citas/medicos';

  constructor(private http: HttpClient) {}

  getMedicos(): Observable<Medico[]> {
    return this.http.get<Medico[]>(this.apiUrl);
  }
}
