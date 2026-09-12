import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
export class ConsultoriosService {

  private apiUrl =
    'https://localhost:7250/Consultorios';

  constructor(
    private http: HttpClient
  ) {}


  // =========================================
  // OBTENER TODOS
  // =========================================

  getConsultorios():
    Observable<Consultorio[]> {

    return this.http.get<Consultorio[]>(
      this.apiUrl
    );
  }


  // =========================================
  // OBTENER POR ID
  // =========================================

  getConsultorioById(
    idConsultorio: number
  ): Observable<Consultorio> {

    return this.http.get<Consultorio>(
      `${this.apiUrl}/${idConsultorio}`
    );
  }


  // =========================================
  // AGREGAR
  // =========================================

  addConsultorio(
    consultorio: Consultorio
  ): Observable<boolean> {

    return this.http.post<boolean>(
      this.apiUrl,
      consultorio
    );
  }


  // =========================================
  // ACTUALIZAR
  // =========================================

  updateConsultorio(
    consultorio: Consultorio
  ): Observable<boolean> {

    return this.http.put<boolean>(
      `${this.apiUrl}/${consultorio.idConsultorio}`,
      consultorio
    );
  }


  // =========================================
  // ELIMINAR
  // =========================================

  deleteConsultorio(
    idConsultorio: number
  ): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.apiUrl}/${idConsultorio}`
    );
  }

}