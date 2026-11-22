import {
  Component,
  OnInit,
  OnDestroy,
  signal,
  effect,
  untracked
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin, of, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { FacturasService, Factura } from '../../../core/services/MicroFacturacion/facturas';
import { FacturaDetallesService, FacturaDetalle } from '../../../core/services/MicroFacturacion/factura-detalles';
import { ServiciosService, Servicio } from '../../../core/services/MicroFacturacion/servicios';
import { PagosService } from '../../../core/services/MicroFacturacion/pagos';
import { ImpresionFacturaService } from '../../../core/services/MicroFacturacion/impresion-factura';

import { MedicamentosService, Medicamento } from '../../../core/services/MicroTratamientosLab/medicamentos';
import { CatalogoEstudiosService, CatalogoEstudio } from '../../../core/services/MicroTratamientosLab/catalogo-estudios';

import { PacientesService, Paciente } from '../../../core/services/MicroPacientesAgendas/pacientes';

import { BusquedaService } from '../../../core/services/Compartidos/busqueda';

import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

import { FacturaDetalleComponent } from './components/factura-detalle/factura-detalle';
import { FacturaPagoComponent } from './components/factura-pago/factura-pago';
import { FacturaDetalleManagerService } from './services/factura-detalle-manager';
@Component({
  selector: 'app-facturas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion,
    FacturaDetalleComponent,
    FacturaPagoComponent
  ],
  templateUrl: './facturas.html',
  styleUrl: './facturas.css'
})
export class Facturas implements OnInit, OnDestroy {
  // Signals para el manejo reactivo del estado local y catálogos
  facturas = signal<Factura[]>([]);
  pacientes = signal<Paciente[]>([]);
  servicios = signal<Servicio[]>([]);
  medicamentos = signal<Medicamento[]>([]);
  catalogoEstudios = signal<CatalogoEstudio[]>([]);
  detallesFactura = signal<FacturaDetalle[]>([]);

  // Configuración de paginación
  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  // Estados visuales y de control de modales/formularios
  mostrarFormulario = false;
  mostrarModalPago = false;
  cargandoDetalles = false;
  guardando = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';

  // Modelos de trabajo actuales
  facturaSeleccionada: Factura | null = null;
  facturaPago: Factura | null = null;
  totalFacturaPago = 0;
  nuevaFactura: Factura = this.crearFacturaVacia();
  detallesOriginales: FacturaDetalle[] = [];

  constructor(
    private facturasService: FacturasService,
    private facturaDetallesService: FacturaDetallesService,
    private serviciosService: ServiciosService,
    private medicamentosService: MedicamentosService,
    private catalogoEstudiosService: CatalogoEstudiosService,
    private pacientesService: PacientesService,
    private pagosService: PagosService,
    private impresionFacturaService: ImpresionFacturaService,
    private busquedaService: BusquedaService,
    private detalleManager: FacturaDetalleManagerService
  ) {
    // Efecto reactivo para escuchar cambios en el texto de búsqueda global
    effect(() => {
      this.busquedaService.texto();
      untracked(() => {
        this.paginaActual = 1;
        this.cargarFacturas();
      });
    });
  }

  ngOnInit(): void {
    // Configuración inicial y carga de catálogos base
    this.busquedaService.configurar('Buscar facturas...');
    this.cargarPacientes();
    this.cargarServicios();
    this.cargarMedicamentos();
    this.cargarCatalogoEstudios();
  }

  ngOnDestroy(): void {
    // Limpieza de sugerencias al destruir el componente
    this.busquedaService.establecerSugerencias([]);
  }

  /** Carga la lista paginada de facturas aplicando filtros de búsqueda. */
  cargarFacturas(): void {
    const texto = this.busquedaService.texto();
    const filtro = this.crearFiltroBusqueda(texto);

    this.facturasService.getFacturas(this.paginaActual, this.tamanoPagina, filtro).subscribe({
      next: resultado => {
        this.facturas.set(resultado.data ?? []);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: error => console.error('Error cargando facturas:', error)
    });
  }

  /** Carga el catálogo de pacientes. */
  cargarPacientes(): void {
    this.pacientesService.getPacientes(1, 100).subscribe({
      next: resultado => this.pacientes.set(resultado.data ?? []),
      error: error => console.error('Error cargando pacientes:', error)
    });
  }

  /** Carga el catálogo de servicios médicos. */
  cargarServicios(): void {
    this.serviciosService.getServicios(1, 100).subscribe({
      next: resultado => this.servicios.set(resultado.data ?? []),
      error: error => console.error('Error cargando servicios:', error)
    });
  }

  /** Carga los medicamentos activos del catálogo. */
  cargarMedicamentos(): void {
    this.medicamentosService.getMedicamentos(1, 100, 'Activo == true').subscribe({
      next: resultado => this.medicamentos.set(resultado.data ?? []),
      error: error => console.error('Error cargando medicamentos:', error)
    });
  }

  /** Carga los estudios clínicos activos del catálogo. */
  cargarCatalogoEstudios(): void {
    this.catalogoEstudiosService.getCatalogoEstudios(1, 100, 'Activo == true').subscribe({
      next: resultado => this.catalogoEstudios.set(resultado.data ?? []),
      error: error => console.error('Error cargando estudios:', error)
    });
  }

  /** Obtiene el nombre completo del paciente según su ID. */
  obtenerNombrePaciente(idPaciente: number): string {
    const paciente = this.pacientes().find(item => item.idPaciente === idPaciente);
    return paciente ? `${paciente.nombres} ${paciente.apellidos}`.trim() : `Paciente #${idPaciente}`;
  }

  /** Devuelve la clase CSS correspondiente según el estado de la factura. */
  obtenerClaseEstado(estado: string): string {
    switch (estado?.trim().toLowerCase()) {
      case 'pagada': return 'estado-pagada';
      case 'parcial': return 'estado-parcial';
      case 'cancelada': return 'estado-cancelada';
      default: return 'estado-pendiente';
    }
  }

  /** Valida si una factura se encuentra en estado pendiente para su edición. */
  puedeEditarFactura(factura: Factura): boolean {
    const estado = factura.estado?.trim().toLowerCase() ?? '';
    return estado === 'pendiente';
  }

  /** Valida si una factura puede recibir pagos (pendiente o parcial). */
  puedePagarFactura(factura: Factura): boolean {
    const estado = factura.estado?.trim().toLowerCase() ?? '';
    return estado === 'pendiente' || estado === 'parcial';
  }

  /** Valida si una factura puede ser anulada. */
  puedeAnularFactura(factura: Factura): boolean {
    const estado = factura.estado?.trim().toLowerCase() ?? '';
    return estado !== 'cancelada';
  }

  /** Actualiza localmente los detalles de la factura clonando los objetos. */
  actualizarDetalles(detalles: FacturaDetalle[]): void {
    this.detallesFactura.set(detalles.map(detalle => ({ ...detalle })));
  }

  /** Calcula el monto total general de los detalles de la factura. */
  calcularTotalFactura(): number {
    return this.detalleManager.calcularTotal(this.detallesFactura());
  }

  /** Prepara el formulario para crear una nueva factura. */
  abrirNuevaFactura(): void {
    this.modoFormulario = 'nuevo';
    this.facturaSeleccionada = null;
    this.nuevaFactura = this.crearFacturaVacia();
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.mostrarFormulario = true;
  }

  /** Abre una factura existente en modo de solo lectura. */
  verFactura(factura: Factura): void {
    this.modoFormulario = 'ver';
    this.facturaSeleccionada = factura;
    this.nuevaFactura = { ...factura };
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.mostrarFormulario = true;
    this.cargarDetallesFactura(factura.idFactura);
  }

  /** Carga una factura existente para su modificación. */
  editarFactura(factura: Factura): void {
    if (!this.puedeEditarFactura(factura)) {
      alert('Solo se pueden editar facturas pendientes.');
      return;
    }
    this.modoFormulario = 'editar';
    this.facturaSeleccionada = factura;
    this.nuevaFactura = { ...factura };
    this.nuevaFactura.fechaFactura = this.formatearFechaInput(factura.fechaFactura);
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.mostrarFormulario = true;
    this.cargarDetallesFactura(factura.idFactura);
  }

  /** Carga los ítems de detalle asociados a una factura desde el servidor. */

cargarDetallesFactura(idFactura: number): void {
  this.cargandoDetalles = true;

  this.facturaDetallesService.getDetallesFactura(idFactura).subscribe({
    next: resultado => {
      const detalles: FacturaDetalle[] = (resultado.data ?? []) as FacturaDetalle[];

      this.detallesFactura.set(
        detalles.map((detalle: FacturaDetalle) => ({ ...detalle }))
      );

      this.detallesOriginales = detalles.map(
        (detalle: FacturaDetalle) => ({ ...detalle })
      );

      this.cargandoDetalles = false;
    },
    error: error => {
      console.error('Error cargando detalles:', error);
      this.detallesFactura.set([]);
      this.detallesOriginales = [];
      this.cargandoDetalles = false;
    }
  });
}


  /** Valida la integridad de la cabecera, montos y detalles de la factura. */
  validarFactura(): boolean {
    if (this.nuevaFactura.idPaciente <= 0) {
      alert('Seleccione un paciente.');
      return false;
    }
    if (!this.nuevaFactura.fechaFactura) {
      alert('Ingrese la fecha de la factura.');
      return false;
    }
    const detalles = this.detallesFactura();
    if (detalles.length === 0) {
      alert('La factura debe tener al menos un detalle.');
      return false;
    }
    for (const detalle of detalles) {
      if (Number(detalle.cantidad) <= 0) {
        alert('La cantidad de los detalles debe ser mayor que cero.');
        return false;
      }
      if (Number(detalle.precioUnitario) <= 0) {
        alert('El precio de los detalles debe ser mayor que cero.');
        return false;
      }
    }
    const error = this.detalleManager.validar(detalles);
    if (error) {
      alert(error);
      return false;
    }
    const total = this.calcularTotalFactura();
    if (!Number.isFinite(total) || total <= 0) {
      alert('El total de la factura debe ser mayor que cero.');
      return false;
    }
    return true;
  }

  /** Procesa el almacenamiento de la factura (creación o actualización). */
  guardarFactura(): void {
    if (this.guardando) return;
    if (!this.validarFactura()) return;

    if (this.modoFormulario === 'nuevo') {
      this.crearFactura(false);
      return;
    }
    if (this.modoFormulario === 'editar') {
      if (!this.puedeEditarFactura(this.nuevaFactura)) {
        alert('Esta factura ya no puede editarse.');
        return;
      }
      this.actualizarFactura(false);
    }
  }

  /** Valida y direcciona el flujo de pago de la factura actual. */
  pagarFactura(): void {
    if (this.guardando) return;
    if (!this.validarFactura()) return;

    if (this.modoFormulario === 'nuevo') {
      this.crearFactura(true);
      return;
    }
    if (!this.puedePagarFactura(this.nuevaFactura)) {
      alert('Esta factura no admite nuevos pagos.');
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarFactura(true);
      return;
    }
    this.prepararPago({ ...this.nuevaFactura }, this.calcularTotalFactura());
  }

  /** Envía los datos al servidor para registrar una nueva factura y sus detalles. */
  private crearFactura(abrirPago: boolean): void {
    if (this.guardando) return;
    this.guardando = true;
    const total = this.calcularTotalFactura();
    const factura: Factura = { ...this.nuevaFactura, estado: 'Pendiente' };

    this.facturasService.addFactura(factura).pipe(
      switchMap(facturaCreada => {
        const idFactura = facturaCreada.idFactura;
        const peticiones = this.detallesFactura().map(detalle =>
          this.facturaDetallesService.agregarDetalle({ ...detalle, idDetalle: 0, idFactura })
        );
        if (peticiones.length === 0) return of({ factura: facturaCreada, total });
        return forkJoin(peticiones).pipe(map(() => ({ factura: facturaCreada, total })));
      })
    ).subscribe({
      next: resultado => {
        this.guardando = false;
        this.paginaActual = 1;
        this.cargarFacturas();
        if (abrirPago) {
          this.prepararPago(resultado.factura, resultado.total);
          return;
        }
        this.cerrarFormulario();
      },
      error: error => {
        this.guardando = false;
        console.error('Error creando factura:', error);
        alert(error.error?.mensaje ?? 'No se pudo crear la factura.');
      }
    });
  }

  /** Actualiza una factura existente y sincroniza sus detalles. */
  private actualizarFactura(abrirPago: boolean): void {
    if (this.guardando) return;
    if (!this.puedeEditarFactura(this.nuevaFactura)) {
      alert('Esta factura ya no puede editarse.');
      return;
    }
    this.guardando = true;
    const total = this.calcularTotalFactura();

    this.facturasService.updateFactura(this.nuevaFactura).pipe(
      switchMap(() => this.sincronizarDetalles(this.nuevaFactura.idFactura))
    ).subscribe({
      next: () => {
        this.guardando = false;
        this.cargarFacturas();
        if (abrirPago) {
          this.prepararPago({ ...this.nuevaFactura }, total);
          return;
        }
        this.cerrarFormulario();
      },
      error: error => {
        this.guardando = false;
        console.error('Error actualizando factura:', error);
        alert(error.error?.mensaje ?? 'No se pudo actualizar la factura.');
      }
    });
  }

  /** Sincroniza (agrega, actualiza o elimina) los detalles de la factura. */
  private sincronizarDetalles(idFactura: number): Observable<any> {
    const actuales = this.detallesFactura();
    const operaciones: Observable<any>[] = [];

    for (const detalle of actuales) {
      if (detalle.idDetalle === 0) {
        operaciones.push(this.facturaDetallesService.agregarDetalle({ ...detalle, idFactura }));
      } else {
        operaciones.push(this.facturaDetallesService.actualizarDetalle({ ...detalle, idFactura }));
      }
    }

    const idsActuales = new Set(actuales.filter(d => d.idDetalle > 0).map(d => d.idDetalle));
    for (const original of this.detallesOriginales) {
      if (original.idDetalle > 0 && !idsActuales.has(original.idDetalle)) {
        operaciones.push(this.facturaDetallesService.eliminarDetalle(original.idDetalle));
      }
    }

    if (operaciones.length === 0) return of([]);
    return forkJoin(operaciones);
  }

  /** Configura y abre el modal de pagos para una factura. */
  private prepararPago(factura: Factura, total: number): void {
    if (!this.puedePagarFactura(factura)) {
      alert('Esta factura no admite nuevos pagos.');
      return;
    }
    this.facturaPago = { ...factura };
    this.totalFacturaPago = total;
    this.mostrarFormulario = false;
    this.mostrarModalPago = true;
  }

  /** Cierra el modal de pago y recarga la lista. */
  cerrarModalPago(): void {
    this.mostrarModalPago = false;
    this.facturaPago = null;
    this.totalFacturaPago = 0;
    this.limpiarFormulario();
    this.cargarFacturas();
  }

  /** Callback al registrar un pago con éxito. */
  pagoRegistrado(): void {
    this.cargarFacturas();
  }

  /** Prepara y solicita la impresión de la factura. */
  imprimirFactura(factura?: Factura): void {
    const facturaImprimir = factura ?? this.nuevaFactura;
    if (!facturaImprimir || facturaImprimir.idFactura <= 0) {
      alert('Primero debe crear la factura.');
      return;
    }

    forkJoin({
      detalles: this.facturaDetallesService.getDetallesFactura(facturaImprimir.idFactura),
      pagos: this.pagosService.getPagosFactura(facturaImprimir.idFactura)
    }).subscribe({
      next: resultado => {
        this.impresionFacturaService.imprimir(
          facturaImprimir,
          resultado.detalles.data ?? [],
          resultado.pagos.data ?? [],
          this.pacientes()
        );
      },
      error: error => {
        console.error('Error preparando impresión:', error);
        alert('No se pudo cargar la información de la factura.');
      }
    });
  }

  /** Anula una factura previa confirmación. */
  anularFactura(factura: Factura): void {
    if (!this.puedeAnularFactura(factura)) {
      alert('La factura ya se encuentra cancelada.');
      return;
    }
    if (!confirm(`¿Está seguro de anular la factura #${factura.idFactura}?`)) return;

    this.facturasService.anularFactura(factura.idFactura).subscribe({
      next: () => {
        this.cargarFacturas();
        if (this.nuevaFactura.idFactura === factura.idFactura) {
          this.nuevaFactura = { ...this.nuevaFactura, estado: 'CANCELADA' };
        }
        if (this.facturaSeleccionada?.idFactura === factura.idFactura) {
          this.facturaSeleccionada = { ...this.facturaSeleccionada, estado: 'CANCELADA' };
        }
        alert('Factura anulada correctamente.');
      },
      error: error => {
        console.error('Error anulando factura:', error);
        alert(error.error?.mensaje ?? 'No se pudo anular la factura.');
      }
    });
  }

  /** Cierra el formulario principal si no hay procesos activos. */
  cerrarFormulario(): void {
    if (this.guardando) return;
    this.mostrarFormulario = false;
    this.limpiarFormulario();
  }

  /** Restablece los valores por defecto del formulario. */
  private limpiarFormulario(): void {
    this.modoFormulario = 'nuevo';
    this.facturaSeleccionada = null;
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.cargandoDetalles = false;
    this.guardando = false;
    this.nuevaFactura = this.crearFacturaVacia();
  }

  /** Cambia la página actual de la lista. */
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarFacturas();
  }

  /** Modifica la cantidad de registros por página. */
  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = Number(tamano);
    this.paginaActual = 1;
    this.cargarFacturas();
  }

  /** Construye la cadena de filtro OData/SQL para la búsqueda. */
  private crearFiltroBusqueda(texto: string): string {
    const valor = texto.trim();
    if (!valor) return '';

    const seguro = valor.replace(/\\/g, '\\\\').replace(/"/g, '\\"');
    const numero = Number(valor);

    if (!Number.isNaN(numero)) {
      return `IdFactura == ${numero} || IdPaciente == ${numero} || Estado.Contains("${seguro}")`;
    }
    return `Estado.Contains("${seguro}")`;
  }

  /** Crea una estructura de factura vacía. */
  private crearFacturaVacia(): Factura {
    return {
      idFactura: 0,
      idPaciente: 0,
      fechaFactura: this.obtenerFechaActual(),
      estado: 'Pendiente'
    };
  }

  /** Obtiene la fecha actual para inputs tipo datetime-local. */
  private obtenerFechaActual(): string {
    const fecha = new Date();
    const offset = fecha.getTimezoneOffset();
    return new Date(fecha.getTime() - offset * 60000).toISOString().slice(0, 16);
  }

  /** Formatea una fecha para ajustarla al input. */
  private formatearFechaInput(fecha: string): string {
    return fecha ? fecha.substring(0, 16) : '';
  }
}