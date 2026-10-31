import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Factura } from '../../../../../core/services/facturas';
import { PagosService, Pago } from '../../../../../core/services/pagos';
import { FormasPagoService, FormaPago } from '../../../../../core/services/formas-pago';

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

  totalPagado = computed(() =>
    this.pagos().reduce((total, pago) => total + Number(pago.monto), 0)
  );

  saldo = computed(() =>
    Math.max(Number(this.total) - this.totalPagado(), 0)
  );

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

  private preparar(): void {
    if (!this.factura) return;
    this.nuevoPago = this.crearPagoVacio();
    this.nuevoPago.idFactura = this.factura.idFactura;
    this.cargarPagos();
  }

  private cargarFormasPago(): void {
    this.formasPagoService.getFormasPago(1, 100).subscribe({
      next: resultado => {
        this.formasPago.set(
          (resultado.data ?? []).filter(forma => forma.estado)
        );
      },
      error: error => {
        console.error('Error cargando formas de pago:', error);
      }
    });
  }

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

  registrarPago(): void {
    if (!this.factura) return;

    if (this.nuevoPago.idFormaPago <= 0) {
      alert('Seleccione una forma de pago.');
      return;
    }

    const monto = Number(this.nuevoPago.monto);

    if (!Number.isFinite(monto) || monto <= 0) {
      alert('Ingrese un monto válido.');
      return;
    }

    if (monto > this.saldo()) {
      alert('El monto no puede superar el saldo pendiente.');
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

  cerrarModal(): void {
    if (this.guardando()) return;
    this.pagos.set([]);
    this.cerrar.emit();
  }

  private crearPagoVacio(): Pago {
    return {
      idPago: 0,
      idFactura: 0,
      idFormaPago: 0,
      monto: 0,
      fechaPago: this.obtenerFechaActual()
    };
  }

  private obtenerFechaActual(): string {
    const fecha = new Date();
    const offset = fecha.getTimezoneOffset();
    return new Date(fecha.getTime() - offset * 60000).toISOString().slice(0, 16);
  }
}