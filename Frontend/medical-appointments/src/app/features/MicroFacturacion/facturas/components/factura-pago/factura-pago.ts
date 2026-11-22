import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  computed,
  signal
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Factura } from '../../../../../core/services/MicroFacturacion/facturas';
import { PagosService, Pago } from '../../../../../core/services/MicroFacturacion/pagos';
import { FormasPagoService, FormaPago } from '../../../../../core/services/MicroFacturacion/formas-pago';

@Component({
  selector: 'app-factura-pago',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './factura-pago.html',
  styleUrl: './factura-pago.css'
})
export class FacturaPagoComponent implements OnChanges {
  @Input() factura: Factura | null = null;
  @Input() total = 0;
  @Input() visible = false;

  @Output() cerrar = new EventEmitter<void>();
  @Output() pagoRegistrado = new EventEmitter<void>();

  pagos = signal<Pago[]>([]);
  formasPago = signal<FormaPago[]>([]);
  cargando = signal(false);
  guardando = signal(false);

  nuevoPago: Pago = this.crearPagoVacio();
  montoPago: number | null = null;

  // Propiedades computadas reactivas con redondeo monetario seguro
  totalPagado = computed(() => {
    const total = this.pagos().reduce((suma, pago) => suma + Number(pago.monto), 0);
    return this.redondearMoneda(total);
  });

  saldo = computed(() => {
    const saldo = Number(this.total) - this.totalPagado();
    return Math.max(this.redondearMoneda(saldo), 0);
  });

  pagoCompleto = computed(() => this.saldo() <= 0);

  constructor(
    private pagosService: PagosService,
    private formasPagoService: FormasPagoService
  ) {
    this.cargarFormasPago();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['visible'] && this.visible && this.factura) {
      this.preparar();
    }
  }

  /** Prepara el estado inicial del componente al abrirse el modal. */
  private preparar(): void {
    if (!this.factura) return;

    this.pagos.set([]);
    this.cargando.set(false);
    this.guardando.set(false);
    this.montoPago = null;
    this.nuevoPago = this.crearPagoVacio();
    this.nuevoPago.idFactura = this.factura.idFactura;

    this.cargarPagos();
  }

  /** Carga el catÃ¡logo de formas de pago activas. */
  private cargarFormasPago(): void {
    this.formasPagoService.getFormasPago(1, 100).subscribe({
      next: resultado => {
        this.formasPago.set((resultado.data ?? []).filter(forma => forma.estado));
      },
      error: error => {
        console.error('Error cargando formas de pago:', error);
        this.formasPago.set([]);
      }
    });
  }

  /** Carga los pagos registrados asociados a la factura actual. */
  cargarPagos(): void {
    if (!this.factura) return;

    this.cargando.set(true);
    this.pagosService.getPagosFactura(this.factura.idFactura).subscribe({
      next: resultado => {
        this.pagos.set(resultado.data ?? []);
        this.cargando.set(false);
      },
      error: error => {
        console.error('Error cargando pagos:', error);
        this.pagos.set([]);
        this.cargando.set(false);
      }
    });
  }

  /** Valida y registra un nuevo pago para la factura. */
  registrarPago(): void {
    if (!this.factura || this.guardando() || this.cargando()) return;

    if (this.pagoCompleto()) {
      alert('La factura ya estÃ¡ pagada completamente.');
      return;
    }

    if (this.nuevoPago.idFormaPago <= 0) {
      alert('Seleccione una forma de pago.');
      return;
    }

    if (this.montoPago === null || this.montoPago === undefined) {
      alert('Ingrese el monto del pago.');
      return;
    }

    const monto = this.redondearMoneda(Number(this.montoPago));

    if (!Number.isFinite(monto) || monto <= 0) {
      alert('El monto del pago debe ser mayor a cero.');
      return;
    }

    const saldoPendiente = this.redondearMoneda(this.saldo());

    if (saldoPendiente <= 0) {
      alert('La factura no tiene saldo pendiente.');
      return;
    }

    if (monto > saldoPendiente) {
      alert(`El monto no puede superar el saldo pendiente de $${saldoPendiente.toFixed(2)}.`);
      return;
    }

    if (!this.nuevoPago.fechaPago) {
      alert('Ingrese la fecha del pago.');
      return;
    }

    const pago: Pago = {
      idPago: 0,
      idFactura: this.factura.idFactura,
      idFormaPago: this.nuevoPago.idFormaPago,
      monto,
      fechaPago: this.nuevoPago.fechaPago
    };

    this.guardando.set(true);

    this.pagosService.addPago(pago).subscribe({
      next: () => {
        this.guardando.set(false);
        this.montoPago = null;
        this.nuevoPago = this.crearPagoVacio();
        this.nuevoPago.idFactura = this.factura!.idFactura;
        this.cargarPagos();
        this.pagoRegistrado.emit();
      },
      error: error => {
        this.guardando.set(false);
        console.error('Error registrando pago:', error);
        alert(error.error?.mensaje ?? 'No se pudo registrar el pago.');
      }
    });
  }

  /** Cierra el modal de pagos si no hay procesos activos. */
  cerrarModal(): void {
    if (this.guardando()) return;

    this.pagos.set([]);
    this.cargando.set(false);
    this.montoPago = null;
    this.nuevoPago = this.crearPagoVacio();
    this.cerrar.emit();
  }

  /** Crea una estructura de pago vacÃ­a con valores por defecto. */
  private crearPagoVacio(): Pago {
    return {
      idPago: 0,
      idFactura: 0,
      idFormaPago: 0,
      monto: 0,
      fechaPago: this.obtenerFechaActual()
    };
  }

  /** Redondea un valor numÃ©rico a dos decimales de manera precisa. */
  private redondearMoneda(valor: number): number {
    return Math.round((Number(valor) + Number.EPSILON) * 100) / 100;
  }

  /** Obtiene la fecha actual para campos tipo input datetime-local. */
  private obtenerFechaActual(): string {
    const fecha = new Date();
    const offset = fecha.getTimezoneOffset();
    return new Date(fecha.getTime() - offset * 60000).toISOString().slice(0, 16);
  }
}
