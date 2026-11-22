import { Injectable } from '@angular/core';
import { Factura } from './facturas';
import { FacturaDetalle } from './factura-detalles';
import { Pago } from './pagos';
import { Paciente } from '../MicroPacientesAgendas/pacientes';

@Injectable({
  providedIn: 'root'
})
export class ImpresionFacturaService {

  // Datos predeterminados de la empresa para el membrete
  private readonly empresa = {
    nombre: 'CLÍNICA MÉDICA',
    lema: 'Sistema de Citas Médicas',
    direccion: 'San Salvador, El Salvador',
    telefono: '+503 0000-0000',
    correo: 'correo@clinica.com'
  };

  /**
   * Genera y abre una ventana emergente con el diseño optimizado de la factura para impresión.
   */
  imprimir(
    factura: Factura,
    detalles: FacturaDetalle[],
    pagos: Pago[],
    pacientes: Paciente[]
  ): void {
    if (!factura || factura.idFactura <= 0) {
      alert('No hay una factura válida para imprimir.');
      return;
    }

    // Búsqueda y formato de datos del paciente
    const paciente = pacientes.find(
      x => x.idPaciente === factura.idPaciente
    );

    const nombrePaciente = paciente
      ? `${paciente.nombres} ${paciente.apellidos}`.trim()
      : `Paciente #${factura.idPaciente}`;

    // Cálculos financieros (Totales, Pagos y Saldos)
    const total = detalles.reduce(
      (suma, detalle) =>
        suma +
        (
          Number(detalle.cantidad) *
          Number(detalle.precioUnitario)
        ),
      0
    );

    const totalPagado = pagos.reduce(
      (suma, pago) =>
        suma + Number(pago.monto),
      0
    );

    const estadoFactura =
      factura.estado?.trim().toLowerCase() ?? '';

    const facturaPagada =
      estadoFactura === 'pagada';

    const totalPagadoMostrar =
      facturaPagada
        ? total
        : totalPagado;

    const saldo =
      facturaPagada
        ? 0
        : Math.max(
            total - totalPagado,
            0
          );

    // Generación dinámica de filas para el detalle de la factura
    const filasDetalle = detalles
      .map(detalle => {
        const cantidad = Number(detalle.cantidad);
        const precio = Number(detalle.precioUnitario);
        const subtotal = cantidad * precio;

        return `
          <tr class="fila-producto">
            <td class="descripcion-celda">
              <div class="descripcion-principal">
                ${this.escaparHtml(detalle.descripcion)}
              </div>
              <div class="descripcion-secundaria">
                ${this.escaparHtml(detalle.tipoItem)}
              </div>
            </td>
            <td class="cantidad-celda">
              ${cantidad}
            </td>
            <td class="precio-celda">
              ${this.moneda(precio)}
            </td>
            <td class="total-celda">
              ${this.moneda(subtotal)}
            </td>
          </tr>
        `;
      })
      .join('');

    // Apertura de ventana de impresión
    const ventana = window.open(
      '',
      '_blank',
      'width=1000,height=850'
    );

    if (!ventana) {
      alert('El navegador bloqueó la ventana de impresión.');
      return;
    }

    ventana.document.open();

    ventana.document.write(`
      <!DOCTYPE html>
      <html lang="es">
      <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>
          Factura ${String(factura.idFactura).padStart(6, '0')}
        </title>
        <style>
          * {
            box-sizing: border-box;
          }
          html,
          body {
            margin: 0;
            padding: 0;
          }
          body {
            background: #ffffff;
            color: #111111;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
          }
          .pagina {
            width: 100%;
            max-width: 850px;
            min-height: 1000px;
            margin: 0 auto;
            padding: 32px 32px 25px;
            background: #ffffff;
          }

          /* ENCABEZADO */
          .encabezado {
            display: grid;
            grid-template-columns: minmax(0, 1fr) 310px;
            gap: 50px;
            min-height: 145px;
          }
          .empresa {
            padding-top: 4px;
          }
          .nombre-empresa {
            margin: 0 0 3px;
            color: #111111;
            font-size: 20px;
            font-weight: 700;
          }
          .lema {
            margin: 0 0 20px;
            color: #555555;
            font-size: 10px;
          }
          .datos-empresa {
            color: #333333;
            font-size: 10px;
            line-height: 1.5;
          }
          .datos-empresa p {
            margin: 0;
          }

          /* DATOS DE FACTURA */
          .factura-encabezado {
            text-align: right;
          }
          .titulo-factura {
            margin: 0 0 15px;
            color: #6c5ce7;
            font-size: 36px;
            font-weight: 500;
            line-height: 1;
          }
          .datos-factura {
            width: 100%;
            margin-left: auto;
          }
          .dato-factura {
            display: grid;
            grid-template-columns: 1fr 120px;
            align-items: center;
            margin-bottom: 3px;
          }
          .dato-factura .etiqueta {
            padding-right: 8px;
            color: #111111;
            font-size: 10px;
            font-weight: 700;
            text-align: right;
          }
          .dato-factura .valor {
            min-height: 23px;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 3px 6px;
            border: 1px solid #333333;
            background: #fafafa;
            font-size: 10px;
          }

          /* CLIENTE */
          .cliente {
            width: 55%;
            margin-top: 10px;
            margin-bottom: 25px;
          }
          .barra-seccion {
            width: 100%;
            padding: 3px 6px;
            background: #51469b;
            color: #ffffff;
            font-size: 10px;
            font-weight: 700;
            text-transform: uppercase;
          }
          .datos-cliente {
            padding: 5px 3px 0;
          }
          .cliente-nombre {
            margin: 0 0 4px;
            color: #111111;
            font-size: 12px;
            font-weight: 700;
          }
          .cliente-dato {
            margin: 2px 0;
            color: #333333;
            font-size: 10px;
          }

          /* TABLA */
          .detalle-contenedor {
            width: 100%;
            border: 1px solid #333333;
          }
          .tabla-detalle {
            width: 100%;
            border-collapse: collapse;
            table-layout: fixed;
          }
          .tabla-detalle col.descripcion-col {
            width: 47%;
          }
          .tabla-detalle col.cantidad-col {
            width: 13%;
          }
          .tabla-detalle col.precio-col {
            width: 20%;
          }
          .tabla-detalle col.total-col {
            width: 20%;
          }
          .tabla-detalle thead {
            background: #51469b;
            color: #ffffff;
          }
          .tabla-detalle th {
            height: 25px;
            padding: 5px 8px;
            border-right: 1px solid rgba(255, 255, 255, 0.35);
            color: #ffffff;
            font-size: 9px;
            font-weight: 700;
            text-align: center;
            text-transform: uppercase;
          }
          .tabla-detalle th:first-child {
            text-align: left;
          }
          .tabla-detalle th:last-child {
            border-right: none;
          }
          .tabla-detalle tbody {
            height: 270px;
          }
          .tabla-detalle td {
            padding: 8px;
            border-right: 1px solid #999999;
            vertical-align: top;
          }
          .tabla-detalle td:last-child {
            border-right: none;
          }
          .fila-producto {
            height: 48px;
          }
          .fila-producto td {
            border-bottom: 1px solid #e2e2e2;
          }
          .descripcion-principal {
            color: #111111;
            font-size: 10px;
            font-weight: 600;
          }
          .descripcion-secundaria {
            margin-top: 3px;
            color: #777777;
            font-size: 8px;
            text-transform: uppercase;
          }
          .cantidad-celda {
            text-align: center;
          }
          .precio-celda,
          .total-celda {
            text-align: right;
          }
          .total-celda {
            font-weight: 700;
          }
          .espacio-detalle {
            height: 200px;
          }
          .espacio-detalle td {
            border-right: 1px solid #999999;
          }
          .espacio-detalle td:last-child {
            border-right: none;
          }

          /* ZONA INFERIOR */
          .zona-inferior {
            display: grid;
            grid-template-columns: minmax(0, 1fr) 300px;
            gap: 40px;
            margin-top: 20px;
          }

          /* INFORMACIÓN */
          .comentarios {
            min-height: 130px;
            border: 1px solid #777777;
          }
          .comentarios-titulo {
            padding: 4px 6px;
            background: #d6d6d6;
            color: #111111;
            font-size: 9px;
            font-weight: 700;
            text-transform: uppercase;
          }
          .comentarios-contenido {
            padding: 7px;
            color: #333333;
            font-size: 9px;
            line-height: 1.6;
          }
          .comentarios-contenido p {
            margin: 0 0 3px;
          }
          .estado-pagado {
            color: #047857;
            font-weight: 700;
          }
          .estado-pendiente {
            color: #dc2626;
            font-weight: 700;
          }

          /* TOTALES */
          .totales {
            width: 100%;
          }
          .total-fila {
            display: grid;
            grid-template-columns: 1fr 115px;
            min-height: 29px;
            border-bottom: 1px solid #cccccc;
          }
          .total-etiqueta {
            display: flex;
            align-items: center;
            padding: 5px 8px;
            color: #333333;
            font-size: 9px;
            font-weight: 600;
            text-transform: uppercase;
          }
          .total-valor {
            display: flex;
            align-items: center;
            justify-content: flex-end;
            padding: 5px 8px;
            border-left: 1px solid #cccccc;
            background: #f4f4f4;
            color: #111111;
            font-size: 10px;
            font-weight: 600;
          }
          .pagado .total-valor {
            color: #047857;
          }
          .saldo .total-valor {
            color: #dc2626;
          }
          .total-final {
            margin-top: 2px;
            border-top: 2px solid #333333;
            border-bottom: 2px solid #333333;
          }
          .total-final .total-etiqueta {
            font-size: 11px;
            font-weight: 800;
          }
          .total-final .total-valor {
            background: #51469b;
            color: #ffffff;
            font-size: 12px;
            font-weight: 800;
          }

          /* ESTADO DE PAGO */
          .pago-info {
            margin-top: 20px;
            color: #333333;
            font-size: 9px;
            line-height: 1.5;
            text-align: right;
          }
          .pago-info strong {
            display: block;
            margin-top: 2px;
            font-size: 11px;
          }
          .pago-completo {
            color: #047857;
          }
          .pago-pendiente {
            color: #dc2626;
          }

          /* PIE */
          .pie {
            margin-top: 45px;
            text-align: center;
          }
          .contacto-pie {
            color: #444444;
            font-size: 9px;
            line-height: 1.5;
          }
          .gracias {
            margin-top: 22px;
            color: #111111;
            font-size: 13px;
            font-style: italic;
            font-weight: 700;
          }

          /* IMPRESIÓN */
          @media print {
            body {
              background: #ffffff;
            }
            .pagina {
              max-width: none;
              min-height: auto;
              padding: 0;
            }
            .detalle-contenedor,
            .zona-inferior,
            .comentarios,
            .totales {
              page-break-inside: avoid;
            }
            @page {
              size: letter;
              margin: 12mm;
            }
          }
        </style>
      </head>
      <body>
        <main class="pagina">
          <!-- ENCABEZADO -->
          <header class="encabezado">
            <section class="empresa">
              <h1 class="nombre-empresa">
                ${this.escaparHtml(this.empresa.nombre)}
              </h1>
              <p class="lema">
                ${this.escaparHtml(this.empresa.lema)}
              </p>
              <div class="datos-empresa">
                <p>${this.escaparHtml(this.empresa.direccion)}</p>
                <p>Teléfono: ${this.escaparHtml(this.empresa.telefono)}</p>
                <p>Correo: ${this.escaparHtml(this.empresa.correo)}</p>
              </div>
            </section>
            <section class="factura-encabezado">
              <h2 class="titulo-factura">FACTURA</h2>
              <div class="datos-factura">
                <div class="dato-factura">
                  <div class="etiqueta">Fecha:</div>
                  <div class="valor">
                    ${this.formatearFecha(factura.fechaFactura)}
                  </div>
                </div>
                <div class="dato-factura">
                  <div class="etiqueta">N.º de Factura:</div>
                  <div class="valor">
                    ${String(factura.idFactura).padStart(6, '0')}
                  </div>
                </div>
                <div class="dato-factura">
                  <div class="etiqueta">Estado:</div>
                  <div class="valor">
                    ${this.escaparHtml(factura.estado)}
                  </div>
                </div>
              </div>
            </section>
          </header>

          <!-- FACTURAR A -->
          <section class="cliente">
            <div class="barra-seccion">Facturar a:</div>
            <div class="datos-cliente">
              <p class="cliente-nombre">
                ${this.escaparHtml(nombrePaciente)}
              </p>
              ${
                paciente?.direccion
                  ? `<p class="cliente-dato">${this.escaparHtml(paciente.direccion)}</p>`
                  : ''
              }
              ${
                paciente?.dui
                  ? `<p class="cliente-dato">DUI: ${this.escaparHtml(paciente.dui)}</p>`
                  : ''
              }
              ${
                paciente?.telefono
                  ? `<p class="cliente-dato">Teléfono: ${this.escaparHtml(paciente.telefono)}</p>`
                  : ''
              }
              ${
                paciente?.correo
                  ? `<p class="cliente-dato">Correo: ${this.escaparHtml(paciente.correo)}</p>`
                  : ''
              }
            </div>
          </section>

          <!-- DETALLE -->
          <section class="detalle-contenedor">
            <table class="tabla-detalle">
              <colgroup>
                <col class="descripcion-col">
                <col class="cantidad-col">
                <col class="precio-col">
                <col class="total-col">
              </colgroup>
              <thead>
                <tr>
                  <th>Descripción</th>
                  <th>Cantidad</th>
                  <th>Precio unitario</th>
                  <th>Total</th>
                </tr>
              </thead>
              <tbody>
                ${
                  filasDetalle ||
                  `
                    <tr class="fila-producto">
                      <td>Sin detalles</td>
                      <td></td><td></td><td></td>
                    </tr>
                  `
                }
                <tr class="espacio-detalle">
                  <td></td><td></td><td></td><td></td>
                </tr>
              </tbody>
            </table>
          </section>

          <!-- ZONA INFERIOR -->
          <section class="zona-inferior">
            <!-- INFORMACIÓN -->
            <div class="comentarios">
              <div class="comentarios-titulo">Información</div>
              <div class="comentarios-contenido">
                <p>1. Conserve este documento como comprobante.</p>
                ${
                  facturaPagada
                    ? `
                      <p>2. Estado: <span class="estado-pagado">PAGADA</span></p>
                      <p>3. Esta factura se encuentra pagada completamente.</p>
                    `
                    : `
                      <p>2. Estado: <span class="estado-pendiente">${this.escaparHtml(factura.estado)}</span></p>
                      <p>3. Esta factura posee un saldo pendiente de <strong>${this.moneda(saldo)}</strong>.</p>
                    `
                }
              </div>
            </div>

            <!-- TOTALES -->
            <div>
              <div class="totales">
                <div class="total-fila">
                  <div class="total-etiqueta">Subtotal</div>
                  <div class="total-valor">${this.moneda(total)}</div>
                </div>
                <div class="total-fila pagado">
                  <div class="total-etiqueta">Total pagado</div>
                  <div class="total-valor">${this.moneda(totalPagadoMostrar)}</div>
                </div>
                ${
                  !facturaPagada && saldo > 0
                    ? `
                      <div class="total-fila saldo">
                        <div class="total-etiqueta">Saldo pendiente</div>
                        <div class="total-valor">${this.moneda(saldo)}</div>
                      </div>
                    `
                    : ''
                }
                <div class="total-fila total-final">
                  <div class="total-etiqueta">Total</div>
                  <div class="total-valor">${this.moneda(total)}</div>
                </div>
              </div>

              <!-- ESTADO DE PAGO -->
              <div class="pago-info">
                Estado de pago
                ${
                  facturaPagada
                    ? '<strong class="pago-completo">PAGADA</strong>'
                    : `<strong class="pago-pendiente">${this.escaparHtml(factura.estado)}</strong>`
                }
              </div>
            </div>
          </section>

          <!-- PIE -->
          <footer class="pie">
            <div class="contacto-pie">
              Si tiene alguna pregunta sobre esta factura, póngase en contacto con nosotros.
              <br>
              ${this.escaparHtml(this.empresa.telefono)} · ${this.escaparHtml(this.empresa.correo)}
            </div>
            <div class="gracias">¡Gracias por su preferencia!</div>
          </footer>
        </main>

        <script>
          window.addEventListener('load', function () {
            setTimeout(function () {
              window.focus();
              window.print();
            }, 250);
          });
        </script>
      </body>
      </html>
    `);

    ventana.document.close();
  }

  /**
   * Formatea un valor numérico a moneda local (USD / El Salvador).
   */
  private moneda(valor: number): string {
    return new Intl.NumberFormat('es-SV', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(Number(valor));
  }

  /**
   * Sanitiza textos para evitar inyección de HTML en la ventana de impresión.
   */
  private escaparHtml(valor: string | null | undefined): string {
    if (valor === null || valor === undefined) {
      return '';
    }
    return String(valor)
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#039;');
  }

  /**
   * Normaliza y formatea fechas para el reporte de factura.
   */
  private formatearFecha(fecha: string): string {
    if (!fecha) {
      return '';
    }

    const fechaNormalizada = fecha.includes('T') ? fecha : `${fecha}T00:00:00`;
    const valor = new Date(fechaNormalizada);

    if (Number.isNaN(valor.getTime())) {
      return fecha;
    }

    return valor.toLocaleDateString('es-SV', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  }
}