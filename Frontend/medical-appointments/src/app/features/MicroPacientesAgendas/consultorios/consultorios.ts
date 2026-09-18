import {
  Component,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  ConsultoriosService,
  Consultorio
} from '../../../core/services/consultorios';

import {
  BotonesAcciones
} from '../../../shared/components/botones-acciones/botones-acciones';

import {
  Paginacion
} from '../../../shared/components/paginacion/paginacion';


@Component({
  selector: 'app-consultorios',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],

  templateUrl: './consultorios.html',
  styleUrl: './consultorios.css'
})
export class Consultorios implements OnInit {


  // =========================================
  // LISTA DE CONSULTORIOS
  // =========================================

  consultorios = signal<Consultorio[]>([]);


  // =========================================
  // PAGINACIÓN
  // =========================================

  paginaActual = 1;

  tamanoPagina = 10;

  totalRegistros = 0;

  totalPaginas = 0;


  // =========================================
  // FORMULARIO
  // =========================================

  mostrarFormulario = false;

  modoFormulario:
    'nuevo' |
    'ver' |
    'editar' = 'nuevo';

  consultorioSeleccionado:
    Consultorio | null = null;

  nuevoConsultorio:
    Consultorio = this.crearConsultorioVacio();


  // =========================================
  // CONSTRUCTOR
  // =========================================

  constructor(
    private consultoriosService: ConsultoriosService
  ) {}


  // =========================================
  // INIT
  // =========================================

  ngOnInit(): void {

    this.cargarConsultorios();

  }


  // =========================================
  // CARGAR CONSULTORIOS PAGINADOS
  // =========================================

  cargarConsultorios(): void {

    this.consultoriosService
      .getConsultorios(
        this.paginaActual,
        this.tamanoPagina
      )
      .subscribe({

        next: (resultado) => {

          console.log(
            'CONSULTORIOS RECIBIDOS:',
            resultado
          );

          this.consultorios.set(
            resultado.data
          );

          this.totalRegistros =
            resultado.totalRecords;

          this.totalPaginas =
            resultado.totalPages;

          this.paginaActual =
            resultado.currentPage;

          this.tamanoPagina =
            resultado.pageSize;

        },

        error: (error) => {

          console.error(
            'Error cargando consultorios:',
            error
          );

        }

      });

  }


  // =========================================
  // CAMBIAR PÁGINA
  // =========================================

  cambiarPagina(
    pagina: number
  ): void {

    if (
      pagina < 1 ||
      pagina > this.totalPaginas ||
      pagina === this.paginaActual
    ) {
      return;
    }

    this.paginaActual = pagina;

    this.cargarConsultorios();

  }


  // =========================================
  // CAMBIAR TAMAÑO DE PÁGINA
  // =========================================

  cambiarTamanoPagina(
    tamano: number
  ): void {

    this.tamanoPagina = tamano;

    this.paginaActual = 1;

    this.cargarConsultorios();

  }


  // =========================================
  // NUEVO CONSULTORIO
  // =========================================

  abrirNuevoConsultorio(): void {

    this.modoFormulario = 'nuevo';

    this.consultorioSeleccionado = null;

    this.nuevoConsultorio =
      this.crearConsultorioVacio();

    this.mostrarFormulario = true;

  }


  // =========================================
  // VER CONSULTORIO
  // =========================================

  verConsultorio(
    consultorio: Consultorio
  ): void {

    this.consultoriosService
      .getConsultorioById(
        consultorio.idConsultorio
      )
      .subscribe({

        next: (data) => {

          this.modoFormulario = 'ver';

          this.consultorioSeleccionado =
            data;

          this.nuevoConsultorio = {
            ...data
          };

          this.mostrarFormulario = true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo consultorio:',
            error
          );

        }

      });

  }


  // =========================================
  // EDITAR CONSULTORIO
  // =========================================

  editarConsultorio(
    consultorio: Consultorio
  ): void {

    this.consultoriosService
      .getConsultorioById(
        consultorio.idConsultorio
      )
      .subscribe({

        next: (data) => {

          this.modoFormulario = 'editar';

          this.consultorioSeleccionado =
            data;

          this.nuevoConsultorio = {
            ...data
          };

          this.mostrarFormulario = true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo consultorio:',
            error
          );

        }

      });

  }


  // =========================================
  // ELIMINAR CONSULTORIO
  // =========================================

  eliminarConsultorio(
    consultorio: Consultorio
  ): void {

    const confirmar = confirm(
      `¿Desea eliminar el consultorio ${consultorio.nombre}?`
    );

    if (!confirmar) {
      return;
    }


    this.consultoriosService
      .deleteConsultorio(
        consultorio.idConsultorio
      )
      .subscribe({

        next: () => {

          console.log(
            'Consultorio eliminado correctamente'
          );

          if (
            this.consultorios().length === 1 &&
            this.paginaActual > 1
          ) {

            this.paginaActual--;

          }

          this.cargarConsultorios();

        },

        error: (error) => {

          console.error(
            'Error eliminando consultorio:',
            error
          );

        }

      });

  }


  // =========================================
  // GUARDAR CONSULTORIO
  // =========================================

  guardarConsultorio(): void {

    if (
      this.modoFormulario === 'nuevo'
    ) {

      this.agregarConsultorio();

      return;

    }


    if (
      this.modoFormulario === 'editar'
    ) {

      this.actualizarConsultorio();

    }

  }


  // =========================================
  // AGREGAR CONSULTORIO
  // =========================================

  agregarConsultorio(): void {

    this.consultoriosService
      .addConsultorio(
        this.nuevoConsultorio
      )
      .subscribe({

        next: () => {

          console.log(
            'Consultorio agregado correctamente'
          );

          this.cerrarFormulario();

          this.paginaActual = 1;

          this.cargarConsultorios();

        },

        error: (error) => {

          console.error(
            'Error agregando consultorio:',
            error
          );

        }

      });

  }


  // =========================================
  // ACTUALIZAR CONSULTORIO
  // =========================================

  actualizarConsultorio(): void {

    this.consultoriosService
      .updateConsultorio(
        this.nuevoConsultorio
      )
      .subscribe({

        next: () => {

          console.log(
            'Consultorio actualizado correctamente'
          );

          this.cerrarFormulario();

          this.cargarConsultorios();

        },

        error: (error) => {

          console.error(
            'Error actualizando consultorio:',
            error
          );

        }

      });

  }


  // =========================================
  // CERRAR FORMULARIO
  // =========================================

  cerrarFormulario(): void {

    this.mostrarFormulario = false;

    this.modoFormulario = 'nuevo';

    this.consultorioSeleccionado = null;

    this.limpiarFormulario();

  }


  // =========================================
  // LIMPIAR FORMULARIO
  // =========================================

  limpiarFormulario(): void {

    this.nuevoConsultorio =
      this.crearConsultorioVacio();

  }


  // =========================================
  // CONSULTORIO VACÍO
  // =========================================

  private crearConsultorioVacio():
    Consultorio {

    return {

      idConsultorio: 0,

      nombre: '',

      numeroConsultorio: '',

      piso: '',

      ubicacion: '',

      descripcion: '',

      estado: true,

      fechaRegistro:
        new Date().toISOString()

    };

  }

}