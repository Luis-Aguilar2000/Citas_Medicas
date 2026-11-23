
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  ExpedientesClinicosService,
  ExpedienteClinico
} from '../../../core/services/expediente-clinico';

import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-expediente-clinico',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],
  templateUrl: './expediente-clinico.html',
  styleUrl: './expediente-clinico.css'
})
export class ExpedientesClinicos implements OnInit {

  expedientes = signal<ExpedienteClinico[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  mostrarFormulario = false;

  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';

  expedienteSeleccionado: ExpedienteClinico | null = null;

  nuevoExpediente: ExpedienteClinico = this.crearExpedienteVacio();

  guardando = false;

  constructor(
    private expedientesService: ExpedientesClinicosService
  ) {}

  ngOnInit(): void {
    this.cargarExpedientes();
  }

  cargarExpedientes(): void {

    this.expedientesService
      .getExpedientes(
        this.paginaActual,
        this.tamanoPagina
      )
      .subscribe({
        next: resultado => {

          this.expedientes.set(resultado.data);

          this.totalRegistros = resultado.totalRecords;
          this.totalPaginas = resultado.totalPages;
          this.paginaActual = resultado.currentPage;

        },
        error: error => {
          console.error(
            'Error cargando expedientes:',
            error
          );
        }
      });
  }

  abrirNuevoExpediente(): void {

    this.modoFormulario = 'nuevo';

    this.expedienteSeleccionado = null;

    this.nuevoExpediente = this.crearExpedienteVacio();

    this.mostrarFormulario = true;
  }

  verExpediente(expediente: ExpedienteClinico): void {

    this.modoFormulario = 'ver';

    this.expedienteSeleccionado = expediente;

    this.nuevoExpediente = { ...expediente };

    this.mostrarFormulario = true;
  }

  editarExpediente(expediente: ExpedienteClinico): void {

    this.modoFormulario = 'editar';

    this.expedienteSeleccionado = expediente;

    this.nuevoExpediente = { ...expediente };

    this.mostrarFormulario = true;
  }

  eliminarExpediente(expediente: ExpedienteClinico): void {

    if (
      !confirm(
        `¿Desea eliminar el expediente ${expediente.numeroExpediente}?`
      )
    ) {
      return;
    }

    this.expedientesService
      .deleteExpediente(expediente.expedienteId)
      .subscribe({
        next: () => {

          if (
            this.expedientes().length === 1 &&
            this.paginaActual > 1
          ) {
            this.paginaActual--;
          }

          this.cargarExpedientes();
        },
        error: error => {
          console.error(
            'Error eliminando expediente:',
            error
          );
        }
      });
  }

  guardarExpediente(): void {

    if (this.guardando) return;

    if (!this.validarFormulario()) return;

    if (this.modoFormulario === 'nuevo') {
      this.agregarExpediente();
      return;
    }

    if (this.modoFormulario === 'editar') {
      this.actualizarExpediente();
    }
  }

  private agregarExpediente(): void {

    this.guardando = true;

    this.expedientesService
      .addExpediente(this.nuevoExpediente)
      .subscribe({
        next: () => {

          this.guardando = false;

          this.cerrarFormulario();

          this.paginaActual = 1;

          this.cargarExpedientes();
        },
        error: error => {

          console.error(
            'Error agregando expediente:',
            error
          );

          this.guardando = false;
        }
      });
  }

  private actualizarExpediente(): void {

    this.guardando = true;

    this.expedientesService
      .updateExpediente(this.nuevoExpediente)
      .subscribe({
        next: () => {

          this.guardando = false;

          this.cerrarFormulario();

          this.cargarExpedientes();
        },
        error: error => {

          console.error(
            'Error actualizando expediente:',
            error
          );

          this.guardando = false;
        }
      });
  }

  cerrarFormulario(): void {

    this.mostrarFormulario = false;

    this.modoFormulario = 'nuevo';

    this.expedienteSeleccionado = null;

    this.limpiarFormulario();
  }

  limpiarFormulario(): void {

    this.nuevoExpediente = this.crearExpedienteVacio();
  }

  cambiarPagina(pagina: number): void {

    if (
      pagina < 1 ||
      pagina > this.totalPaginas ||
      pagina === this.paginaActual
    ) {
      return;
    }

    this.paginaActual = pagina;

    this.cargarExpedientes();
  }

  cambiarTamanoPagina(tamano: number): void {

    this.tamanoPagina = tamano;

    this.paginaActual = 1;

    this.cargarExpedientes();
  }

  private validarFormulario(): boolean {

    if (this.nuevoExpediente.pacienteId <= 0) {
      alert('Ingrese un identificador de paciente válido.');
      return false;
    }

    if (!this.nuevoExpediente.numeroExpediente.trim()) {
      alert('Ingrese el número de expediente.');
      return false;
    }

    if (!this.nuevoExpediente.fechaApertura) {
      alert('Seleccione la fecha de apertura.');
      return false;
    }

    if (!this.nuevoExpediente.estado.trim()) {
      alert('Seleccione el estado del expediente.');
      return false;
    }

    return true;
  }

  private crearExpedienteVacio(): ExpedienteClinico {

    return {
      expedienteId: 0,
      pacienteId: 0,
      numeroExpediente: '',
      fechaApertura: '',
      estado: 'Activo',
      creadoEn: '',
      actualizadoEn: ''
    };
  }
}
