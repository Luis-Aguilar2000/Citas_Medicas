import { Component, Input, inject } from '@angular/core';
import { BusquedaService, SugerenciaBusqueda } from '../../core/services/busqueda';
import { AuthService } from '../../core/Auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  private readonly authService = inject(AuthService);

  readonly usuario = this.authService.obtenerUsuario();

  get nombreCompleto(): string {
    return [
      this.usuario?.nombres,
      this.usuario?.apellidos
    ].filter(Boolean).join(' ') || 'Usuario';
  }

  get iniciales(): string {
    const nombre = this.usuario?.nombres?.trim() ?? '';
    const apellido = this.usuario?.apellidos?.trim() ?? '';

    return (
      (nombre.charAt(0) + apellido.charAt(0)).toUpperCase() || 'US'
    );
  }

  // SIDEBAR
  @Input() sidebarColapsado = false;

  constructor(
    public busquedaService: BusquedaService
  ) {}

  // ESCRIBIR EN EL BUSCADOR
  onEscribir(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.busquedaService.escribir(input.value);
  }

  // CONFIRMAR CON ENTER
  onEnter(): void {
    this.realizarBusqueda();
  }

  // CLIC EN EL BOTÓN DE LUPA
  onClickBuscar(): void {
    this.realizarBusqueda();
  }

  // REALIZAR BÚSQUEDA GLOBAL
  private realizarBusqueda(): void {
    this.busquedaService.buscar();
  }

  // SELECCIONAR UNA SUGERENCIA DE LA PREDICCIÓN
  seleccionarSugerencia(sugerencia: SugerenciaBusqueda): void {
    this.busquedaService.seleccionarSugerencia(sugerencia);
  }

  // FOCO EN EL INPUT
  onFocus(): void {
    if (this.busquedaService.sugerencias().length > 0) {
      this.busquedaService.mostrarSugerencias.set(true);
    }
  }
}