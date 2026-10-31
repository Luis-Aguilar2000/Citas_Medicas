import { Component, OnInit, OnDestroy, signal, effect, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Servicio, ServiciosService } from '../../../core/services/servicios';
import { BusquedaService } from '../../../core/services/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-servicios',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],
  templateUrl: './servicios.html',
  styleUrl: './servicios.css'
})
export class Servicios implements OnInit, OnDestroy {
  servicios = signal<Servicio[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  mostrarFormulario = false;
  guardando = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  servicioSeleccionado: Servicio | null = null;
  nuevoServicio: Servicio = this.crearServicioVacio();

  constructor(
    private serviciosService: ServiciosService,
    private busquedaService: BusquedaService
  ) {
    effect(() => {
      this.busquedaService.texto();
      untracked(() => {
        this.paginaActual = 1;
        this.cargarServicios();
      });
    });
  }

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar servicios...');
  }

  ngOnDestroy(): void {
    this.busquedaService.establecerSugerencias([]);
  }

  cargarServicios(): void {
    const texto = this.busquedaService.texto();
    const filtro = this.crearFiltroBusqueda(texto);

    this.serviciosService.getServicios(this.paginaActual, this.tamanoPagina, filtro).subscribe({
      next: resultado => {
        this.servicios.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: error => console.error('Error cargando servicios:', error)
    });
  }

  abrirNuevoServicio(): void {
    this.modoFormulario = 'nuevo';
    this.servicioSeleccionado = null;
    this.nuevoServicio = this.crearServicioVacio();
    this.mostrarFormulario = true;
  }

  verServicio(servicio: Servicio): void {
    this.modoFormulario = 'ver';
    this.servicioSeleccionado = servicio;
    this.nuevoServicio = { ...servicio };
    this.mostrarFormulario = true;
  }

  editarServicio(servicio: Servicio): void {
    this.modoFormulario = 'editar';
    this.servicioSeleccionado = servicio;
    this.nuevoServicio = { ...servicio };
    this.mostrarFormulario = true;
  }

  guardarServicio(): void {
    const nombre = this.nuevoServicio.nombre.trim();

    if (!nombre) {
      alert('Ingrese el nombre del servicio.');
      return;
    }

    if (this.nuevoServicio.precio < 0) {
      alert('El precio no puede ser negativo.');
      return;
    }

    this.nuevoServicio.nombre = nombre;

    if (this.modoFormulario === 'nuevo') {
      this.agregarServicio();
      return;
    }

    if (this.modoFormulario === 'editar') {
      this.actualizarServicio();
    }
  }

  private agregarServicio(): void {
    this.guardando = true;

    this.serviciosService.addServicio(this.nuevoServicio).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarServicios();
      },
      error: error => {
        this.guardando = false;
        console.error('Error agregando servicio:', error);
        alert('No se pudo registrar el servicio.');
      }
    });
  }

  private actualizarServicio(): void {
    this.guardando = true;

    this.serviciosService.updateServicio(this.nuevoServicio).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.cargarServicios();
      },
      error: error => {
        this.guardando = false;
        console.error('Error actualizando servicio:', error);
        alert('No se pudo actualizar el servicio.');
      }
    });
  }

  eliminarServicio(servicio: Servicio): void {
    const confirmar = confirm(`¿Desea eliminar el servicio "${servicio.nombre}"?`);
    if (!confirmar) return;

    this.serviciosService.deleteServicio(servicio.idServicio).subscribe({
      next: () => {
        if (this.servicios().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarServicios();
      },
      error: error => {
        console.error('Error eliminando servicio:', error);
        alert('No se pudo eliminar el servicio.');
      }
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.guardando = false;
    this.modoFormulario = 'nuevo';
    this.servicioSeleccionado = null;
    this.nuevoServicio = this.crearServicioVacio();
  }

  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarServicios();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarServicios();
  }

  private crearFiltroBusqueda(texto: string): string {
    const valor = texto.trim();
    if (!valor) return '';

    const seguro = valor.replace(/\\/g, '\\\\').replace(/"/g, '\\"');
    const numero = Number(valor);

    if (!Number.isNaN(numero)) {
      return `IdServicio == ${numero} || Nombre.Contains("${seguro}") || Descripcion.Contains("${seguro}")`;
    }

    return `Nombre.Contains("${seguro}") || Descripcion.Contains("${seguro}")`;
  }

  private crearServicioVacio(): Servicio {
    return {
      idServicio: 0,
      nombre: '',
      descripcion: '',
      precio: 0,
      estado: true
    };
  }
}