import {
  Component,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  CitasService,
  Cita
} from '../../../core/services/citas';

import {
  PacientesService,
  Paciente
} from '../../../core/services/pacientes';

import {
  ConsultoriosService,
  Consultorio
} from '../../../core/services/consultorios';

import {
  EstadoCitasService,
  EstadoCita
} from '../../../core/services/estado-citas';

import {
  BotonesAcciones
} from '../../../shared/components/botones-acciones/botones-acciones';

import {
  Paginacion
} from '../../../shared/components/paginacion/paginacion';


@Component({
  selector: 'app-agenda-citas',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],

  templateUrl: './agenda-citas.html',
  styleUrl: './agenda-citas.css'
})
export class AgendaCitas implements OnInit {


  // =========================================
  // DATOS
  // =========================================

  citas = signal<Cita[]>([]);

  pacientes = signal<Paciente[]>([]);

  consultorios = signal<Consultorio[]>([]);

  estadosCitas = signal<EstadoCita[]>([]);


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

  citaSeleccionada:
    Cita | null = null;

  nuevaCita:
    Cita = this.crearCitaVacia();


  // =========================================
  // CONSTRUCTOR
  // =========================================

  constructor(
    private citasService: CitasService,
    private pacientesService: PacientesService,
    private consultoriosService: ConsultoriosService,
    private estadoCitasService: EstadoCitasService
  ) {}


  // =========================================
  // INICIO
  // =========================================

  ngOnInit(): void {

    this.cargarCitas();

    this.cargarPacientes();

    this.cargarConsultorios();

    this.cargarEstadosCitas();

  }


  // =========================================
  // CARGAR CITAS
  // =========================================

  cargarCitas(): void {

    this.citasService
      .getCitas(
        this.paginaActual,
        this.tamanoPagina
      )
      .subscribe({

        next: (resultado) => {

          this.citas.set(
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
            'Error cargando citas:',
            error
          );

        }

      });

  }


  // =========================================
  // CARGAR PACIENTES
  // =========================================

  cargarPacientes(): void {

    this.pacientesService
      .getPacientes(
        1,
        1000
      )
      .subscribe({

        next: (resultado) => {

          this.pacientes.set(
            resultado.data
          );

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
  // CARGAR CONSULTORIOS
  // =========================================

  cargarConsultorios(): void {

    this.consultoriosService
      .getConsultorios(
        1,
        1000
      )
      .subscribe({

        next: (resultado) => {

          this.consultorios.set(
            resultado.data
          );

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
  // CARGAR ESTADOS
  // =========================================

  cargarEstadosCitas(): void {

    this.estadoCitasService
      .getEstadosCitas(
        1,
        1000
      )
      .subscribe({

        next: (resultado) => {

          this.estadosCitas.set(
            resultado.data
          );

        },

        error: (error) => {

          console.error(
            'Error cargando estados:',
            error
          );

        }

      });

  }


  // =========================================
  // OBTENER NOMBRE PACIENTE
  // =========================================

  obtenerNombrePaciente(
    idPaciente: number
  ): string {

    const paciente =
      this.pacientes()
        .find(
          p =>
            p.idPaciente ===
            idPaciente
        );

    if (!paciente) {

      return `Paciente #${idPaciente}`;

    }

    return (
      `${paciente.nombres} ` +
      `${paciente.apellidos}`
    );

  }


  // =========================================
  // OBTENER CONSULTORIO
  // =========================================

  obtenerNombreConsultorio(
    idConsultorio: number
  ): string {

    const consultorio =
      this.consultorios()
        .find(
          c =>
            c.idConsultorio ===
            idConsultorio
        );

    if (!consultorio) {

      return `Consultorio #${idConsultorio}`;

    }

    return consultorio.nombre;

  }


  // =========================================
  // OBTENER ESTADO
  // =========================================

  obtenerNombreEstado(
    idEstadoCita: number
  ): string {

    const estado =
      this.estadosCitas()
        .find(
          e =>
            e.idEstadoCita ===
            idEstadoCita
        );

    if (!estado) {

      return `Estado #${idEstadoCita}`;

    }

    return estado.nombreEstado;

  }


  // =========================================
  // NUEVA CITA
  // =========================================

  abrirNuevaCita(): void {

    this.modoFormulario = 'nuevo';

    this.citaSeleccionada = null;

    this.nuevaCita =
      this.crearCitaVacia();

    this.mostrarFormulario = true;

  }


  // =========================================
  // VER CITA
  // =========================================

  verCita(
    cita: Cita
  ): void {

    this.citasService
      .getCitaById(
        cita.idCita
      )
      .subscribe({

        next: (resultado) => {

          this.modoFormulario = 'ver';

          this.citaSeleccionada =
            resultado;

          this.nuevaCita = {
            ...resultado,

            horaInicio:
              this.formatearHora(
                resultado.horaInicio
              ),

            horaFin:
              this.formatearHora(
                resultado.horaFin
              )
          };

          this.mostrarFormulario = true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo cita:',
            error
          );

        }

      });

  }


  // =========================================
  // EDITAR CITA
  // =========================================

  editarCita(
    cita: Cita
  ): void {

    this.citasService
      .getCitaById(
        cita.idCita
      )
      .subscribe({

        next: (resultado) => {

          this.modoFormulario = 'editar';

          this.citaSeleccionada =
            resultado;

          this.nuevaCita = {
            ...resultado,

            horaInicio:
              this.formatearHora(
                resultado.horaInicio
              ),

            horaFin:
              this.formatearHora(
                resultado.horaFin
              )
          };

          this.mostrarFormulario = true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo cita:',
            error
          );

        }

      });

  }


  // =========================================
  // ELIMINAR CITA
  // =========================================

  eliminarCita(
    cita: Cita
  ): void {

    const paciente =
      this.obtenerNombrePaciente(
        cita.idPaciente
      );

    const confirmar = confirm(
      `¿Desea eliminar la cita #${cita.idCita} de ${paciente}?`
    );

    if (!confirmar) {
      return;
    }


    this.citasService
      .deleteCita(
        cita.idCita
      )
      .subscribe({

        next: () => {

          if (
            this.citas().length === 1 &&
            this.paginaActual > 1
          ) {

            this.paginaActual--;

          }

          this.cargarCitas();

        },

        error: (error) => {

          console.error(
            'Error eliminando cita:',
            error
          );

        }

      });

  }


  // =========================================
  // GUARDAR CITA
  // =========================================

  guardarCita(): void {

    if (
      this.nuevaCita.idPaciente <= 0
    ) {

      alert(
        'Seleccione un paciente.'
      );

      return;

    }


    if (
      this.nuevaCita.idMedico <= 0
    ) {

      alert(
        'Ingrese un médico válido.'
      );

      return;

    }


    if (
      this.nuevaCita.idConsultorio <= 0
    ) {

      alert(
        'Seleccione un consultorio.'
      );

      return;

    }


    if (
      this.nuevaCita.idEstadoCita <= 0
    ) {

      alert(
        'Seleccione un estado.'
      );

      return;

    }


    if (
      !this.nuevaCita.fechaCita
    ) {

      alert(
        'Seleccione la fecha de la cita.'
      );

      return;

    }


    if (
      !this.nuevaCita.horaInicio ||
      !this.nuevaCita.horaFin
    ) {

      alert(
        'Ingrese la hora de inicio y fin.'
      );

      return;

    }


    if (
      this.nuevaCita.horaInicio >=
      this.nuevaCita.horaFin
    ) {

      alert(
        'La hora de inicio debe ser menor que la hora de fin.'
      );

      return;

    }


    if (
      this.modoFormulario === 'nuevo'
    ) {

      this.agregarCita();

      return;

    }


    if (
      this.modoFormulario === 'editar'
    ) {

      this.actualizarCita();

    }

  }


  // =========================================
  // AGREGAR CITA
  // =========================================

  private agregarCita(): void {

    const cita =
      this.prepararCitaParaApi(
        this.nuevaCita
      );


    this.citasService
      .addCita(
        cita
      )
      .subscribe({

        next: () => {

          this.cerrarFormulario();

          this.paginaActual = 1;

          this.cargarCitas();

        },

        error: (error) => {

          console.error(
            'Error agregando cita:',
            error
          );

        }

      });

  }


  // =========================================
  // ACTUALIZAR CITA
  // =========================================

  private actualizarCita(): void {

    const cita =
      this.prepararCitaParaApi(
        this.nuevaCita
      );


    this.citasService
      .updateCita(
        cita
      )
      .subscribe({

        next: () => {

          this.cerrarFormulario();

          this.cargarCitas();

        },

        error: (error) => {

          console.error(
            'Error actualizando cita:',
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

    this.citaSeleccionada = null;

    this.nuevaCita =
      this.crearCitaVacia();

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

    this.cargarCitas();

  }


  // =========================================
  // CAMBIAR TAMAÑO DE PÁGINA
  // =========================================

  cambiarTamanoPagina(
    tamano: number
  ): void {

    this.tamanoPagina = tamano;

    this.paginaActual = 1;

    this.cargarCitas();

  }


  // =========================================
  // PREPARAR CITA PARA API
  // =========================================

  private prepararCitaParaApi(
    cita: Cita
  ): Cita {

    return {

      ...cita,

      horaInicio:
        this.convertirHoraParaApi(
          cita.horaInicio
        ),

      horaFin:
        this.convertirHoraParaApi(
          cita.horaFin
        )

    };

  }


  // =========================================
  // CONVERTIR HORA PARA API
  // =========================================

  private convertirHoraParaApi(
    hora: string
  ): string {

    if (!hora) {
      return '';
    }

    if (
      hora.length === 5
    ) {

      return `${hora}:00`;

    }

    return hora;

  }


  // =========================================
  // FORMATEAR HORA
  // =========================================

  formatearHora(
    hora: string
  ): string {

    if (!hora) {
      return '';
    }

    return hora.substring(
      0,
      5
    );

  }


  // =========================================
  // CREAR CITA VACÍA
  // =========================================

  private crearCitaVacia():
    Cita {

    return {

      idCita: 0,

      idPaciente: 0,

      idMedico: 0,

      idConsultorio: 0,

      idEstadoCita: 0,

      fechaCita: '',

      horaInicio: '',

      horaFin: '',

      motivoConsulta: '',

      observaciones: '',

      fechaRegistro:
        new Date().toISOString()

    };

  }

}