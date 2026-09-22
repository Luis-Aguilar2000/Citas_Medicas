import {
  Component,
  Input
} from '@angular/core';

import {
  BusquedaService,
  SugerenciaBusqueda
} from '../../core/services/busqueda';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {


  // =========================================
  // SIDEBAR
  // =========================================

  @Input()
  sidebarColapsado = false;


  // =========================================
  // CONSTRUCTOR
  // =========================================

  constructor(
    public busquedaService: BusquedaService
  ) {}


  // =========================================
  // ESCRIBIR
  // =========================================

  onEscribir(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    this.busquedaService.escribir(
      input.value
    );

  }


  // =========================================
  // ENTER
  // =========================================

  onEnter(): void {

    this.realizarBusqueda();

  }


  // =========================================
  // CLIC EN LUPA
  // =========================================

  onClickBuscar(): void {

    this.realizarBusqueda();

  }


  // =========================================
  // REALIZAR BÚSQUEDA
  // =========================================

  private realizarBusqueda(): void {

    this.busquedaService.buscar();

  }


  // =========================================
  // SELECCIONAR SUGERENCIA
  // =========================================

  seleccionarSugerencia(
    sugerencia: SugerenciaBusqueda
  ): void {

    this.busquedaService
      .seleccionarSugerencia(
        sugerencia
      );

  }


  // =========================================
  // FOCUS
  // =========================================

  onFocus(): void {

    if (
      this.busquedaService
        .sugerencias()
        .length > 0
    ) {

      this.busquedaService
        .mostrarSugerencias
        .set(true);

    }

  }

}