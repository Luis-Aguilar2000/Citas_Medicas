import {
  Injectable,
  signal
} from '@angular/core';


/* =========================================
   SUGERENCIA DE BÚSQUEDA
========================================= */

export interface SugerenciaBusqueda {

  // Texto principal que verá el usuario
  texto: string;

  // Información secundaria
  descripcion?: string;

  // Filtro exacto para la búsqueda
  filtro?: string;

}


@Injectable({
  providedIn: 'root'
})
export class BusquedaService {


  // =========================================
  // TEXTO QUE SE ESTÁ ESCRIBIENDO
  // =========================================

  entrada =
    signal<string>('');


  // =========================================
  // TEXTO CONFIRMADO
  // =========================================

  texto =
    signal<string>('');


  // =========================================
  // FILTRO EXACTO SELECCIONADO
  // =========================================

  filtroSeleccionado =
    signal<string>('');


  // =========================================
  // PLACEHOLDER
  // =========================================

  placeholder =
    signal<string>('Buscar');


  // =========================================
  // SUGERENCIAS
  // =========================================

  sugerencias =
    signal<SugerenciaBusqueda[]>([]);


  // =========================================
  // MOSTRAR SUGERENCIAS
  // =========================================

  mostrarSugerencias =
    signal<boolean>(false);


  // =========================================
  // ESCRIBIR
  // =========================================

  escribir(
    texto: string
  ): void {

    /*
     * IMPORTANTE:
     *
     * Aquí solamente modificamos entrada().
     *
     * NO modificamos texto().
     * NO modificamos filtroSeleccionado().
     *
     * Por lo tanto escribir NO debe
     * modificar la tabla.
     */

    this.entrada.set(
      texto
    );


    if (
      texto.trim() === ''
    ) {

      this.sugerencias.set([]);

      this.mostrarSugerencias.set(false);

    }

  }


  // =========================================
  // ESTABLECER SUGERENCIAS
  // =========================================

  establecerSugerencias(
    sugerencias: SugerenciaBusqueda[]
  ): void {

    this.sugerencias.set(
      sugerencias
    );


    this.mostrarSugerencias.set(
      sugerencias.length > 0
    );

  }


  // =========================================
  // BÚSQUEDA MANUAL
  // ENTER O LUPA
  // =========================================

  buscar(
    texto?: string
  ): void {

    const valor =
      texto !== undefined
        ? texto
        : this.entrada();


    const valorLimpio =
      valor.trim();


    this.entrada.set(
      valorLimpio
    );


    /*
     * Al usar Enter o la lupa
     * hacemos búsqueda general.
     */

    this.filtroSeleccionado.set('');


    this.texto.set(
      valorLimpio
    );


    this.cerrarSugerencias();

  }


  // =========================================
  // SELECCIONAR SUGERENCIA
  // =========================================

  seleccionarSugerencia(
    sugerencia: SugerenciaBusqueda
  ): void {

    /*
     * El input mostrará el nombre.
     */

    this.entrada.set(
      sugerencia.texto
    );


    /*
     * Guardamos el filtro exacto.
     *
     * Ejemplo:
     *
     * IdPaciente == 5
     */

    this.filtroSeleccionado.set(
      sugerencia.filtro ?? ''
    );


    /*
     * texto() funciona como señal
     * de búsqueda confirmada.
     */

    this.texto.set(
      sugerencia.texto
    );


    this.cerrarSugerencias();

  }


  // =========================================
  // CERRAR SUGERENCIAS
  // =========================================

  cerrarSugerencias(): void {

    this.mostrarSugerencias.set(false);

  }


  // =========================================
  // CONFIGURAR
  // =========================================

  configurar(
    placeholder: string
  ): void {

    this.placeholder.set(
      placeholder
    );

    this.entrada.set('');

    this.texto.set('');

    this.filtroSeleccionado.set('');

    this.sugerencias.set([]);

    this.mostrarSugerencias.set(false);

  }


  // =========================================
  // LIMPIAR
  // =========================================

  limpiar(): void {

    this.entrada.set('');

    this.texto.set('');

    this.filtroSeleccionado.set('');

    this.sugerencias.set([]);

    this.mostrarSugerencias.set(false);

  }

}