import {
  Component,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  PacientesService,
  Paciente
} from '../../../core/services/pacientes';

import {
  BotonesAcciones
} from '../../../shared/components/botones-acciones/botones-acciones';


@Component({
  selector: 'app-pacientes',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones
  ],

  templateUrl: './pacientes.html',
  styleUrl: './pacientes.css'
})
export class Pacientes implements OnInit {


  // =========================================
  // DATOS
  // =========================================

  pacientes = signal<Paciente[]>([]);


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

  pacienteSeleccionado:
    Paciente | null = null;

  nuevoPaciente:
    Paciente = this.crearPacienteVacio();


  // =========================================
  // CONSTRUCTOR
  // =========================================

  constructor(
    private pacientesService: PacientesService
  ) {}


  // =========================================
  // INICIO
  // =========================================

  ngOnInit(): void {

    this.cargarPacientes();

  }


  // =========================================
  // CARGAR PACIENTES
  // =========================================

  cargarPacientes(): void {

    this.pacientesService
      .getPacientes(
        this.paginaActual,
        this.tamanoPagina
      )
      .subscribe({

        next: (resultado) => {

          this.pacientes.set(
            resultado.data
          );

          this.totalRegistros =
            resultado.totalRecords;

          this.totalPaginas =
            resultado.totalPages;

          this.paginaActual =
            resultado.currentPage;

        },

        error: (error) => {

          console.error(
            'Error cargando pacientes:',
            error
          );

        }

      });

  }


  // =========================================
  // NUEVO PACIENTE
  // =========================================

  abrirNuevoPaciente(): void {

    this.modoFormulario = 'nuevo';

    this.pacienteSeleccionado = null;

    this.nuevoPaciente =
      this.crearPacienteVacio();

    this.mostrarFormulario = true;

  }


  // =========================================
  // VER PACIENTE
  // =========================================

  verPaciente(
    paciente: Paciente
  ): void {

    this.modoFormulario = 'ver';

    this.pacienteSeleccionado =
      paciente;

    this.nuevoPaciente = {
      ...paciente
    };

    this.mostrarFormulario = true;

  }


  // =========================================
  // EDITAR PACIENTE
  // =========================================

  editarPaciente(
    paciente: Paciente
  ): void {

    this.modoFormulario = 'editar';

    this.pacienteSeleccionado =
      paciente;

    this.nuevoPaciente = {
      ...paciente
    };

    this.mostrarFormulario = true;

  }


  // =========================================
  // ELIMINAR PACIENTE
  // =========================================

  eliminarPaciente(
    paciente: Paciente
  ): void {

    const nombreCompleto =
      `${paciente.nombres} ${paciente.apellidos}`;

    const confirmar = confirm(
      `¿Desea eliminar al paciente ${nombreCompleto}?`
    );

    if (!confirmar) {
      return;
    }


    this.pacientesService
      .deletePaciente(
        paciente.idPaciente
      )
      .subscribe({

        next: () => {

          /*
           Si eliminamos el único registro
           de una página distinta de la primera,
           retrocedemos una página.
          */

          if (
            this.pacientes().length === 1 &&
            this.paginaActual > 1
          ) {

            this.paginaActual--;

          }

          this.cargarPacientes();

        },

        error: (error) => {

          console.error(
            'Error eliminando paciente:',
            error
          );

        }

      });

  }


  // =========================================
  // GUARDAR PACIENTE
  // =========================================

  guardarPaciente(): void {

    if (
      this.modoFormulario === 'nuevo'
    ) {

      this.agregarPaciente();

      return;

    }


    if (
      this.modoFormulario === 'editar'
    ) {

      this.actualizarPaciente();

    }

  }


  // =========================================
  // AGREGAR PACIENTE
  // =========================================

  private agregarPaciente(): void {

    this.pacientesService
      .addPaciente(
        this.nuevoPaciente
      )
      .subscribe({

        next: () => {

          this.cerrarFormulario();

          /*
           Al registrar uno nuevo volvemos
           a la primera página.
          */

          this.paginaActual = 1;

          this.cargarPacientes();

        },

        error: (error) => {

          console.error(
            'Error agregando paciente:',
            error
          );

        }

      });

  }


  // =========================================
  // ACTUALIZAR PACIENTE
  // =========================================

  private actualizarPaciente(): void {

    this.pacientesService
      .updatePaciente(
        this.nuevoPaciente
      )
      .subscribe({

        next: () => {

          this.cerrarFormulario();

          /*
           En edición conservamos
           la página actual.
          */

          this.cargarPacientes();

        },

        error: (error) => {

          console.error(
            'Error actualizando paciente:',
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

    this.pacienteSeleccionado = null;

    this.limpiarFormulario();

  }


  // =========================================
  // LIMPIAR FORMULARIO
  // =========================================

  limpiarFormulario(): void {

    this.nuevoPaciente =
      this.crearPacienteVacio();

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

    this.cargarPacientes();

  }


  // =========================================
  // PÁGINA ANTERIOR
  // =========================================

  paginaAnterior(): void {

    if (
      this.paginaActual <= 1
    ) {

      return;

    }

    this.paginaActual--;

    this.cargarPacientes();

  }


  // =========================================
  // PÁGINA SIGUIENTE
  // =========================================

  paginaSiguiente(): void {

    if (
      this.paginaActual >=
      this.totalPaginas
    ) {

      return;

    }

    this.paginaActual++;

    this.cargarPacientes();

  }


  // =========================================
  // OBTENER PÁGINAS
  // =========================================

  obtenerPaginas(): number[] {

    if (
      this.totalPaginas <= 0
    ) {

      return [];

    }

    return Array.from(
      {
        length:
          this.totalPaginas
      },

      (_, indice) =>
        indice + 1
    );

  }


  // =========================================
  // CAMBIAR TAMAÑO DE PÁGINA
  // =========================================

  cambiarTamanoPagina(): void {

    /*
     Siempre regresamos a página 1
     cuando cambia la cantidad.
    */

    this.paginaActual = 1;

    this.cargarPacientes();

  }


  // =========================================
  // REGISTRO INICIAL
  // =========================================

  obtenerRegistroInicial(): number {

    if (
      this.totalRegistros === 0
    ) {

      return 0;

    }

    return (
      (this.paginaActual - 1) *
      this.tamanoPagina
    ) + 1;

  }


  // =========================================
  // REGISTRO FINAL
  // =========================================

  obtenerRegistroFinal(): number {

    if (
      this.totalRegistros === 0
    ) {

      return 0;

    }

    return Math.min(

      this.paginaActual *
      this.tamanoPagina,

      this.totalRegistros

    );

  }


  // =========================================
  // CREAR PACIENTE VACÍO
  // =========================================

  private crearPacienteVacio():
    Paciente {

    return {

      idPaciente: 0,

      nombres: '',

      apellidos: '',

      fechaNacimiento: '',

      sexo: '',

      dui: '',

      telefono: '',

      correo: '',

      direccion: '',

      fechaRegistro:
        new Date().toISOString()

    };

  }

}