import {
  Component,
  OnInit,
  OnDestroy,
  signal,
  effect,
  untracked
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  PacientesService,
  Paciente
} from '../../../core/services/pacientes';

import {
  BusquedaService,
  SugerenciaBusqueda
} from '../../../core/services/busqueda';

import {
  BotonesAcciones
} from '../../../shared/components/botones-acciones/botones-acciones';

import {
  Paginacion
} from '../../../shared/components/paginacion/paginacion';


@Component({
  selector: 'app-pacientes',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],

  templateUrl: './pacientes.html',
  styleUrl: './pacientes.css'
})
export class Pacientes
  implements OnInit, OnDestroy {


  // =========================================
  // DATOS
  // =========================================

  pacientes =
    signal<Paciente[]>([]);


  // =========================================
  // PAGINACIÓN
  // =========================================

  paginaActual = 1;

  tamanoPagina = 10;

  totalRegistros = 0;

  totalPaginas = 0;


  // =========================================
  // TEMPORIZADOR DE PREDICCIONES
  // =========================================

  private temporizadorPredicciones:
    ReturnType<typeof setTimeout> | null = null;


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
    private pacientesService: PacientesService,
    private busquedaService: BusquedaService
  ) {


    // =========================================
    // BÚSQUEDA CONFIRMADA
    //
    // SOLO CAMBIA LA TABLA CUANDO:
    //
    // - Presionamos ENTER
    // - Presionamos la lupa
    // - Seleccionamos una sugerencia
    //
    // Escribir solamente NO modifica la tabla.
    // =========================================

    effect(() => {

      this.busquedaService.texto();


      /*
       * cargarPacientes() lee otros signals.
       *
       * Usamos untracked() para evitar
       * convertirlos en dependencias
       * de este effect.
       */

      untracked(() => {

        this.paginaActual = 1;

        this.cargarPacientes();

      });

    });


    // =========================================
    // PREDICCIONES
    //
    // ESTE EFFECT SOLO OBSERVA entrada()
    // =========================================

    effect(() => {

      const texto =
        this.busquedaService.entrada();


      // =====================================
      // CANCELAR TEMPORIZADOR ANTERIOR
      // =====================================

      if (
        this.temporizadorPredicciones
      ) {

        clearTimeout(
          this.temporizadorPredicciones
        );

      }


      // =====================================
      // MÍNIMO 2 CARACTERES
      // =====================================

      if (
        texto.trim().length < 2
      ) {

        this.busquedaService
          .establecerSugerencias([]);

        return;

      }


      // =====================================
      // DEBOUNCE 350 MS
      // =====================================

      this.temporizadorPredicciones =
        setTimeout(() => {

          this.cargarPredicciones(
            texto
          );

        }, 350);

    });

  }


  // =========================================
  // INICIO
  // =========================================

  ngOnInit(): void {

    this.busquedaService.configurar(
      'Buscar pacientes...'
    );

  }


  // =========================================
  // DESTRUIR COMPONENTE
  // =========================================

  ngOnDestroy(): void {

    if (
      this.temporizadorPredicciones
    ) {

      clearTimeout(
        this.temporizadorPredicciones
      );

    }


    this.busquedaService
      .establecerSugerencias([]);

  }


  // =========================================
  // CARGAR PACIENTES
  // =========================================

  cargarPacientes(): void {

    const textoBusqueda =
      this.busquedaService.texto();


    const filtroSeleccionado =
      this.busquedaService
        .filtroSeleccionado();


    /*
     * Si seleccionamos una predicción:
     *
     * IdPaciente == 5
     *
     * Si usamos ENTER o lupa:
     *
     * se construye el filtro utilizando
     * las palabras escritas.
     */

    const filtro =
      filtroSeleccionado !== ''
        ? filtroSeleccionado
        : this.crearFiltroBusqueda(
            textoBusqueda
          );


    this.pacientesService
      .getPacientes(
        this.paginaActual,
        this.tamanoPagina,
        filtro
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
  // CARGAR PREDICCIONES
  // =========================================

  private cargarPredicciones(
    texto: string
  ): void {

    const filtro =
      this.crearFiltroBusqueda(
        texto
      );


    if (
      filtro === ''
    ) {

      this.busquedaService
        .establecerSugerencias([]);

      return;

    }


    /*
     * Solo solicitamos 5 registros
     * para las predicciones.
     */

    this.pacientesService
      .getPacientes(
        1,
        5,
        filtro
      )
      .subscribe({

        next: (resultado) => {


          // =====================================
          // EVITAR RESPUESTAS VIEJAS
          // =====================================

          /*
           * Ejemplo:
           *
           * Usuario escribe:
           *
           * Jo
           *
           * luego rápidamente:
           *
           * Jose
           *
           * Si la respuesta de "Jo" llega
           * después, no queremos mostrarla.
           */

          if (
            this.busquedaService
              .entrada()
              .trim() !== texto.trim()
          ) {

            return;

          }


          // =====================================
          // CONSTRUIR SUGERENCIAS
          // =====================================

          const sugerencias:
            SugerenciaBusqueda[] =
            resultado.data.map(
              (paciente) => {

                return {

                  // Nombre que verá el usuario

                  texto:
                    `${paciente.nombres} ${paciente.apellidos}`.trim(),


                  // DUI mostrado a la derecha

                  descripcion:
                    paciente.dui,


                  // Búsqueda exacta al hacer clic

                  filtro:
                    `IdPaciente == ${paciente.idPaciente}`

                };

              }
            );


          this.busquedaService
            .establecerSugerencias(
              sugerencias
            );

        },


        error: (error) => {

          console.error(
            'Error cargando predicciones:',
            error
          );


          this.busquedaService
            .establecerSugerencias([]);

        }

      });

  }


  // =========================================
  // CREAR FILTRO DE BÚSQUEDA
  // =========================================

  private crearFiltroBusqueda(
    texto: string
  ): string {

    const valor =
      texto.trim();


    // =========================================
    // SIN TEXTO = SIN FILTRO
    // =========================================

    if (
      valor === ''
    ) {

      return '';

    }


    // =========================================
    // SEPARAR EN PALABRAS
    // =========================================

    /*
     * Ejemplo:
     *
     * "Jose Flores"
     *
     * se convierte en:
     *
     * [
     *   "Jose",
     *   "Flores"
     * ]
     */

    const palabras =
      valor
        .split(/\s+/)
        .filter(
          palabra =>
            palabra.trim() !== ''
        );


    // =========================================
    // CREAR FILTRO POR PALABRA
    // =========================================

    const filtros =
      palabras.map(
        palabra => {


          // =====================================
          // ESCAPAR CARACTERES ESPECIALES
          // =====================================

          const palabraSegura =
            palabra
              .replace(/\\/g, '\\\\')
              .replace(/"/g, '\\"');


          // =====================================
          // BUSCAR PALABRA EN CUALQUIER CAMPO
          // =====================================

          /*
           * Cada palabra puede encontrarse
           * en cualquiera de estos campos.
           *
           * Ejemplo:
           *
           * Jose → Nombres
           * Flores → Apellidos
           */

          return `(
            Nombres.Contains("${palabraSegura}") ||
            Apellidos.Contains("${palabraSegura}") ||
            DUI.Contains("${palabraSegura}") ||
            Telefono.Contains("${palabraSegura}") ||
            Correo.Contains("${palabraSegura}")
          )`;

        }
      );


    // =========================================
    // UNIR TODAS LAS PALABRAS CON &&
    // =========================================

    /*
     * Si buscamos:
     *
     * Jose Flores
     *
     * queda aproximadamente:
     *
     * (
     *   Nombres.Contains("Jose")
     *   ||
     *   Apellidos.Contains("Jose")
     *   ||
     *   ...
     * )
     *
     * &&
     *
     * (
     *   Nombres.Contains("Flores")
     *   ||
     *   Apellidos.Contains("Flores")
     *   ||
     *   ...
     * )
     *
     * Por eso las palabras pueden estar
     * en columnas diferentes.
     */

    return filtros.join(' && ');

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


    const confirmar =
      confirm(
        `¿Desea eliminar al paciente ${nombreCompleto}?`
      );


    if (
      !confirmar
    ) {

      return;

    }


    this.pacientesService
      .deletePaciente(
        paciente.idPaciente
      )
      .subscribe({

        next: () => {


          /*
           * Si eliminamos el último registro
           * de una página diferente a la 1,
           * regresamos una página.
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


    this.paginaActual =
      pagina;


    this.cargarPacientes();

  }


  // =========================================
  // CAMBIAR TAMAÑO DE PÁGINA
  // =========================================

  cambiarTamanoPagina(
    tamano: number
  ): void {

    this.tamanoPagina =
      tamano;


    this.paginaActual =
      1;


    this.cargarPacientes();

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