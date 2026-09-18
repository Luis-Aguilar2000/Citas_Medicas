import {
  Component,
  OnInit,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  HorarioMedicoService,
  HorarioMedico as HorarioMedicoModel
} from '../../../core/services/horario-medico';

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
  selector: 'app-horario-medico',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],

  templateUrl: './horario-medico.html',
  styleUrl: './horario-medico.css'
})
export class HorarioMedico implements OnInit {


  // =========================================
  // DATOS
  // =========================================

  horarios = signal<HorarioMedicoModel[]>([]);

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

  horarioSeleccionado:
    HorarioMedicoModel | null = null;

  nuevoHorario:
    HorarioMedicoModel = this.crearHorarioVacio();


  // =========================================
  // DÍAS DE LA SEMANA
  // =========================================

  diasSemana: string[] = [
    'Lunes',
    'Martes',
    'Miércoles',
    'Jueves',
    'Viernes',
    'Sábado',
    'Domingo'
  ];


  // =========================================
  // CONSTRUCTOR
  // =========================================

  constructor(
    private horarioService: HorarioMedicoService,
    private consultoriosService: ConsultoriosService
  ) {}


  // =========================================
  // INIT
  // =========================================

  ngOnInit(): void {

    this.cargarHorarios();

    this.cargarConsultorios();

  }


  // =========================================
  // CARGAR HORARIOS PAGINADOS
  // =========================================

  cargarHorarios(): void {

    this.horarioService
      .getHorarios(
        this.paginaActual,
        this.tamanoPagina
      )
      .subscribe({

        next: (resultado) => {

          console.log(
            'HORARIOS RECIBIDOS:',
            resultado
          );

          this.horarios.set(
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
            'Error cargando horarios:',
            error
          );

        }

      });

  }


  // =========================================
  // CARGAR CONSULTORIOS
  // =========================================

  cargarConsultorios(): void {

    /*
     * Temporalmente solicitamos hasta 1000
     * consultorios para utilizarlos como catálogo.
     *
     * Posteriormente podemos reemplazar esto
     * por un endpoint específico de catálogo.
     */

    this.consultoriosService
      .getConsultorios(
        1,
        1000
      )
      .subscribe({

        next: (resultado) => {

          console.log(
            'CONSULTORIOS PARA HORARIOS:',
            resultado
          );

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
  // NOMBRE DEL CONSULTORIO
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

    this.cargarHorarios();

  }


  // =========================================
  // CAMBIAR TAMAÑO DE PÁGINA
  // =========================================

  cambiarTamanoPagina(
    tamano: number
  ): void {

    this.tamanoPagina = tamano;

    this.paginaActual = 1;

    this.cargarHorarios();

  }


  // =========================================
  // NUEVO HORARIO
  // =========================================

  abrirNuevoHorario(): void {

    this.modoFormulario = 'nuevo';

    this.horarioSeleccionado = null;

    this.nuevoHorario =
      this.crearHorarioVacio();

    this.mostrarFormulario = true;

  }


  // =========================================
  // VER HORARIO
  // =========================================

  verHorario(
    horario: HorarioMedicoModel
  ): void {

    this.horarioService
      .getHorarioById(
        horario.idHorario
      )
      .subscribe({

        next: (data) => {

          this.modoFormulario = 'ver';

          this.horarioSeleccionado =
            data;

          this.nuevoHorario = {

            ...data,

            horaInicio:
              this.formatearHora(
                data.horaInicio
              ),

            horaFin:
              this.formatearHora(
                data.horaFin
              )

          };

          this.mostrarFormulario = true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo horario:',
            error
          );

        }

      });

  }


  // =========================================
  // EDITAR HORARIO
  // =========================================

  editarHorario(
    horario: HorarioMedicoModel
  ): void {

    this.horarioService
      .getHorarioById(
        horario.idHorario
      )
      .subscribe({

        next: (data) => {

          this.modoFormulario = 'editar';

          this.horarioSeleccionado =
            data;

          this.nuevoHorario = {

            ...data,

            horaInicio:
              this.formatearHora(
                data.horaInicio
              ),

            horaFin:
              this.formatearHora(
                data.horaFin
              )

          };

          this.mostrarFormulario = true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo horario:',
            error
          );

        }

      });

  }


  // =========================================
  // ELIMINAR HORARIO
  // =========================================

  eliminarHorario(
    horario: HorarioMedicoModel
  ): void {

    const consultorio =
      this.obtenerNombreConsultorio(
        horario.idConsultorio
      );

    const confirmar = confirm(
      `¿Desea eliminar el horario del ${horario.diaSemana} en ${consultorio}?`
    );

    if (!confirmar) {

      return;

    }

    this.horarioService
      .deleteHorario(
        horario.idHorario
      )
      .subscribe({

        next: () => {

          console.log(
            'Horario eliminado correctamente'
          );

          if (
            this.horarios().length === 1 &&
            this.paginaActual > 1
          ) {

            this.paginaActual--;

          }

          this.cargarHorarios();

        },

        error: (error) => {

          console.error(
            'Error eliminando horario:',
            error
          );

        }

      });

  }


  // =========================================
  // GUARDAR HORARIO
  // =========================================

  guardarHorario(): void {

    if (
      this.nuevoHorario.idMedico <= 0 ||
      this.nuevoHorario.idConsultorio <= 0 ||
      !this.nuevoHorario.diaSemana ||
      !this.nuevoHorario.horaInicio ||
      !this.nuevoHorario.horaFin
    ) {

      alert(
        'Complete todos los campos del horario.'
      );

      return;

    }


    if (
      this.nuevoHorario.horaInicio >=
      this.nuevoHorario.horaFin
    ) {

      alert(
        'La hora de inicio debe ser menor que la hora de fin.'
      );

      return;

    }


    if (
      this.modoFormulario === 'nuevo'
    ) {

      this.agregarHorario();

      return;

    }


    if (
      this.modoFormulario === 'editar'
    ) {

      this.actualizarHorario();

    }

  }


  // =========================================
  // AGREGAR HORARIO
  // =========================================

  agregarHorario(): void {

    const horario =
      this.prepararHorarioParaApi(
        this.nuevoHorario
      );

    this.horarioService
      .addHorario(
        horario
      )
      .subscribe({

        next: () => {

          console.log(
            'Horario agregado correctamente'
          );

          this.cerrarFormulario();

          this.paginaActual = 1;

          this.cargarHorarios();

        },

        error: (error) => {

          console.error(
            'Error agregando horario:',
            error
          );

        }

      });

  }


  // =========================================
  // ACTUALIZAR HORARIO
  // =========================================

  actualizarHorario(): void {

    const horario =
      this.prepararHorarioParaApi(
        this.nuevoHorario
      );

    this.horarioService
      .updateHorario(
        horario
      )
      .subscribe({

        next: () => {

          console.log(
            'Horario actualizado correctamente'
          );

          this.cerrarFormulario();

          this.cargarHorarios();

        },

        error: (error) => {

          console.error(
            'Error actualizando horario:',
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

    this.horarioSeleccionado = null;

    this.nuevoHorario =
      this.crearHorarioVacio();

  }


  // =========================================
  // PREPARAR HORARIO PARA API
  // =========================================

  private prepararHorarioParaApi(
    horario: HorarioMedicoModel
  ): HorarioMedicoModel {

    return {

      ...horario,

      horaInicio:
        this.convertirHoraParaApi(
          horario.horaInicio
        ),

      horaFin:
        this.convertirHoraParaApi(
          horario.horaFin
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

      return '00:00:00';

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
  // HORARIO VACÍO
  // =========================================

  private crearHorarioVacio():
    HorarioMedicoModel {

    return {

      idHorario: 0,

      idMedico: 0,

      idConsultorio: 0,

      diaSemana: '',

      horaInicio: '',

      horaFin: '',

      estado: true

    };

  }

}