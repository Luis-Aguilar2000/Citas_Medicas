import {
  Injectable,
  signal
} from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class BusquedaService {


  // =========================================
  // TEXTO ACTUAL
  // =========================================

  texto = signal<string>('');


  // =========================================
  // PLACEHOLDER
  // =========================================

  placeholder = signal<string>('Buscar');


  // =========================================
  // BUSCAR
  // =========================================

  buscar(
    texto: string
  ): void {

    this.texto.set(
      texto.trim()
    );

  }


  // =========================================
  // CONFIGURAR BUSCADOR
  // =========================================

  configurar(
    placeholder: string
  ): void {

    this.placeholder.set(
      placeholder
    );

    this.texto.set('');

  }


  // =========================================
  // LIMPIAR
  // =========================================

  limpiar(): void {

    this.texto.set('');

  }

}