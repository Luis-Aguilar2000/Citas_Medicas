import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  MedicamentosService,
  Medicamento
} from '../../../core/services/MicroTratamientosLab/medicamentos';

import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-medicamentos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],
  templateUrl: './medicamentos.html',
  styleUrl: './medicamentos.css'
})
export class Medicamentos implements OnInit {

  medicamentos = signal<Medicamento[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';

  medicamentoSeleccionado: Medicamento | null = null;
  nuevoMedicamento: Medicamento = this.crearMedicamentoVacio();

  guardando = false;

  constructor(
    private medicamentosService: MedicamentosService
  ) {}

  ngOnInit(): void {
    this.cargarMedicamentos();
  }

  cargarMedicamentos(): void {
    this.medicamentosService
      .getMedicamentos(
        this.paginaActual,
        this.tamanoPagina
      )
      .subscribe({
        next: resultado => {
          this.medicamentos.set(resultado.data);
          this.totalRegistros = resultado.totalRecords;
          this.totalPaginas = resultado.totalPages;
          this.paginaActual = resultado.currentPage;
        },
        error: error => {
          console.error(
            'Error cargando medicamentos:',
            error
          );
        }
      });
  }

  abrirNuevoMedicamento(): void {
    this.modoFormulario = 'nuevo';
    this.medicamentoSeleccionado = null;
    this.nuevoMedicamento = this.crearMedicamentoVacio();
    this.mostrarFormulario = true;
  }

  verMedicamento(medicamento: Medicamento): void {
    this.modoFormulario = 'ver';
    this.medicamentoSeleccionado = medicamento;
    this.nuevoMedicamento = { ...medicamento };
    this.mostrarFormulario = true;
  }

  editarMedicamento(medicamento: Medicamento): void {
    this.modoFormulario = 'editar';
    this.medicamentoSeleccionado = medicamento;
    this.nuevoMedicamento = { ...medicamento };
    this.mostrarFormulario = true;
  }

  eliminarMedicamento(medicamento: Medicamento): void {
    if (
      !confirm(
        `Â¿Desea eliminar el medicamento ${medicamento.nombreComercial}?`
      )
    ) {
      return;
    }

    this.medicamentosService
      .deleteMedicamento(medicamento.idMedicamento)
      .subscribe({
        next: () => {
          if (
            this.medicamentos().length === 1 &&
            this.paginaActual > 1
          ) {
            this.paginaActual--;
          }

          this.cargarMedicamentos();
        },
        error: error => {
          console.error(
            'Error eliminando medicamento:',
            error
          );
        }
      });
  }

  guardarMedicamento(): void {
    if (this.guardando) return;

    if (!this.validarFormulario()) return;

    if (this.modoFormulario === 'nuevo') {
      this.agregarMedicamento();
      return;
    }

    if (this.modoFormulario === 'editar') {
      this.actualizarMedicamento();
    }
  }

  private agregarMedicamento(): void {
    this.guardando = true;

    this.medicamentosService
      .addMedicamento(this.nuevoMedicamento)
      .subscribe({
        next: () => {
          this.guardando = false;
          this.cerrarFormulario();
          this.paginaActual = 1;
          this.cargarMedicamentos();
        },
        error: error => {
          console.error(
            'Error agregando medicamento:',
            error
          );

          this.guardando = false;
        }
      });
  }

  private actualizarMedicamento(): void {
    this.guardando = true;

    this.medicamentosService
      .updateMedicamento(this.nuevoMedicamento)
      .subscribe({
        next: () => {
          this.guardando = false;
          this.cerrarFormulario();
          this.cargarMedicamentos();
        },
        error: error => {
          console.error(
            'Error actualizando medicamento:',
            error
          );

          this.guardando = false;
        }
      });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.medicamentoSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoMedicamento = this.crearMedicamentoVacio();
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
    this.cargarMedicamentos();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarMedicamentos();
  }

  private validarFormulario(): boolean {
    if (!this.nuevoMedicamento.nombreComercial.trim()) {
      alert('Ingrese el nombre comercial.');
      return false;
    }

    if (!this.nuevoMedicamento.nombreGenerico.trim()) {
      alert('Ingrese el nombre genÃ©rico.');
      return false;
    }

    if (!this.nuevoMedicamento.presentacion.trim()) {
      alert('Ingrese la presentaciÃ³n.');
      return false;
    }

    if (!this.nuevoMedicamento.concentracion.trim()) {
      alert('Ingrese la concentraciÃ³n.');
      return false;
    }

    if (!this.nuevoMedicamento.viaAdministracion.trim()) {
      alert('Seleccione la vÃ­a de administraciÃ³n.');
      return false;
    }

    if (Number(this.nuevoMedicamento.precio) < 0) {
      alert('Ingrese un precio vÃ¡lido.');
      return false;
    }

    return true;
  }

  private crearMedicamentoVacio(): Medicamento {
    return {
      idMedicamento: 0,
      nombreComercial: '',
      nombreGenerico: '',
      presentacion: '',
      concentracion: '',
      viaAdministracion: '',
      precio: 0,
      activo: true
    };
  }
}
