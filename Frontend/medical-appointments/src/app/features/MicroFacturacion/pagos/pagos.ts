import {
  Component,
  OnInit,
  OnDestroy,
  signal,
  effect,
  untracked
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormsModule
} from '@angular/forms';

import {
  PagosService,
  Pago
} from '../../../core/services/pagos';

import {
  FormasPagoService,
  FormaPago
} from '../../../core/services/formas-pago';

import {
  FacturasService,
  Factura
} from '../../../core/services/facturas';

import {
  PacientesService,
  Paciente
} from '../../../core/services/pacientes';

import {
  BusquedaService
} from '../../../core/services/busqueda';

import {
  BotonesAcciones
} from '../../../shared/components/botones-acciones/botones-acciones';

import {
  Paginacion
} from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-pagos',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],

  templateUrl: './pagos.html',
  styleUrl: './pagos.css'
})
export class Pagos
  implements OnInit, OnDestroy {

  pagos =
    signal<Pago[]>([]);

  formasPago =
    signal<FormaPago[]>([]);

  facturas =
    signal<Factura[]>([]);

  pacientes =
    signal<Paciente[]>([]);

  paginaActual = 1;

  tamanoPagina = 10;

  totalRegistros = 0;

  totalPaginas = 0;

  mostrarFormulario = false;

  modoFormulario:
    'ver' |
    'editar' =
    'ver';

  pagoSeleccionado:
    Pago | null = null;

  pagoFormulario:
    Pago =
    this.crearPagoVacio();

  guardando = false;

  constructor(
    private pagosService:
      PagosService,

    private formasPagoService:
      FormasPagoService,

    private facturasService:
      FacturasService,

    private pacientesService:
      PacientesService,

    private busquedaService:
      BusquedaService
  ) {

    effect(() => {

      this.busquedaService.texto();

      untracked(() => {

        this.paginaActual = 1;

        this.cargarPagos();
      });
    });
  }

  ngOnInit(): void {

    this.busquedaService.configurar(
      'Buscar pago o factura...'
    );

    this.cargarFormasPago();

    this.cargarFacturas();

    this.cargarPacientes();
  }

  ngOnDestroy(): void {

    this.busquedaService
      .establecerSugerencias([]);
  }

  cargarPagos(): void {

    const texto =
      this.busquedaService
        .texto()
        .trim();

    const filtro =
      this.crearFiltroBusqueda(
        texto
      );

    this.pagosService
      .getPagos(
        this.paginaActual,
        this.tamanoPagina,
        filtro
      )
      .subscribe({

        next: resultado => {

          this.pagos.set(
            resultado.data ?? []
          );

          this.totalRegistros =
            resultado.totalRecords;

          this.totalPaginas =
            resultado.totalPages;

          this.paginaActual =
            resultado.currentPage;
        },

        error: error => {

          console.error(
            'Error cargando pagos:',
            error
          );
        }
      });
  }

  cargarFormasPago(): void {

    this.formasPagoService
      .getFormasPago(
        1,
        100
      )
      .subscribe({

        next: resultado => {

          this.formasPago.set(
            resultado.data ?? []
          );
        },

        error: error => {

          console.error(
            'Error cargando formas de pago:',
            error
          );
        }
      });
  }

  cargarFacturas(): void {

    this.facturasService
      .getFacturas(
        1,
        100
      )
      .subscribe({

        next: resultado => {

          this.facturas.set(
            resultado.data ?? []
          );
        },

        error: error => {

          console.error(
            'Error cargando facturas:',
            error
          );
        }
      });
  }

  cargarPacientes(): void {

    this.pacientesService
      .getPacientes(
        1,
        100
      )
      .subscribe({

        next: resultado => {

          this.pacientes.set(
            resultado.data ?? []
          );
        },

        error: error => {

          console.error(
            'Error cargando pacientes:',
            error
          );
        }
      });
  }

  obtenerFormaPago(
    idFormaPago: number
  ): string {

    const forma =
      this.formasPago()
        .find(
          item =>
            item.idFormaPago ===
            idFormaPago
        );

    if (!forma) {

      return `Forma #${idFormaPago}`;
    }

    return forma.nombre;
  }

  obtenerPacientePago(
    idFactura: number
  ): string {

    const factura =
      this.facturas()
        .find(
          item =>
            item.idFactura ===
            idFactura
        );

    if (!factura) {
      return '-';
    }

    const paciente =
      this.pacientes()
        .find(
          item =>
            item.idPaciente ===
            factura.idPaciente
        );

    if (!paciente) {

      return `Paciente #${factura.idPaciente}`;
    }

    return `${paciente.nombres} ${paciente.apellidos}`
      .trim();
  }

  verPago(
    pago: Pago
  ): void {

    this.modoFormulario =
      'ver';

    this.pagoSeleccionado =
      pago;

    this.pagoFormulario = {
      ...pago
    };

    this.mostrarFormulario =
      true;
  }

  editarPago(
    pago: Pago
  ): void {

    this.modoFormulario =
      'editar';

    this.pagoSeleccionado =
      pago;

    this.pagoFormulario = {
      ...pago,
      fechaPago:
        this.formatearFechaInput(
          pago.fechaPago
        )
    };

    this.mostrarFormulario =
      true;
  }

  guardarPago(): void {

    if (
      this.modoFormulario !==
      'editar'
    ) {
      return;
    }

    if (
      this.pagoFormulario
        .idFormaPago <= 0
    ) {

      alert(
        'Seleccione una forma de pago.'
      );

      return;
    }

    if (
      Number(
        this.pagoFormulario.monto
      ) <= 0
    ) {

      alert(
        'Ingrese un monto válido.'
      );

      return;
    }

    this.guardando =
      true;

    this.pagosService
      .updatePago(
        this.pagoFormulario
      )
      .subscribe({

        next: () => {

          this.guardando =
            false;

          this.cerrarFormulario();

          this.cargarPagos();
        },

        error: error => {

          this.guardando =
            false;

          console.error(
            'Error actualizando pago:',
            error
          );

          alert(
            error.error?.mensaje ??
            'No se pudo actualizar el pago.'
          );
        }
      });
  }

  eliminarPago(
    pago: Pago
  ): void {

    const confirmar =
      confirm(
        `¿Desea eliminar el pago #${pago.idPago}?`
      );

    if (
      !confirmar
    ) {
      return;
    }

    this.pagosService
      .deletePago(
        pago.idPago
      )
      .subscribe({

        next: () => {

          if (
            this.pagos().length === 1 &&
            this.paginaActual > 1
          ) {

            this.paginaActual--;
          }

          this.cargarPagos();
        },

        error: error => {

          console.error(
            'Error eliminando pago:',
            error
          );

          alert(
            error.error?.mensaje ??
            'No se pudo eliminar el pago.'
          );
        }
      });
  }

  cerrarFormulario(): void {

    if (
      this.guardando
    ) {
      return;
    }

    this.mostrarFormulario =
      false;

    this.pagoSeleccionado =
      null;

    this.pagoFormulario =
      this.crearPagoVacio();

    this.modoFormulario =
      'ver';
  }

  cambiarPagina(
    pagina: number
  ): void {

    if (
      pagina < 1 ||
      pagina >
        this.totalPaginas ||
      pagina ===
        this.paginaActual
    ) {
      return;
    }

    this.paginaActual =
      pagina;

    this.cargarPagos();
  }

  cambiarTamanoPagina(
    tamano: number
  ): void {

    this.tamanoPagina =
      Number(tamano);

    this.paginaActual = 1;

    this.cargarPagos();
  }

  private crearFiltroBusqueda(
    texto: string
  ): string {

    if (!texto) {
      return '';
    }

    const numero =
      Number(texto);

    if (
      !Number.isNaN(numero)
    ) {

      return (
        `IdPago == ${numero} || ` +
        `IdFactura == ${numero}`
      );
    }

    return '';
  }

  private crearPagoVacio():
    Pago {

    return {

      idPago: 0,

      idFactura: 0,

      idFormaPago: 0,

      monto: 0,

      fechaPago: ''
    };
  }

  private formatearFechaInput(
    fecha: string
  ): string {

    if (!fecha) {
      return '';
    }

    return fecha.substring(
      0,
      16
    );
  }
}