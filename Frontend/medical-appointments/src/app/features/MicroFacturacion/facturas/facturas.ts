import {
  Component,
  OnInit,
  OnDestroy,
  signal,
  effect,
  untracked,
  computed
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  forkJoin,
  of,
  Observable
} from 'rxjs';

import {
  switchMap,
  map
} from 'rxjs/operators';

import {
  FacturasService,
  Factura
} from '../../../core/services/facturas';

import {
  FacturaDetallesService,
  FacturaDetalle
} from '../../../core/services/factura-detalles';

import {
  ServiciosService,
  Servicio
} from '../../../core/services/servicios';

import {
  PacientesService,
  Paciente
} from '../../../core/services/pacientes';

import {
  PagosService,
  Pago
} from '../../../core/services/pagos';

import {
  FormasPagoService,
  FormaPago
} from '../../../core/services/formas-pago';

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
  selector: 'app-facturas',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],

  templateUrl: './facturas.html',
  styleUrl: './facturas.css'
})
export class Facturas implements OnInit, OnDestroy {

  facturas =
    signal<Factura[]>([]);

  pacientes =
    signal<Paciente[]>([]);

  servicios =
    signal<Servicio[]>([]);

  detallesFactura =
    signal<FacturaDetalle[]>([]);

  pagosFactura =
    signal<Pago[]>([]);

  formasPago =
    signal<FormaPago[]>([]);

  paginaActual = 1;

  tamanoPagina = 10;

  totalRegistros = 0;

  totalPaginas = 0;

  mostrarFormulario = false;

  cargandoDetalles = false;

  guardando = false;

  modoFormulario:
    'nuevo' |
    'ver' |
    'editar' =
    'nuevo';

  facturaSeleccionada:
    Factura | null = null;

  nuevaFactura:
    Factura =
    this.crearFacturaVacia();

  servicioSeleccionado = 0;

  cantidadServicio = 1;

  detallesOriginales:
    FacturaDetalle[] = [];

  mostrarModalPago =
    signal(false);

  cargandoPago =
    signal(false);

  guardandoPago =
    signal(false);

  facturaPago =
    signal<Factura | null>(null);

  totalFacturaPago =
    signal(0);

  nuevoPago:
    Pago =
    this.crearPagoVacio();

  totalPagadoFactura =
    computed(() => {

      return this.pagosFactura()
        .reduce(
          (total, pago) =>
            total + Number(pago.monto),
          0
        );
    });

  saldoFactura =
    computed(() => {

      return Math.max(
        this.totalFacturaPago() -
        this.totalPagadoFactura(),
        0
      );
    });

  constructor(
    private facturasService:
      FacturasService,

    private facturaDetallesService:
      FacturaDetallesService,

    private serviciosService:
      ServiciosService,

    private pacientesService:
      PacientesService,

    private pagosService:
      PagosService,

    private formasPagoService:
      FormasPagoService,

    private busquedaService:
      BusquedaService
  ) {

    effect(() => {

      this.busquedaService.texto();

      untracked(() => {

        this.paginaActual = 1;

        this.cargarFacturas();
      });
    });
  }

  ngOnInit(): void {

    this.busquedaService.configurar(
      'Buscar facturas...'
    );

    this.cargarPacientes();

    this.cargarServicios();

    this.cargarFormasPago();
  }

  ngOnDestroy(): void {

    this.busquedaService
      .establecerSugerencias([]);
  }

  cargarFacturas(): void {

    const texto =
      this.busquedaService.texto();

    const filtro =
      this.crearFiltroBusqueda(
        texto
      );

    this.facturasService
      .getFacturas(
        this.paginaActual,
        this.tamanoPagina,
        filtro
      )
      .subscribe({

        next: resultado => {

          this.facturas.set(
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

  cargarServicios(): void {

    this.serviciosService
      .getServicios(
        1,
        100
      )
      .subscribe({

        next: resultado => {

          this.servicios.set(
            resultado.data ?? []
          );
        },

        error: error => {

          console.error(
            'Error cargando servicios:',
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
            (resultado.data ?? [])
              .filter(
                forma =>
                  forma.estado
              )
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

  obtenerNombrePaciente(
    idPaciente: number
  ): string {

    const paciente =
      this.pacientes()
        .find(
          item =>
            item.idPaciente ===
            idPaciente
        );

    if (!paciente) {

      return `Paciente #${idPaciente}`;
    }

    return `${paciente.nombres} ${paciente.apellidos}`
      .trim();
  }

  obtenerClaseEstado(
    estado: string
  ): string {

    switch (
      estado
        .toLowerCase()
    ) {

      case 'pagada':
        return 'estado-pagada';

      case 'parcial':
        return 'estado-parcial';

      case 'cancelada':
        return 'estado-cancelada';

      default:
        return 'estado-pendiente';
    }
  }

  agregarServicio(): void {

    if (
      this.servicioSeleccionado <= 0
    ) {

      alert(
        'Seleccione un servicio.'
      );

      return;
    }

    if (
      this.cantidadServicio <= 0
    ) {

      alert(
        'La cantidad debe ser mayor a cero.'
      );

      return;
    }

    const servicio =
      this.servicios()
        .find(
          item =>
            item.idServicio ===
            this.servicioSeleccionado
        );

    if (!servicio) {
      return;
    }

    const detalles = [
      ...this.detallesFactura()
    ];

    const existente =
      detalles.find(
        detalle =>
          detalle.tipoItem === 'Servicio' &&
          detalle.idItem === servicio.idServicio
      );

    if (existente) {

      existente.cantidad +=
        this.cantidadServicio;

      this.detallesFactura.set([
        ...detalles
      ]);

    } else {

      const detalle:
        FacturaDetalle = {

        idDetalle: 0,

        idFactura:
          this.nuevaFactura.idFactura,

        tipoItem:
          'Servicio',

        idItem:
          servicio.idServicio,

        descripcion:
          servicio.nombre,

        cantidad:
          this.cantidadServicio,

        precioUnitario:
          servicio.precio
      };

      this.detallesFactura.set([
        ...detalles,
        detalle
      ]);
    }

    this.servicioSeleccionado = 0;

    this.cantidadServicio = 1;
  }

  eliminarServicio(
    indice: number
  ): void {

    const detalles = [
      ...this.detallesFactura()
    ];

    detalles.splice(
      indice,
      1
    );

    this.detallesFactura.set(
      detalles
    );
  }

  calcularSubtotal(
    detalle: FacturaDetalle
  ): number {

    return (
      Number(detalle.cantidad) *
      Number(detalle.precioUnitario)
    );
  }

  calcularTotalFactura(): number {

    return this.detallesFactura()
      .reduce(
        (total, detalle) =>
          total +
          this.calcularSubtotal(
            detalle
          ),
        0
      );
  }

  abrirNuevaFactura(): void {

    this.modoFormulario =
      'nuevo';

    this.facturaSeleccionada =
      null;

    this.nuevaFactura =
      this.crearFacturaVacia();

    this.detallesFactura.set([]);

    this.detallesOriginales = [];

    this.servicioSeleccionado = 0;

    this.cantidadServicio = 1;

    this.mostrarFormulario = true;
  }

  verFactura(
    factura: Factura
  ): void {

    this.modoFormulario =
      'ver';

    this.facturaSeleccionada =
      factura;

    this.nuevaFactura = {
      ...factura
    };

    this.detallesFactura.set([]);

    this.detallesOriginales = [];

    this.mostrarFormulario = true;

    this.cargarDetallesFactura(
      factura.idFactura
    );
  }

  editarFactura(
    factura: Factura
  ): void {

    this.modoFormulario =
      'editar';

    this.facturaSeleccionada =
      factura;

    this.nuevaFactura = {
      ...factura
    };

    this.nuevaFactura.fechaFactura =
      this.formatearFechaInput(
        factura.fechaFactura
      );

    this.detallesFactura.set([]);

    this.detallesOriginales = [];

    this.mostrarFormulario = true;

    this.cargarDetallesFactura(
      factura.idFactura
    );
  }

  cargarDetallesFactura(
    idFactura: number
  ): void {

    this.cargandoDetalles = true;

    this.facturaDetallesService
      .getDetallesFactura(
        idFactura
      )
      .subscribe({

        next: resultado => {

          const detalles =
            resultado.data ?? [];

          this.detallesFactura.set(
            detalles.map(
              detalle => ({
                ...detalle
              })
            )
          );

          this.detallesOriginales =
            detalles.map(
              detalle => ({
                ...detalle
              })
            );

          this.cargandoDetalles =
            false;
        },

        error: error => {

          console.error(
            'Error cargando detalles:',
            error
          );

          this.detallesFactura.set([]);

          this.detallesOriginales = [];

          this.cargandoDetalles =
            false;
        }
      });
  }

  validarFactura(): boolean {

    if (
      this.nuevaFactura.idPaciente <=
      0
    ) {

      alert(
        'Seleccione un paciente.'
      );

      return false;
    }

    if (
      !this.nuevaFactura.fechaFactura
    ) {

      alert(
        'Ingrese la fecha de la factura.'
      );

      return false;
    }

    if (
      this.detallesFactura()
        .length === 0
    ) {

      alert(
        'Agregue al menos un servicio.'
      );

      return false;
    }

    for (
      const detalle of
      this.detallesFactura()
    ) {

      if (
        Number(detalle.cantidad) <= 0
      ) {

        alert(
          'Las cantidades deben ser mayores a cero.'
        );

        return false;
      }
    }

    return true;
  }

  guardarFactura(): void {

    if (
      !this.validarFactura()
    ) {
      return;
    }

    if (
      this.modoFormulario ===
      'nuevo'
    ) {

      this.crearFactura(
        false
      );

      return;
    }

    if (
      this.modoFormulario ===
      'editar'
    ) {

      this.actualizarFactura(
        false
      );
    }
  }

  pagarFactura(): void {

    if (
      !this.validarFactura()
    ) {
      return;
    }

    if (
      this.modoFormulario ===
      'nuevo'
    ) {

      this.crearFactura(
        true
      );

      return;
    }

    if (
      this.modoFormulario ===
      'editar'
    ) {

      this.actualizarFactura(
        true
      );

      return;
    }

    this.abrirPagoFacturaExistente();
  }

  private crearFactura(
    abrirPago: boolean
  ): void {

    this.guardando = true;

    const total =
      this.calcularTotalFactura();

    const factura:
      Factura = {

      ...this.nuevaFactura,

      estado:
        'Pendiente'
    };

    this.facturasService
      .addFactura(
        factura
      )
      .pipe(

        switchMap(
          facturaCreada => {

            const idFactura =
              facturaCreada.idFactura;

            const peticiones =
              this.detallesFactura()
                .map(
                  detalle =>

                    this.facturaDetallesService
                      .agregarDetalle({
                        ...detalle,
                        idDetalle: 0,
                        idFactura
                      })
                );

            if (
              peticiones.length === 0
            ) {

              return of({
                factura: facturaCreada,
                total
              });
            }

            return forkJoin(
              peticiones
            )
              .pipe(
                map(
                  () => ({
                    factura:
                      facturaCreada,
                    total
                  })
                )
              );
          }
        )
      )
      .subscribe({

        next: resultado => {

          this.guardando =
            false;

          this.paginaActual = 1;

          this.cargarFacturas();

          if (
            abrirPago
          ) {

            this.prepararPago(
              resultado.factura,
              resultado.total
            );

            return;
          }

          this.cerrarFormulario();
        },

        error: error => {

          this.guardando =
            false;

          console.error(
            'Error creando factura:',
            error
          );

          alert(
            error.error?.mensaje ??
            'No se pudo crear la factura.'
          );
        }
      });
  }

  private actualizarFactura(
    abrirPago: boolean
  ): void {

    this.guardando =
      true;

    const total =
      this.calcularTotalFactura();

    this.facturasService
      .updateFactura(
        this.nuevaFactura
      )
      .pipe(

        switchMap(
          () =>
            this.sincronizarDetalles(
              this.nuevaFactura
                .idFactura
            )
        )
      )
      .subscribe({

        next: () => {

          this.guardando =
            false;

          this.cargarFacturas();

          if (
            abrirPago
          ) {

            this.prepararPago(
              {
                ...this.nuevaFactura
              },
              total
            );

            return;
          }

          this.cerrarFormulario();
        },

        error: error => {

          this.guardando =
            false;

          console.error(
            'Error actualizando factura:',
            error
          );

          alert(
            'No se pudo actualizar la factura.'
          );
        }
      });
  }

  private sincronizarDetalles(
    idFactura: number
  ): Observable<any> {

    const actuales =
      this.detallesFactura();

    const operaciones:
      Observable<any>[] = [];

    for (
      const detalle of actuales
    ) {

      if (
        detalle.idDetalle === 0
      ) {

        operaciones.push(

          this.facturaDetallesService
            .agregarDetalle({
              ...detalle,
              idFactura
            })
        );

      } else {

        operaciones.push(

          this.facturaDetallesService
            .actualizarDetalle({
              ...detalle,
              idFactura
            })
        );
      }
    }

    const idsActuales =
      new Set(
        actuales
          .filter(
            detalle =>
              detalle.idDetalle > 0
          )
          .map(
            detalle =>
              detalle.idDetalle
          )
      );

    for (
      const original of
      this.detallesOriginales
    ) {

      if (
        original.idDetalle > 0 &&
        !idsActuales.has(
          original.idDetalle
        )
      ) {

        operaciones.push(

          this.facturaDetallesService
            .eliminarDetalle(
              original.idDetalle
            )
        );
      }
    }

    if (
      operaciones.length === 0
    ) {

      return of([]);
    }

    return forkJoin(
      operaciones
    );
  }

  private abrirPagoFacturaExistente():
    void {

    const factura = {
      ...this.nuevaFactura
    };

    const total =
      this.calcularTotalFactura();

    this.prepararPago(
      factura,
      total
    );
  }

  private prepararPago(
    factura: Factura,
    total: number
  ): void {

    this.facturaPago.set(
      factura
    );

    this.totalFacturaPago.set(
      total
    );

    this.pagosFactura.set([]);

    this.nuevoPago =
      this.crearPagoVacio();

    this.nuevoPago.idFactura =
      factura.idFactura;

    this.mostrarFormulario =
      false;

    this.mostrarModalPago.set(
      true
    );

    this.cargandoPago.set(
      true
    );

    this.cargarPagosParaFactura(
      factura.idFactura
    );
  }

  private cargarPagosParaFactura(
    idFactura: number
  ): void {

    this.pagosService
      .getPagosFactura(
        idFactura
      )
      .subscribe({

        next: resultado => {

          this.pagosFactura.set(
            resultado.data ?? []
          );

          this.cargandoPago.set(
            false
          );
        },

        error: error => {

          console.error(
            'Error cargando pagos:',
            error
          );

          this.pagosFactura.set([]);

          this.cargandoPago.set(
            false
          );
        }
      });
  }

  registrarPago(): void {

    const factura =
      this.facturaPago();

    if (!factura) {
      return;
    }

    if (
      this.nuevoPago.idFormaPago <=
      0
    ) {

      alert(
        'Seleccione una forma de pago.'
      );

      return;
    }

    const monto =
      Number(
        this.nuevoPago.monto
      );

    if (
      !Number.isFinite(monto) ||
      monto <= 0
    ) {

      alert(
        'Ingrese un monto válido.'
      );

      return;
    }

    if (
      monto >
      this.saldoFactura()
    ) {

      alert(
        'El monto no puede superar el saldo pendiente.'
      );

      return;
    }

    if (
      !this.nuevoPago.fechaPago
    ) {

      alert(
        'Ingrese la fecha del pago.'
      );

      return;
    }

    const pago:
      Pago = {

      idPago: 0,

      idFactura:
        factura.idFactura,

      idFormaPago:
        this.nuevoPago
          .idFormaPago,

      monto,

      fechaPago:
        this.nuevoPago
          .fechaPago
    };

    this.guardandoPago.set(
      true
    );

    this.pagosService
      .addPago(
        pago
      )
      .subscribe({

        next: () => {

          this.guardandoPago.set(
            false
          );

          this.nuevoPago =
            this.crearPagoVacio();

          this.nuevoPago.idFactura =
            factura.idFactura;

          this.cargarPagosParaFactura(
            factura.idFactura
          );

          this.cargarFacturas();
        },

        error: error => {

          this.guardandoPago.set(
            false
          );

          console.error(
            'Error registrando pago:',
            error
          );

          alert(
            error.error?.mensaje ??
            'No se pudo registrar el pago.'
          );
        }
      });
  }

  eliminarFactura(
    factura: Factura
  ): void {

    const confirmar =
      confirm(
        `¿Desea eliminar la factura #${factura.idFactura}?`
      );

    if (
      !confirmar
    ) {
      return;
    }

    this.facturasService
      .deleteFactura(
        factura.idFactura
      )
      .subscribe({

        next: () => {

          if (
            this.facturas().length === 1 &&
            this.paginaActual > 1
          ) {

            this.paginaActual--;
          }

          this.cargarFacturas();
        },

        error: error => {

          console.error(
            'Error eliminando factura:',
            error
          );

          alert(
            error.error?.mensaje ??
            'No se pudo eliminar la factura.'
          );
        }
      });
  }

  cerrarModalPago(): void {

    if (
      this.guardandoPago()
    ) {
      return;
    }

    this.mostrarModalPago.set(
      false
    );

    this.cargandoPago.set(
      false
    );

    this.facturaPago.set(
      null
    );

    this.pagosFactura.set([]);

    this.totalFacturaPago.set(
      0
    );

    this.nuevoPago =
      this.crearPagoVacio();

    this.limpiarFormulario();

    this.cargarFacturas();
  }

  cerrarFormulario(): void {

    if (
      this.guardando
    ) {
      return;
    }

    this.mostrarFormulario =
      false;

    this.limpiarFormulario();
  }

  private limpiarFormulario():
    void {

    this.modoFormulario =
      'nuevo';

    this.facturaSeleccionada =
      null;

    this.detallesFactura.set([]);

    this.detallesOriginales = [];

    this.servicioSeleccionado = 0;

    this.cantidadServicio = 1;

    this.cargandoDetalles = false;

    this.guardando = false;

    this.nuevaFactura =
      this.crearFacturaVacia();
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

    this.cargarFacturas();
  }

  cambiarTamanoPagina(
    tamano: number
  ): void {

    this.tamanoPagina =
      Number(tamano);

    this.paginaActual = 1;

    this.cargarFacturas();
  }

  private crearFiltroBusqueda(
    texto: string
  ): string {

    const valor =
      texto.trim();

    if (!valor) {
      return '';
    }

    const seguro =
      valor
        .replace(
          /\\/g,
          '\\\\'
        )
        .replace(
          /"/g,
          '\\"'
        );

    const numero =
      Number(valor);

    if (
      !Number.isNaN(numero)
    ) {

      return (
        `IdFactura == ${numero} || ` +
        `IdPaciente == ${numero} || ` +
        `Estado.Contains("${seguro}")`
      );
    }

    return (
      `Estado.Contains("${seguro}")`
    );
  }

  private crearFacturaVacia():
    Factura {

    return {

      idFactura: 0,

      idPaciente: 0,

      fechaFactura:
        this.obtenerFechaActual(),

      estado:
        'Pendiente'
    };
  }

  private crearPagoVacio():
    Pago {

    return {

      idPago: 0,

      idFactura: 0,

      idFormaPago: 0,

      monto: 0,

      fechaPago:
        this.obtenerFechaActual()
    };
  }

  private obtenerFechaActual():
    string {

    const fecha =
      new Date();

    const offset =
      fecha.getTimezoneOffset();

    return new Date(
      fecha.getTime() -
      offset * 60000
    )
      .toISOString()
      .slice(
        0,
        16
      );
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