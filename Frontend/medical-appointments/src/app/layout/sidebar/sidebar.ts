import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../../core/Auth/auth.service';


@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class Sidebar {
  @Input() colapsado = false;
  @Output() cambiarEstado = new EventEmitter<void>();

  private readonly authService = inject(AuthService);

  pacientesAbierto = false;
  identidadAbierto = false;
  expedientesAbierto = false;
  tratamientosAbierto = false;
  facturacionAbierto = false;

  menuFlotante: string | null = null;
  cerrandoFlotante = false;

  private temporizadorCierre: ReturnType<typeof setTimeout> | null = null;
  private temporizadorAnimacion: ReturnType<typeof setTimeout> | null = null;

  constructor(private cdr: ChangeDetectorRef) {}

  cerrarSesion(): void {
    this.cerrarFlotanteInmediato();
    this.authService.cerrarSesion();
  }

  // ALTERNAR ESTADO DEL SIDEBAR
  toggleSidebar(): void {
    this.cerrarFlotanteInmediato();
    this.cambiarEstado.emit();
  }

  // MENÚ DESPLEGABLE: PACIENTES Y AGENDAS
  togglePacientes(): void {
    if (this.colapsado) return;
    this.pacientesAbierto = !this.pacientesAbierto;
  }

  // MENÚ DESPLEGABLE: IDENTIDAD Y USUARIOS
  toggleIdentidad(): void {
    if (this.colapsado) return;
    this.identidadAbierto = !this.identidadAbierto;
  }

  // MENÚ DESPLEGABLE: EXPEDIENTES CLÍNICOS
  toggleExpedientes(): void {
    if (this.colapsado) return;
    this.expedientesAbierto = !this.expedientesAbierto;
  }

  // MENÚ DESPLEGABLE: TRATAMIENTOS Y LAB
  toggleTratamientos(): void {
    if (this.colapsado) return;
    this.tratamientosAbierto = !this.tratamientosAbierto;
  }

  // MENÚ DESPLEGABLE: FACTURACIÓN
  toggleFacturacion(): void {
    if (this.colapsado) return;
    this.facturacionAbierto = !this.facturacionAbierto;
  }

  // ABRIR MENÚ FLOTANTE (MODO COLAPSADO)
  abrirFlotante(nombre: string): void {
    if (!this.colapsado) return;

    this.cancelarCierre();
    this.cerrandoFlotante = false;
    this.menuFlotante = nombre;
    this.cdr.detectChanges();
  }

  // MANTENER MENÚ FLOTANTE ACTIVO AL HACER HOVER
  mantenerFlotante(): void {
    if (!this.colapsado) return;

    this.cancelarCierre();
    this.cerrandoFlotante = false;
  }

  // PROGRAMAR CIERRE DEL MENÚ FLOTANTE
  programarCierre(): void {
    if (!this.colapsado) return;

    this.cancelarCierre();

    this.temporizadorCierre = setTimeout(() => {
      this.cerrandoFlotante = true;
      this.cdr.detectChanges();

      this.temporizadorAnimacion = setTimeout(() => {
        this.menuFlotante = null;
        this.cerrandoFlotante = false;
        this.temporizadorAnimacion = null;
        this.cdr.detectChanges();
      }, 220);
    }, 250);
  }

  // CANCELAR TEMPORIZADORES DE CIERRE
  cancelarCierre(): void {
    if (this.temporizadorCierre !== null) {
      clearTimeout(this.temporizadorCierre);
      this.temporizadorCierre = null;
    }

    if (this.temporizadorAnimacion !== null) {
      clearTimeout(this.temporizadorAnimacion);
      this.temporizadorAnimacion = null;
    }
  }

  // CERRAR FLOTANTE CON ANIMACIÓN
  cerrarFlotante(): void {
    this.cancelarCierre();
    this.cerrandoFlotante = true;
    this.cdr.detectChanges();

    this.temporizadorAnimacion = setTimeout(() => {
      this.menuFlotante = null;
      this.cerrandoFlotante = false;
      this.temporizadorAnimacion = null;
      this.cdr.detectChanges();
    }, 220);
  }

  // CERRAR FLOTANTE DE FORMA INMEDIATA
  cerrarFlotanteInmediato(): void {
    this.cancelarCierre();
    this.menuFlotante = null;
    this.cerrandoFlotante = false;
    this.cdr.detectChanges();
  }
}