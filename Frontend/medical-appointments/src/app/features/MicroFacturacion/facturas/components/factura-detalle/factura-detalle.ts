import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FacturaDetalle } from '../../../../../core/services/factura-detalles';
import { Servicio } from '../../../../../core/services/servicios';
import { Medicamento } from '../../../../../core/services/medicamentos';
import { CatalogoEstudio } from '../../../../../core/services/catalogo-estudios';
import { TipoItemFactura } from '../../models/item-facturable';
import { FacturaDetalleManagerService } from '../../services/factura-detalle-manager';

@Component({
  selector: 'app-factura-detalle',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './factura-detalle.html',
  styleUrl: './factura-detalle.css'
})
export class FacturaDetalleComponent {
  @Input() idFactura = 0;
  @Input() editable = true;
  @Input() cargando = false;
  @Input() servicios: Servicio[] = [];
  @Input() medicamentos: Medicamento[] = [];
  @Input() estudios: CatalogoEstudio[] = [];
  @Input() detalles: FacturaDetalle[] = [];

  @Output() detallesChange = new EventEmitter<FacturaDetalle[]>();

  tipoItemSeleccionado: TipoItemFactura = 'Servicio';
  itemSeleccionado = 0;
  cantidadItem = 1;

  constructor(
    private detalleManager: FacturaDetalleManagerService
  ) {}

  cambiarTipoItem(): void {
    this.itemSeleccionado = 0;
    this.cantidadItem = 1;
  }

  agregarItem(): void {
    if (this.itemSeleccionado <= 0) {
      alert('Seleccione un ítem.');
      return;
    }

    if (Number(this.cantidadItem) <= 0) {
      alert('La cantidad debe ser mayor a cero.');
      return;
    }

    const item = this.detalleManager.obtenerItem(
      this.tipoItemSeleccionado,
      this.itemSeleccionado,
      this.servicios,
      this.medicamentos,
      this.estudios
    );

    if (!item) {
      alert('No se encontró el ítem seleccionado.');
      return;
    }

    const nuevos = this.detalleManager.agregar(
      this.detalles,
      item,
      this.cantidadItem,
      this.idFactura
    );

    this.detallesChange.emit(nuevos);

    this.itemSeleccionado = 0;
    this.cantidadItem = 1;
  }

  eliminarItem(indice: number): void {
    const nuevos = this.detalleManager.eliminar(
      this.detalles,
      indice
    );

    this.detallesChange.emit(nuevos);
  }

  calcularSubtotal(detalle: FacturaDetalle): number {
    return this.detalleManager.calcularSubtotal(detalle);
  }

  calcularTotal(): number {
    return this.detalleManager.calcularTotal(this.detalles);
  }
}