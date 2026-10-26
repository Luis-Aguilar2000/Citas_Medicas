import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/Auth/auth.service';

@Component({
  selector: 'app-inicio',
  standalone: true,

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl: './inicio.html',
  styleUrl: './inicio.css'
})
export class Inicio {
  private readonly authService = inject(AuthService);

  readonly usuario = this.authService.obtenerUsuario();

  get nombreCompleto(): string {
    return [
      this.usuario?.nombres,
      this.usuario?.apellidos
    ].filter(Boolean).join(' ') || 'Usuario';
  }
}