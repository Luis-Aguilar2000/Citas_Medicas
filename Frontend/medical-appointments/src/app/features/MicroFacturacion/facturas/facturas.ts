import { Component, OnInit, OnDestroy, signal, effect, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin, of, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { FacturasService, Factura } from '../../../core/services/facturas';
import { FacturaDetallesService, FacturaDetalle } from '../../../core/services/factura-detalles';
import { ServiciosService, Servicio } from '../../../core/services/servicios';
import { MedicamentosService, Medicamento } from '../../../core/services/medicamentos';
import { CatalogoEstudiosService, CatalogoEstudio } from '../../../core/services/catalogo-estudios';
import { PacientesService, Paciente } from '../../../core/services/pacientes';
import { PagosService } from '../../../core/services/pagos';
import { ImpresionFacturaService } from '../../../core/services/impresion-factura';
import { BusquedaService } from '../../../core/services/busqueda';
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
  facturas = signal<Factura[]>([]);
  pacientes = signal<Paciente[]>([]);
  servicios = signal<Servicio[]>([]);
  medicamentos = signal<Medicamento[]>([]);
  catalogoEstudios = signal<CatalogoEstudio[]>([]);
  detallesFactura = signal<FacturaDetalle[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;
  mostrarFormulario = false;
  mostrarModalPago = false;
  cargandoDetalles = false;
  guardando = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
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
    effect(() => {
      this.busquedaService.texto();
      untracked(() => {
        this.paginaActual = 1;
        this.cargarFacturas();
      });
    });
  }

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar facturas...');
    this.cargarPacientes();
    this.cargarServicios();
    this.cargarMedicamentos();
    this.cargarCatalogoEstudios();
  }

  ngOnDestroy(): void {
    this.busquedaService.establecerSugerencias([]);
  }

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
      error: error => {
        console.error('Error cargando facturas:', error);
      }
    });
  }

  cargarPacientes(): void {
    this.pacientesService.getPacientes(1, 100).subscribe({
      next: resultado => this.pacientes.set(resultado.data ?? []),
      error: error => console.error('Error cargando pacientes:', error)
    });
  }

  cargarServicios(): void {
    this.serviciosService.getServicios(1, 100).subscribe({
      next: resultado => this.servicios.set(resultado.data ?? []),
      error: error => console.error('Error cargando servicios:', error)
    });
  }

  cargarMedicamentos(): void {
    this.medicamentosService.getMedicamentos(1, 100, 'Activo == true').subscribe({
      next: resultado => this.medicamentos.set(resultado.data ?? []),
      error: error => console.error('Error cargando medicamentos:', error)
    });
  }

  cargarCatalogoEstudios(): void {
    this.catalogoEstudiosService.getCatalogoEstudios(1, 100, 'Activo == true').subscribe({
      next: resultado => this.catalogoEstudios.set(resultado.data ?? []),
      error: error => console.error('Error cargando estudios:', error)
    });
  }

  obtenerNombrePaciente(idPaciente: number): string {
    const paciente = this.pacientes().find(item => item.idPaciente === idPaciente);
    if (!paciente) return `Paciente #${idPaciente}`;
    return `${paciente.nombres} ${paciente.apellidos}`.trim();
  }

  obtenerClaseEstado(estado: string): string {
    switch (estado.toLowerCase()) {
      case 'pagada': return 'estado-pagada';
      case 'parcial': return 'estado-parcial';
      case 'cancelada': return 'estado-cancelada';
      default: return 'estado-pendiente';
    }
  }

  actualizarDetalles(detalles: FacturaDetalle[]): void {
    this.detallesFactura.set(detalles.map(detalle => ({ ...detalle })));
  }

  calcularTotalFactura(): number {
    return this.detalleManager.calcularTotal(this.detallesFactura());
  }

  abrirNuevaFactura(): void {
    this.modoFormulario = 'nuevo';
    this.facturaSeleccionada = null;
    this.nuevaFactura = this.crearFacturaVacia();
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.mostrarFormulario = true;
  }

  verFactura(factura: Factura): void {
    this.modoFormulario = 'ver';
    this.facturaSeleccionada = factura;
    this.nuevaFactura = { ...factura };
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.mostrarFormulario = true;
    this.cargarDetallesFactura(factura.idFactura);
  }

  editarFactura(factura: Factura): void {
    this.modoFormulario = 'editar';
    this.facturaSeleccionada = factura;
    this.nuevaFactura = { ...factura };
    this.nuevaFactura.fechaFactura = this.formatearFechaInput(factura.fechaFactura);
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.mostrarFormulario = true;
    this.cargarDetallesFactura(factura.idFactura);
  }

  cargarDetallesFactura(idFactura: number): void {
    this.cargandoDetalles = true;
    this.facturaDetallesService.getDetallesFactura(idFactura).subscribe({
      next: resultado => {
        const detalles = resultado.data ?? [];
        this.detallesFactura.set(detalles.map(d => ({ ...d })));
        this.detallesOriginales = detalles.map(d => ({ ...d }));
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

  validarFactura(): boolean {
    if (this.nuevaFactura.idPaciente <= 0) {
      alert('Seleccione un paciente.');
      return false;
    }
    if (!this.nuevaFactura.fechaFactura) {
      alert('Ingrese la fecha de la factura.');
      return false;
    }
    const error = this.detalleManager.validar(this.detallesFactura());
    if (error) {
      alert(error);
      return false;
    }
    return true;
  }

  guardarFactura(): void {
    if (!this.validarFactura()) return;
    if (this.modoFormulario === 'nuevo') {
      this.crearFactura(false);
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarFactura(false);
    }
  }

  pagarFactura(): void {
    if (!this.validarFactura()) return;
    if (this.modoFormulario === 'nuevo') {
      this.crearFactura(true);
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarFactura(true);
      return;
    }
    this.prepararPago({ ...this.nuevaFactura }, this.calcularTotalFactura());
  }

  private crearFactura(abrirPago: boolean): void {
    this.guardando = true;
    const total = this.calcularTotalFactura();
    const factura: Factura = { ...this.nuevaFactura, estado: 'Pendiente' };

    this.facturasService.addFactura(factura).pipe(
      switchMap(facturaCreada => {
        const idFactura = facturaCreada.idFactura;
        const peticiones = this.detallesFactura().map(detalle =>
          this.facturaDetallesService.agregarDetalle({ ...detalle, idDetalle: 0, idFactura })
        );

        if (peticiones.length === 0) {
          return of({ factura: facturaCreada, total });
        }

        return forkJoin(peticiones).pipe(
          map(() => ({ factura: facturaCreada, total }))
        );
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

  private actualizarFactura(abrirPago: boolean): void {
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
        alert('No se pudo actualizar la factura.');
      }
    });
  }

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

    const idsActuales = new Set(
      actuales.filter(d => d.idDetalle > 0).map(d => d.idDetalle)
    );

    for (const original of this.detallesOriginales) {
      if (original.idDetalle > 0 && !idsActuales.has(original.idDetalle)) {
        operaciones.push(this.facturaDetallesService.eliminarDetalle(original.idDetalle));
      }
    }

    if (operaciones.length === 0) {
      return of([]);
    }

    return forkJoin(operaciones);
  }

  private prepararPago(factura: Factura, total: number): void {
    this.facturaPago = { ...factura };
    this.totalFacturaPago = total;
    this.mostrarFormulario = false;
    this.mostrarModalPago = true;
  }

  cerrarModalPago(): void {
    this.mostrarModalPago = false;
    this.facturaPago = null;
    this.totalFacturaPago = 0;
    this.limpiarFormulario();
    this.cargarFacturas();
  }

  pagoRegistrado(): void {
    this.cargarFacturas();
  }

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

  eliminarFactura(factura: Factura): void {
    const confirmar = confirm(`¿Desea eliminar la factura #${factura.idFactura}?`);
    if (!confirmar) return;

    this.facturasService.deleteFactura(factura.idFactura).subscribe({
      next: () => {
        if (this.facturas().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarFacturas();
      },
      error: error => {
        console.error('Error eliminando factura:', error);
        alert(error.error?.mensaje ?? 'No se pudo eliminar la factura.');
      }
    });
  }

  cerrarFormulario(): void {
    if (this.guardando) return;
    this.mostrarFormulario = false;
    this.limpiarFormulario();
  }

  private limpiarFormulario(): void {
    this.modoFormulario = 'nuevo';
    this.facturaSeleccionada = null;
    this.detallesFactura.set([]);
    this.detallesOriginales = [];
    this.cargandoDetalles = false;
    this.guardando = false;
    this.nuevaFactura = this.crearFacturaVacia();
  }

  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarFacturas();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = Number(tamano);
    this.paginaActual = 1;
    this.cargarFacturas();
  }

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

  private crearFacturaVacia(): Factura {
    return {
      idFactura: 0,
      idPaciente: 0,
      fechaFactura: this.obtenerFechaActual(),
      estado: 'Pendiente'
    };
  }

  private obtenerFechaActual(): string {
    const fecha = new Date();
    const offset = fecha.getTimezoneOffset();
    return new Date(fecha.getTime() - offset * 60000).toISOString().slice(0, 16);
  }

  private formatearFechaInput(fecha: string): string {
    if (!fecha) return '';
    return fecha.substring(0, 16);
  }
}