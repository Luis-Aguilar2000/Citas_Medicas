import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ConsultoriosService, Consultorio } from '../../../core/services/MicroPacientesAgendas/consultorios';
import { BusquedaService } from '../../../core/services/Compartidos/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-consultorios',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonesAcciones, Paginacion],
  templateUrl: './consultorios.html',
  styleUrl: './consultorios.css'
})
export class Consultorios implements OnInit {
  // LISTA Y PAGINACIÃ“N
  consultorios = signal<Consultorio[]>([]);
  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  // FORMULARIO Y ESTADOS
  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  consultorioSeleccionado: Consultorio | null = null;
  nuevoConsultorio: Consultorio = this.crearConsultorioVacio();

  constructor(
    private consultoriosService: ConsultoriosService,
    private busquedaService: BusquedaService
  ) {}

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar consultorios...');
    this.cargarConsultorios();
  }

  // CARGAR Y PAGINAR
  cargarConsultorios(): void {
    this.consultoriosService.getConsultorios(this.paginaActual, this.tamanoPagina).subscribe({
      next: (resultado) => {
        this.consultorios.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
        this.tamanoPagina = resultado.pageSize;
      },
      error: (error) => console.error('Error cargando consultorios:', error)
    });
  }

  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarConsultorios();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarConsultorios();
  }

  // ACCIONES DEL MODAL
  abrirNuevoConsultorio(): void {
    this.modoFormulario = 'nuevo';
    this.consultorioSeleccionado = null;
    this.nuevoConsultorio = this.crearConsultorioVacio();
    this.mostrarFormulario = true;
  }

  verConsultorio(consultorio: Consultorio): void {
    this.consultoriosService.getConsultorioById(consultorio.idConsultorio).subscribe({
      next: (data) => {
        this.modoFormulario = 'ver';
        this.consultorioSeleccionado = data;
        this.nuevoConsultorio = { ...data };
        this.mostrarFormulario = true;
      },
      error: (error) => console.error('Error obteniendo consultorio:', error)
    });
  }

  editarConsultorio(consultorio: Consultorio): void {
    this.consultoriosService.getConsultorioById(consultorio.idConsultorio).subscribe({
      next: (data) => {
        this.modoFormulario = 'editar';
        this.consultorioSeleccionado = data;
        this.nuevoConsultorio = { ...data };
        this.mostrarFormulario = true;
      },
      error: (error) => console.error('Error obteniendo consultorio:', error)
    });
  }

  eliminarConsultorio(consultorio: Consultorio): void {
    if (!confirm(`Â¿Desea eliminar el consultorio ${consultorio.nombre}?`)) return;

    this.consultoriosService.deleteConsultorio(consultorio.idConsultorio).subscribe({
      next: () => {
        if (this.consultorios().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarConsultorios();
      },
      error: (error) => console.error('Error eliminando consultorio:', error)
    });
  }

  guardarConsultorio(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarConsultorio();
    } else if (this.modoFormulario === 'editar') {
      this.actualizarConsultorio();
    }
  }

  agregarConsultorio(): void {
    this.consultoriosService.addConsultorio(this.nuevoConsultorio).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarConsultorios();
      },
      error: (error) => console.error('Error agregando consultorio:', error)
    });
  }

  actualizarConsultorio(): void {
    this.consultoriosService.updateConsultorio(this.nuevoConsultorio).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarConsultorios();
      },
      error: (error) => console.error('Error actualizando consultorio:', error)
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.consultorioSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoConsultorio = this.crearConsultorioVacio();
  }

  private crearConsultorioVacio(): Consultorio {
    return {
      idConsultorio: 0,
      nombre: '',
      numeroConsultorio: '',
      piso: '',
      ubicacion: '',
      descripcion: '',
      estado: true,
      fechaRegistro: new Date().toISOString()
    };
  }
}
