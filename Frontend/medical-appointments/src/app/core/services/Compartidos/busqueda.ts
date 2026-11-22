import { Injectable, signal } from '@angular/core';

export interface SugerenciaBusqueda {
  texto: string;
  descripcion?: string;
  filtro?: string;
}

@Injectable({
  providedIn: 'root'
})
export class BusquedaService {
  entrada = signal<string>('');
  texto = signal<string>('');
  filtroSeleccionado = signal<string>('');
  placeholder = signal<string>('Buscar');
  sugerencias = signal<SugerenciaBusqueda[]>([]);
  mostrarSugerencias = signal<boolean>(false);

  // Actualiza lo que el usuario escribe sin disparar la búsqueda en la tabla
  escribir(texto: string): void {
    this.entrada.set(texto);

    if (texto.trim() === '') {
      this.sugerencias.set([]);
      this.mostrarSugerencias.set(false);
    }
  }

  establecerSugerencias(sugerencias: SugerenciaBusqueda[]): void {
    this.sugerencias.set(sugerencias);
    this.mostrarSugerencias.set(sugerencias.length > 0);
  }

  // Ejecuta la búsqueda general (Enter o lupa)
  buscar(texto?: string): void {
    const valor = texto !== undefined ? texto : this.entrada();
    const valorLimpio = valor.trim();

    this.entrada.set(valorLimpio);
    this.filtroSeleccionado.set(''); // Limpia el filtro específico al hacer búsqueda general
    this.texto.set(valorLimpio);
    this.cerrarSugerencias();
  }

  // Aplica el filtro exacto al seleccionar una sugerencia
  seleccionarSugerencia(sugerencia: SugerenciaBusqueda): void {
    this.entrada.set(sugerencia.texto);
    this.filtroSeleccionado.set(sugerencia.filtro ?? '');
    this.texto.set(sugerencia.texto);
    this.cerrarSugerencias();
  }

  cerrarSugerencias(): void {
    this.mostrarSugerencias.set(false);
  }

  configurar(placeholder: string): void {
    this.placeholder.set(placeholder);
    this.limpiar();
  }

  limpiar(): void {
    this.entrada.set('');
    this.texto.set('');
    this.filtroSeleccionado.set('');
    this.sugerencias.set([]);
    this.mostrarSugerencias.set(false);
  }
}