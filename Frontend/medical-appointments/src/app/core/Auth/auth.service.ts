import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

export interface UsuarioSesion {
  idUsuario: number;
  nombres: string;
  apellidos: string;
  correo: string;
  rol: string;
}

export interface LoginResponse {
  token: string;
  expiracion: string;
  usuario: UsuarioSesion;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  readonly apiUrl = 'http://localhost:5144';

  private readonly storageKey = 'citas-medicas-sesion';

  login(correo: string, password: string) {
    return this.http.post<LoginResponse>(
      `${this.apiUrl}/Auth/login`,
      { correo, password }
    ).pipe(
      tap(respuesta => {
        const expiracion = Date.parse(respuesta.expiracion);

        if (
          !respuesta.token ||
          !respuesta.usuario ||
          !Number.isFinite(expiracion) ||
          expiracion <= Date.now()
        ) {
          throw new Error('La API devolvió una sesión inválida.');
        }

        sessionStorage.setItem(
          this.storageKey,
          JSON.stringify(respuesta)
        );
      })
    );
  }

  private obtenerSesion(): LoginResponse | null {
    const datos = sessionStorage.getItem(this.storageKey);

    if (!datos) {
      return null;
    }

    try {
      const sesion = JSON.parse(datos) as LoginResponse;
      const expiracion = Date.parse(sesion.expiracion);

      if (
        !sesion.token ||
        !sesion.usuario ||
        !Number.isFinite(expiracion) ||
        expiracion <= Date.now()
      ) {
        this.limpiarSesion();
        return null;
      }

      return sesion;
    } catch {
      this.limpiarSesion();
      return null;
    }
  }

  obtenerToken(): string | null {
    return this.obtenerSesion()?.token ?? null;
  }

  obtenerUsuario(): UsuarioSesion | null {
    return this.obtenerSesion()?.usuario ?? null;
  }

  estaAutenticado(): boolean {
    return this.obtenerToken() !== null;
  }

  limpiarSesion(): void {
    sessionStorage.removeItem(this.storageKey);
  }

  cerrarSesion(): void {
    this.limpiarSesion();
    void this.router.navigateByUrl('/login');
  }
}