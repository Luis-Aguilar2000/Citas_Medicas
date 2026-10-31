import {Component,EventEmitter,Input,OnInit,OnDestroy,Output,computed,inject,signal} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {Subject} from 'rxjs';
import {takeUntil} from 'rxjs/operators';
import {FacturaDetalle} from '../../../../core/services/factura-detalles';
import {Servicio,ServiciosService} from '../../../../core/services/servicios';
@Component({
  selector: 'app-factura-detalle',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './factura-detalle.html',
  styleUrl: './factura-detalle.css'
})
export class FacturaDetalleComponent implements OnInit, OnDestroy {
  private serviciosService = inject(ServiciosService);
  private destroy$ = new Subject<void>();
  @Input() modo: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  @Input() cargando = false;
  @Input() set detalles(value: FacturaDetalle[]) { this.detallesInternos.set(value ?? []); }
  @Output() detallesChange = new EventEmitter<FacturaDetalle[]>();
  detallesInternos = signal<FacturaDetalle[]>([]);
  servicios = signal<Servicio[]>([]);
  private serviciosMap = computed(() => {
    const map = new Map<number, Servicio>();
    for (const servicio of this.servicios()) { map.set(servicio.idServicio, servicio); }
    return map;
  });
  servicioSeleccionado = 0;
  cantidadServicio = 1;
  ngOnInit(): void {
    if (this.modo === 'nuevo' || this.modo === 'editar') { this.cargarServicios(); }
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  cargarServicios(): void {
    this.serviciosService.getServicios(1, 100)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: resultado => this.servicios.set(resultado.data.filter(s => s.estado)),
        error: error => console.error('Error cargando servicios:', error)
      });
  }
  agregarServicio(): void {
    if (this.servicioSeleccionado <= 0) { alert('Seleccione un servicio.'); return; }
    if (this.cantidadServicio <= 0) { alert('La cantidad debe ser mayor a cero.'); return; }
    const servicio = this.serviciosMap().get(this.servicioSeleccionado);
    if (!servicio) return;
    const detalles = [...this.detallesInternos()];
    const indiceExistente = detalles.findIndex(d => d.tipoItem === 'Servicio' && d.idItem === servicio.idServicio);
    if (indiceExistente >= 0) {
      detalles[indiceExistente] = {
        ...detalles[indiceExistente],
        cantidad: Number(detalles[indiceExistente].cantidad) + Number(this.cantidadServicio)
      };
    } else {
      detalles.push({
        idDetalle: 0,
        idFactura: 0,
        tipoItem: 'Servicio',
        idItem: servicio.idServicio,
        descripcion: servicio.nombre,
        cantidad: Number(this.cantidadServicio),
        precioUnitario: Number(servicio.precio)
      });
    }
    this.actualizarDetalles(detalles);
    this.servicioSeleccionado = 0;
    this.cantidadServicio = 1;
  }
  cambiarCantidad(indice: number, cantidad: number): void {
    let nuevaCantidad = Number(cantidad);
    if (!Number.isFinite(nuevaCantidad) || nuevaCantidad < 1) { nuevaCantidad = 1; }
    nuevaCantidad = Math.floor(nuevaCantidad);
    const detalles = [...this.detallesInternos()];
    if (!detalles[indice]) return;
    detalles[indice] = { ...detalles[indice], cantidad: nuevaCantidad };
    this.actualizarDetalles(detalles);
  }
  eliminarServicio(indice: number): void {
    const detalles = [...this.detallesInternos()];
    if (indice < 0 || indice >= detalles.length) return;
    detalles.splice(indice, 1);
    this.actualizarDetalles(detalles);
  }
  calcularSubtotal(detalle: FacturaDetalle): number {
    return Number(detalle.cantidad) * Number(detalle.precioUnitario);
  }
  calcularTotal(): number {
    return this.detallesInternos().reduce((total, detalle) => total + this.calcularSubtotal(detalle), 0);
  }
  private actualizarDetalles(detalles: FacturaDetalle[]): void {
    this.detallesInternos.set(detalles);
    this.detallesChange.emit(detalles);
  }
}