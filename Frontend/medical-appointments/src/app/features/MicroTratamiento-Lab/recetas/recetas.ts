import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';

import {
  Prescripcion,
  CrearPrescripcion,
  PrescripcionesService
} from '../../../core/services/prescripciones';

import {
  PrescripcionDetalle,
  CrearPrescripcionDetalle,
  PrescripcionDetallesService
} from '../../../core/services/prescripcion-detalles';

import {
  Medicamento,
  MedicamentosService
} from '../../../core/services/medicamentos';

import {
  Paciente,
  PacientesService
} from '../../../core/services/pacientes';

import {
  Cita,
  CitasService
} from '../../../core/services/citas';

import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

interface DetalleRecetaFormulario {
  idPrescripcionDetalle: number;
  idMedicamento: number;
  dosis: number | null;
  unidadMedida: string;
  frecuencia: string;
  duracionDias: number | null;
  indicacionesEspeciales: string;
}

@Component({
  selector: 'app-recetas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],
  templateUrl: './recetas.html',
  styleUrl: './recetas.css'
})
export class Recetas implements OnInit {
  prescripciones: Prescripcion[] = [];
  pacientes: Paciente[] = [];
  citas: Cita[] = [];
  medicamentos: Medicamento[] = [];
  detalles: DetalleRecetaFormulario[] = [];

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  cargando = false;
  guardando = false;

  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';

  formulario: Prescripcion = this.crearFormularioVacio();

  constructor(
    private prescripcionesService: PrescripcionesService,
    private prescripcionDetallesService: PrescripcionDetallesService,
    private medicamentosService: MedicamentosService,
    private pacientesService: PacientesService,
    private citasService: CitasService
  ) {}

  ngOnInit(): void {
    this.cargarPrescripciones();
    this.cargarPacientes();
    this.cargarCitas();
    this.cargarMedicamentos();
  }

  cargarPrescripciones(): void {
    this.cargando = true;

    this.prescripcionesService
      .getPrescripciones(
        this.paginaActual,
        this.tamanoPagina
      )
      .subscribe({
        next: resultado => {
          this.prescripciones = resultado.data;
          this.totalRegistros = resultado.totalRecords;
          this.totalPaginas = resultado.totalPages;
          this.cargando = false;
        },
        error: error => {
          console.error(
            'Error al cargar prescripciones:',
            error
          );
          this.cargando = false;
        }
      });
  }

  cargarPacientes(): void {
    this.pacientesService
      .getPacientes(1, 1000)
      .subscribe({
        next: resultado => {
          this.pacientes = resultado.data;
        },
        error: error => {
          console.error(
            'Error al cargar pacientes:',
            error
          );
        }
      });
  }

  cargarCitas(): void {
    this.citasService
      .getCitas(1, 1000)
      .subscribe({
        next: resultado => {
          this.citas = resultado.data;
        },
        error: error => {
          console.error(
            'Error al cargar citas:',
            error
          );
        }
      });
  }

  cargarMedicamentos(): void {
    this.medicamentosService
      .getMedicamentos(1, 1000)
      .subscribe({
        next: resultado => {
          this.medicamentos = resultado.data.filter(
            medicamento => medicamento.activo
          );
        },
        error: error => {
          console.error(
            'Error al cargar medicamentos:',
            error
          );
        }
      });
  }

  nuevaReceta(): void {
    this.modoFormulario = 'nuevo';
    this.formulario = this.crearFormularioVacio();
    this.detalles = [];
    this.agregarMedicamento();
    this.mostrarFormulario = true;
  }

  verReceta(prescripcion: Prescripcion): void {
    this.modoFormulario = 'ver';
    this.formulario = { ...prescripcion };
    this.mostrarFormulario = true;

    this.cargarDetallesPrescripcion(
      prescripcion.idPrescripcion
    );
  }

  editarReceta(prescripcion: Prescripcion): void {
    this.modoFormulario = 'editar';
    this.formulario = { ...prescripcion };
    this.mostrarFormulario = true;

    this.cargarDetallesPrescripcion(
      prescripcion.idPrescripcion
    );
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.formulario = this.crearFormularioVacio();
    this.detalles = [];
  }

  seleccionarPaciente(): void {
    if (!this.formulario.idPaciente) {
      this.formulario.idCita = null;
      this.formulario.idMedico = 0;
      return;
    }

    if (
      this.formulario.idCita &&
      !this.citasDelPaciente.some(
        cita =>
          cita.idCita === this.formulario.idCita
      )
    ) {
      this.formulario.idCita = null;
      this.formulario.idMedico = 0;
    }
  }

  seleccionarCita(): void {
    if (!this.formulario.idCita) {
      this.formulario.idMedico = 0;
      return;
    }

    const cita = this.citas.find(
      item =>
        item.idCita ===
        Number(this.formulario.idCita)
    );

    if (!cita) {
      this.formulario.idMedico = 0;
      return;
    }

    this.formulario.idMedico = cita.idMedico;
  }

  agregarMedicamento(): void {
    this.detalles.push(this.crearDetalleVacio());
  }

  quitarMedicamento(index: number): void {
    if (this.modoFormulario === 'ver') return;

    if (this.detalles.length === 1) return;

    const detalle = this.detalles[index];

    if (
      this.modoFormulario === 'editar' &&
      detalle.idPrescripcionDetalle > 0
    ) {
      const confirmar = window.confirm(
        '¿Está seguro de eliminar este medicamento de la receta?'
      );

      if (!confirmar) return;

      this.prescripcionDetallesService
        .deletePrescripcionDetalle(
          detalle.idPrescripcionDetalle
        )
        .subscribe({
          next: () => {
            this.detalles.splice(index, 1);
          },
          error: error => {
            console.error(
              'Error al eliminar detalle:',
              error
            );
          }
        });

      return;
    }

    this.detalles.splice(index, 1);
  }

  guardarReceta(): void {
    if (this.guardando) return;

    if (!this.validarFormulario()) return;

    if (this.modoFormulario === 'editar') {
      this.actualizarReceta();
      return;
    }

    this.crearReceta();
  }

  private crearReceta(): void {
    this.guardando = true;

    const prescripcion: CrearPrescripcion = {
      idPaciente: Number(this.formulario.idPaciente),
      idMedico: Number(this.formulario.idMedico),
      idCita: this.formulario.idCita
        ? Number(this.formulario.idCita)
        : null,
      fechaEmision: this.formulario.fechaEmision,
      observaciones:
        this.formulario.observaciones.trim(),
      estado: this.formulario.estado
    };

    this.prescripcionesService
      .addPrescripcion(prescripcion)
      .subscribe({
        next: idPrescripcion => {
          this.guardarDetallesNuevos(
            idPrescripcion
          );
        },
        error: error => {
          console.error(
            'Error al crear receta:',
            error
          );
          this.guardando = false;
        }
      });
  }

  private guardarDetallesNuevos(
    idPrescripcion: number
  ): void {
    const peticiones = this.detalles.map(
      detalle => {
        const nuevoDetalle: CrearPrescripcionDetalle = {
          idPrescripcion,
          idMedicamento:
            Number(detalle.idMedicamento),
          dosis:
            Number(detalle.dosis),
          unidadMedida:
            detalle.unidadMedida.trim(),
          frecuencia:
            detalle.frecuencia.trim(),
          duracionDias:
            Number(detalle.duracionDias),
          indicacionesEspeciales:
            detalle.indicacionesEspeciales.trim()
        };

        return this.prescripcionDetallesService
          .addPrescripcionDetalle(nuevoDetalle);
      }
    );

    forkJoin(peticiones).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.cargarPrescripciones();
      },
      error: error => {
        console.error(
          'Error al guardar detalles:',
          error
        );
        this.guardando = false;
      }
    });
  }

  private actualizarReceta(): void {
    this.guardando = true;

    const prescripcion: Prescripcion = {
      idPrescripcion:
        this.formulario.idPrescripcion,
      idPaciente:
        Number(this.formulario.idPaciente),
      idMedico:
        Number(this.formulario.idMedico),
      idCita:
        this.formulario.idCita
          ? Number(this.formulario.idCita)
          : null,
      fechaEmision:
        this.formulario.fechaEmision,
      observaciones:
        this.formulario.observaciones.trim(),
      estado:
        this.formulario.estado
    };

    this.prescripcionesService
      .updatePrescripcion(prescripcion)
      .subscribe({
        next: () => {
          this.actualizarDetalles(
            prescripcion.idPrescripcion
          );
        },
        error: error => {
          console.error(
            'Error al actualizar receta:',
            error
          );
          this.guardando = false;
        }
      });
  }

  private actualizarDetalles(
    idPrescripcion: number
  ): void {
    const peticiones = this.detalles.map(
      detalle => {
        if (detalle.idPrescripcionDetalle > 0) {
          const existente: PrescripcionDetalle = {
            idPrescripcionDetalle:
              detalle.idPrescripcionDetalle,
            idPrescripcion,
            idMedicamento:
              Number(detalle.idMedicamento),
            dosis:
              Number(detalle.dosis),
            unidadMedida:
              detalle.unidadMedida.trim(),
            frecuencia:
              detalle.frecuencia.trim(),
            duracionDias:
              Number(detalle.duracionDias),
            indicacionesEspeciales:
              detalle.indicacionesEspeciales.trim()
          };

          return this.prescripcionDetallesService
            .updatePrescripcionDetalle(existente);
        }

        const nuevo: CrearPrescripcionDetalle = {
          idPrescripcion,
          idMedicamento:
            Number(detalle.idMedicamento),
          dosis:
            Number(detalle.dosis),
          unidadMedida:
            detalle.unidadMedida.trim(),
          frecuencia:
            detalle.frecuencia.trim(),
          duracionDias:
            Number(detalle.duracionDias),
          indicacionesEspeciales:
            detalle.indicacionesEspeciales.trim()
        };

        return this.prescripcionDetallesService
          .addPrescripcionDetalle(nuevo);
      }
    );

    forkJoin(peticiones).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.cargarPrescripciones();
      },
      error: error => {
        console.error(
          'Error al actualizar detalles:',
          error
        );
        this.guardando = false;
      }
    });
  }

  eliminarReceta(
    prescripcion: Prescripcion
  ): void {
    const confirmar = window.confirm(
      `¿Está seguro de eliminar la receta #${prescripcion.idPrescripcion}?`
    );

    if (!confirmar) return;

    this.prescripcionesService
      .deletePrescripcion(
        prescripcion.idPrescripcion
      )
      .subscribe({
        next: () => {
          if (
            this.prescripciones.length === 1 &&
            this.paginaActual > 1
          ) {
            this.paginaActual--;
          }

          this.cargarPrescripciones();
        },
        error: error => {
          console.error(
            'Error al eliminar receta:',
            error
          );

          alert(
            'No se pudo eliminar la receta.'
          );
        }
      });
  }

  private cargarDetallesPrescripcion(
    idPrescripcion: number
  ): void {
    const filter =
      `IdPrescripcion == ${idPrescripcion}`;

    this.prescripcionDetallesService
      .getPrescripcionDetalles(
        1,
        100,
        filter
      )
      .subscribe({
        next: resultado => {
          this.detalles = resultado.data.map(
            detalle => ({
              idPrescripcionDetalle:
                detalle.idPrescripcionDetalle,
              idMedicamento:
                detalle.idMedicamento,
              dosis:
                detalle.dosis,
              unidadMedida:
                detalle.unidadMedida,
              frecuencia:
                detalle.frecuencia,
              duracionDias:
                detalle.duracionDias,
              indicacionesEspeciales:
                detalle.indicacionesEspeciales
            })
          );

          if (
            this.modoFormulario !== 'ver' &&
            this.detalles.length === 0
          ) {
            this.agregarMedicamento();
          }
        },
        error: error => {
          console.error(
            'Error al cargar detalles:',
            error
          );

          this.detalles = [];
        }
      });
  }

  cambiarPagina(pagina: number): void {
    this.paginaActual = pagina;
    this.cargarPrescripciones();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarPrescripciones();
  }

  private validarFormulario(): boolean {
    if (!this.formulario.idPaciente) {
      alert('Seleccione un paciente.');
      return false;
    }

    if (!this.formulario.idMedico) {
      alert('Seleccione una cita.');
      return false;
    }

    if (!this.formulario.fechaEmision) {
      alert('Ingrese la fecha de emisión.');
      return false;
    }

    if (this.detalles.length === 0) {
      alert('Agregue al menos un medicamento.');
      return false;
    }

    for (let i = 0; i < this.detalles.length; i++) {
      const detalle = this.detalles[i];

      if (!detalle.idMedicamento) {
        alert(
          `Seleccione el medicamento de la línea ${i + 1}.`
        );
        return false;
      }

      if (
        detalle.dosis === null ||
        Number(detalle.dosis) <= 0
      ) {
        alert(
          `Ingrese una dosis válida en la línea ${i + 1}.`
        );
        return false;
      }

      if (!detalle.unidadMedida.trim()) {
        alert(
          `Ingrese la unidad de medida en la línea ${i + 1}.`
        );
        return false;
      }

      if (!detalle.frecuencia.trim()) {
        alert(
          `Ingrese la frecuencia en la línea ${i + 1}.`
        );
        return false;
      }

      if (
        detalle.duracionDias === null ||
        Number(detalle.duracionDias) <= 0
      ) {
        alert(
          `Ingrese la duración en la línea ${i + 1}.`
        );
        return false;
      }
    }

    return true;
  }

  obtenerNombrePaciente(
    idPaciente: number
  ): string {
    const paciente = this.pacientes.find(
      item =>
        item.idPaciente === idPaciente
    );

    if (!paciente) {
      return `Paciente #${idPaciente}`;
    }

    return `${paciente.nombres} ${paciente.apellidos}`;
  }

  obtenerDescripcionCita(cita: Cita): string {
    return `${this.formatearFecha(cita.fechaCita)} - ${cita.horaInicio}`;
  }

  formatearFecha(fecha: string): string {
    if (!fecha) return '';

    const valor = fecha.substring(0, 10);
    const partes = valor.split('-');

    if (partes.length !== 3) {
      return fecha;
    }

    return `${partes[2]}/${partes[1]}/${partes[0]}`;
  }

  get citasDelPaciente(): Cita[] {
    if (!this.formulario.idPaciente) {
      return [];
    }

    return this.citas.filter(
      cita =>
        cita.idPaciente ===
        Number(this.formulario.idPaciente)
    );
  }

  private crearFormularioVacio(): Prescripcion {
    return {
      idPrescripcion: 0,
      idPaciente: 0,
      idMedico: 0,
      idCita: null,
      fechaEmision: this.obtenerFechaActual(),
      observaciones: '',
      estado: 'ACTIVA'
    };
  }

  private crearDetalleVacio():
    DetalleRecetaFormulario {
    return {
      idPrescripcionDetalle: 0,
      idMedicamento: 0,
      dosis: null,
      unidadMedida: '',
      frecuencia: '',
      duracionDias: null,
      indicacionesEspeciales: ''
    };
  }

  private obtenerFechaActual(): string {
    const fecha = new Date();

    const year = fecha.getFullYear();

    const month = String(
      fecha.getMonth() + 1
    ).padStart(2, '0');

    const day = String(
      fecha.getDate()
    ).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}