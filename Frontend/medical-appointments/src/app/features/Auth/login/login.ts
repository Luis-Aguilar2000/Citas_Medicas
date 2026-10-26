import { Component, inject, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { AuthService } from '../../../core/Auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  correo = '';
  password = '';

  readonly cargando = signal(false);
  readonly error = signal('');
  readonly mostrarPassword = signal(false);

  iniciarSesion(formulario: NgForm): void {
    if (this.cargando()) {
      return;
    }

    if (formulario.invalid) {
      formulario.control.markAllAsTouched();
      return;
    }

    this.error.set('');
    this.cargando.set(true);

    this.auth.login(
      this.correo.trim(),
      this.password
    ).pipe(
      finalize(() => this.cargando.set(false))
    ).subscribe({
      next: () => {
        this.password = '';
        void this.router.navigateByUrl('/inicio');
      },
      error: (error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === 401) {
            this.error.set('Correo o contraseña incorrectos.');
          } else if (error.status === 0) {
            this.error.set(
              'No se pudo conectar con el servidor. Verifica que la API esté encendida y permita el acceso desde Angular.'
            );
          } else {
            this.error.set(
              'No se pudo iniciar sesión. Inténtalo nuevamente.'
            );
          }
        } else {
          this.error.set(
            'La respuesta del servidor no contiene una sesión válida.'
          );
        }
      }
    });
  }
}