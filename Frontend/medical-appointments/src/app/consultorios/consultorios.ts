import {
  Component,
  OnInit,
  signal
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormsModule
} from '@angular/forms';

import {
  ConsultoriosService,
  Consultorio
} from '../services/consultorios';

import {
  BotonesAcciones
} from '../botones-acciones/botones-acciones';


@Component({
  selector: 'app-consultorios',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones
  ],

  templateUrl: './consultorios.html',

  styleUrl: './consultorios.css'
})

export class Consultorios
  implements OnInit {


  /* =========================================
     LISTADO
  ========================================= */

  consultorios =
    signal<Consultorio[]>([]);


  /* =========================================
     FORMULARIO
  ========================================= */

  mostrarFormulario = false;

  modoFormulario:
    'nuevo' | 'ver' | 'editar' =
    'nuevo';

  consultorioSeleccionado:
    Consultorio | null = null;

  nuevoConsultorio:
    Consultorio =
    this.crearConsultorioVacio();


  /* =========================================
     CONSTRUCTOR
  ========================================= */

  constructor(
    private consultoriosService:
      ConsultoriosService
  ) {}


  /* =========================================
     INICIALIZACIÓN
  ========================================= */

  ngOnInit(): void {

    this.cargarConsultorios();

  }


  /* =========================================
     CARGAR CONSULTORIOS
  ========================================= */

  cargarConsultorios(): void {

    this.consultoriosService
      .getConsultorios()
      .subscribe({

        next: (data) => {

          console.log(
            'CONSULTORIOS RECIBIDOS:',
            data
          );

          this.consultorios.set(
            data
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


  /* =========================================
     NUEVO CONSULTORIO
  ========================================= */

  abrirNuevoConsultorio(): void {

    this.modoFormulario =
      'nuevo';

    this.consultorioSeleccionado =
      null;

    this.nuevoConsultorio =
      this.crearConsultorioVacio();

    this.mostrarFormulario =
      true;

  }


  /* =========================================
     VER CONSULTORIO
  ========================================= */

  verConsultorio(
    consultorio: Consultorio
  ): void {

    this.consultoriosService
      .getConsultorioById(
        consultorio.idConsultorio
      )
      .subscribe({

        next: (data) => {

          this.modoFormulario =
            'ver';

          this.consultorioSeleccionado =
            data;

          this.nuevoConsultorio = {
            ...data
          };

          this.mostrarFormulario =
            true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo consultorio:',
            error
          );

        }

      });

  }


  /* =========================================
     EDITAR CONSULTORIO
  ========================================= */

  editarConsultorio(
    consultorio: Consultorio
  ): void {

    this.consultoriosService
      .getConsultorioById(
        consultorio.idConsultorio
      )
      .subscribe({

        next: (data) => {

          this.modoFormulario =
            'editar';

          this.consultorioSeleccionado =
            data;

          this.nuevoConsultorio = {
            ...data
          };

          this.mostrarFormulario =
            true;

        },

        error: (error) => {

          console.error(
            'Error obteniendo consultorio:',
            error
          );

        }

      });

  }


  /* =========================================
     ELIMINAR CONSULTORIO
  ========================================= */

  eliminarConsultorio(
    consultorio: Consultorio
  ): void {

    const confirmar =
      confirm(
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


  /* =========================================
     GUARDAR
  ========================================= */

  guardarConsultorio(): void {

    if (
      this.modoFormulario ===
      'nuevo'
    ) {

      this.agregarConsultorio();

      return;

    }


    if (
      this.modoFormulario ===
      'editar'
    ) {

      this.actualizarConsultorio();

    }

  }


  /* =========================================
     AGREGAR CONSULTORIO
  ========================================= */

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


  /* =========================================
     ACTUALIZAR CONSULTORIO
  ========================================= */

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


  /* =========================================
     CERRAR FORMULARIO
  ========================================= */

  cerrarFormulario(): void {

    this.mostrarFormulario =
      false;

    this.modoFormulario =
      'nuevo';

    this.consultorioSeleccionado =
      null;

    this.limpiarFormulario();

  }


  /* =========================================
     LIMPIAR FORMULARIO
  ========================================= */

  limpiarFormulario(): void {

    this.nuevoConsultorio =
      this.crearConsultorioVacio();

  }


  /* =========================================
     CREAR CONSULTORIO VACÍO
  ========================================= */

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