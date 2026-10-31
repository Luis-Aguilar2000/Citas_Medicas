import { Injectable } from '@angular/core';
import { FacturaDetalle } from '../../../../core/services/factura-detalles';
import { Servicio } from '../../../../core/services/servicios';
import { Medicamento } from '../../../../core/services/medicamentos';
import { CatalogoEstudio } from '../../../../core/services/catalogo-estudios';
import { ItemFacturable, TipoItemFactura } from '../models/item-facturable';

@Injectable({
  providedIn: 'root'
})
export class FacturaDetalleManagerService {
  obtenerItem(
    tipo: TipoItemFactura,
    id: number,
    servicios: Servicio[],
    medicamentos: Medicamento[],
    estudios: CatalogoEstudio[]
  ): ItemFacturable | null {
    switch (tipo) {
      case 'Servicio': {
        const servicio = servicios.find(item => item.idServicio === id);
        if (!servicio) return null;
        return {
          tipo: 'Servicio',
          id: servicio.idServicio,
          descripcion: servicio.nombre,
          precio: Number(servicio.precio)
        };
      }

      case 'Medicamento': {
        const medicamento = medicamentos.find(item => item.idMedicamento === id);
        if (!medicamento) return null;

        let descripcion = medicamento.nombreComercial;
        if (medicamento.concentracion) {
          descripcion += ` ${medicamento.concentracion}`;
        }
        if (medicamento.presentacion) {
          descripcion += ` - ${medicamento.presentacion}`;
        }

        return {
          tipo: 'Medicamento',
          id: medicamento.idMedicamento,
          descripcion,
          precio: Number(medicamento.precio)
        };
      }

      case 'Estudio': {
        const estudio = estudios.find(item => item.idCatalogoEstudio === id);
        if (!estudio) return null;
        return {
          tipo: 'Estudio',
          id: estudio.idCatalogoEstudio,
          descripcion: estudio.nombreEstudio,
          precio: Number(estudio.precio)
        };
      }
    }
  }

  agregar(
    detalles: FacturaDetalle[],
    item: ItemFacturable,
    cantidad: number,
    idFactura: number
  ): FacturaDetalle[] {
    const nuevos = detalles.map(detalle => ({ ...detalle }));
    const existente = nuevos.find(
      detalle => detalle.tipoItem === item.tipo && detalle.idItem === item.id
    );

    if (existente) {
      existente.cantidad = Number(existente.cantidad) + Number(cantidad);
      return nuevos;
    }

    return [
      ...nuevos,
      {
        idDetalle: 0,
        idFactura,
        tipoItem: item.tipo,
        idItem: item.id,
        descripcion: item.descripcion,
        cantidad: Number(cantidad),
        precioUnitario: item.precio
      }
    ];
  }

  eliminar(
    detalles: FacturaDetalle[],
    indice: number
  ): FacturaDetalle[] {
    return detalles.filter((_, index) => index !== indice);
  }

  calcularSubtotal(detalle: FacturaDetalle): number {
    return Number(detalle.cantidad) * Number(detalle.precioUnitario);
  }

  calcularTotal(detalles: FacturaDetalle[]): number {
    return detalles.reduce(
      (total, detalle) => total + this.calcularSubtotal(detalle),
      0
    );
  }

  validar(detalles: FacturaDetalle[]): string | null {
    if (detalles.length === 0) {
      return 'Agregue al menos un ítem a la factura.';
    }

    const cantidadInvalida = detalles.some(
      detalle => Number(detalle.cantidad) <= 0
    );

    if (cantidadInvalida) {
      return 'Las cantidades deben ser mayores a cero.';
    }

    return null;
  }
}