import {
  Component,
  Input,
  OnDestroy
} from '@angular/core';

import {
  BusquedaService
} from '../../core/services/busqueda';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header implements OnDestroy {


  // =========================================
  // SIDEBAR
  // =========================================

  @Input()
  sidebarColapsado = false;


  // =========================================
  // TEMPORIZADOR
  // =========================================

  private temporizadorBusqueda:
    ReturnType<typeof setTimeout> | null = null;


  // =========================================
  // CONSTRUCTOR
  // =========================================

  constructor(
    public busquedaService: BusquedaService
  ) {}


  // =========================================
  // BUSCAR
  // =========================================

  onBuscar(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;

    const texto =
      input.value;


    if (
      this.temporizadorBusqueda
    ) {

      clearTimeout(
        this.temporizadorBusqueda
      );

    }


    this.temporizadorBusqueda =
      setTimeout(() => {

        this.busquedaService.buscar(
          texto
        );

      }, 350);

  }


  // =========================================
  // DESTROY
  // =========================================

  ngOnDestroy(): void {

    if (
      this.temporizadorBusqueda
    ) {

      clearTimeout(
        this.temporizadorBusqueda
      );

    }

  }

}