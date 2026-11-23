
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface HorarioDisponible {
  idMedico: number;
  idEspecialidad: number;
  idConsultorio: number;
  fecha: string;
  horaInicio: string;
  horaFin: string;
}

@Injectable({
  providedIn: 'root'
})
export class HorariosMedicosService {

  private readonly apiUrl =
    'http://localhost:5031/HorariosMedicos';

  constructor(private http: HttpClient) {}

  getHorariosDisponibles(
    idMedico: number,
    idEspecialidad: number,
    fecha: string
  ): Observable<HorarioDisponible[]> {

    const params = new HttpParams()
      .set('idMedico', idMedico)
      .set('idEspecialidad', idEspecialidad)
      .set('fecha', fecha);

    return this.http.get<HorarioDisponible[]>(
      `${this.apiUrl}/disponibles`,
      { params }
    );
  }
}
