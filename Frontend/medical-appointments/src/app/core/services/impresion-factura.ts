import { Injectable } from '@angular/core';

import {
  Factura
} from './facturas';

import {
  FacturaDetalle
} from './factura-detalles';

import {
  Pago
} from './pagos';

import {
  Paciente
} from './pacientes';

@Injectable({
  providedIn: 'root'
})
export class ImpresionFacturaService {

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

    const paciente = pacientes.find(
      x => x.idPaciente === factura.idPaciente
    );

    const nombrePaciente = paciente
      ? `${paciente.nombres} ${paciente.apellidos}`.trim()
      : `Paciente #${factura.idPaciente}`;

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

    const saldo = Math.max(
      total - totalPagado,
      0
    );

    const filasDetalle = detalles
      .map(detalle => {

        const subtotal =
          Number(detalle.cantidad) *
          Number(detalle.precioUnitario);

        return `
          <tr>

            <td>

              <div class="descripcion">
                ${this.escaparHtml(detalle.descripcion)}
              </div>

              <div class="tipo">
                ${this.escaparHtml(detalle.tipoItem)}
              </div>

            </td>

            <td class="centro">
              ${Number(detalle.cantidad)}
            </td>

            <td class="derecha">
              $${Number(detalle.precioUnitario).toFixed(2)}
            </td>

            <td class="derecha">
              $${subtotal.toFixed(2)}
            </td>

          </tr>
        `;
      })
      .join('');

    const ventana = window.open(
      '',
      '_blank',
      'width=900,height=750'
    );

    if (!ventana) {
      alert(
        'El navegador bloqueó la ventana de impresión.'
      );
      return;
    }

    ventana.document.open();

    ventana.document.write(`
      <!DOCTYPE html>

      <html lang="es">

      <head>

        <meta charset="UTF-8">

        <meta
          name="viewport"
          content="width=device-width, initial-scale=1.0"
        >

        <title>
          Factura #${factura.idFactura}
        </title>

        <style>

          * {
            box-sizing: border-box;
          }

          body {
            margin: 0;
            padding: 35px;
            background: #ffffff;
            color: #222222;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 13px;
          }

          .factura {
            width: 100%;
            max-width: 850px;
            margin: 0 auto;
          }

          .encabezado {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            gap: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid #222222;
          }

          .empresa h1 {
            margin: 0 0 6px;
            font-size: 23px;
            font-weight: 600;
          }

          .empresa p {
            margin: 3px 0;
            color: #666666;
            font-size: 12px;
          }

          .datos-factura {
            text-align: right;
          }

          .datos-factura h2 {
            margin: 0 0 8px;
            font-size: 22px;
          }

          .datos-factura p {
            margin: 4px 0;
          }

          .estado {
            display: inline-block;
            margin-top: 7px;
            padding: 5px 10px;
            border: 1px solid #333333;
            border-radius: 4px;
            font-size: 10px;
            font-weight: bold;
            text-transform: uppercase;
          }

          .paciente {
            margin-top: 24px;
            padding: 14px 16px;
            background: #f7f7f7;
            border: 1px solid #dddddd;
            border-radius: 5px;
          }

          .paciente-titulo {
            display: block;
            margin-bottom: 5px;
            color: #777777;
            font-size: 10px;
            font-weight: bold;
          }

          .paciente-nombre {
            font-size: 14px;
            font-weight: bold;
          }

          table {
            width: 100%;
            margin-top: 25px;
            border-collapse: collapse;
          }

          th {
            padding: 10px;
            border-bottom: 2px solid #222222;
            font-size: 10px;
            font-weight: bold;
            text-align: left;
            text-transform: uppercase;
          }

          td {
            padding: 11px 10px;
            border-bottom: 1px solid #dddddd;
            vertical-align: top;
          }

          .descripcion {
            font-weight: bold;
          }

          .tipo {
            margin-top: 4px;
            color: #888888;
            font-size: 10px;
          }

          .centro {
            text-align: center;
          }

          .derecha {
            text-align: right;
          }

          .totales {
            width: 320px;
            margin: 25px 0 0 auto;
          }

          .total-linea {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 7px 0;
          }

          .total-principal {
            margin-bottom: 3px;
            padding-top: 12px;
            border-top: 2px solid #222222;
            font-size: 17px;
            font-weight: bold;
          }

          .pagado {
            color: #047857;
          }

          .saldo {
            font-weight: bold;
          }

          .saldo-cero {
            color: #047857;
          }

          .pie {
            margin-top: 55px;
            padding-top: 15px;
            border-top: 1px solid #dddddd;
            color: #777777;
            font-size: 10px;
            text-align: center;
          }

          @media print {

            body {
              padding: 0;
            }

            @page {
              margin: 15mm;
            }
          }

        </style>

      </head>

      <body>

        <div class="factura">

          <div class="encabezado">

            <div class="empresa">

              <h1>
                Sistema de Citas Médicas
              </h1>

              <p>
                Comprobante de servicios médicos
              </p>

            </div>

            <div class="datos-factura">

              <h2>
                FACTURA
              </h2>

              <p>
                <strong>No.</strong>
                ${String(factura.idFactura).padStart(6, '0')}
              </p>

              <p>
                <strong>Fecha:</strong>
                ${this.formatearFecha(factura.fechaFactura)}
              </p>

              <span class="estado">
                ${this.escaparHtml(factura.estado)}
              </span>

            </div>

          </div>

          <div class="paciente">

            <span class="paciente-titulo">
              PACIENTE
            </span>

            <span class="paciente-nombre">
              ${this.escaparHtml(nombrePaciente)}
            </span>

          </div>

          <table>

            <thead>

              <tr>

                <th>
                  Descripción
                </th>

                <th class="centro">
                  Cantidad
                </th>

                <th class="derecha">
                  Precio
                </th>

                <th class="derecha">
                  Subtotal
                </th>

              </tr>

            </thead>

            <tbody>
              ${filasDetalle}
            </tbody>

          </table>

          <div class="totales">

            <div class="total-linea total-principal">

              <span>
                Total
              </span>

              <span>
                $${total.toFixed(2)}
              </span>

            </div>

            <div class="total-linea pagado">

              <span>
                Pagado
              </span>

              <span>
                $${totalPagado.toFixed(2)}
              </span>

            </div>

            <div
              class="
                total-linea
                saldo
                ${saldo <= 0 ? 'saldo-cero' : ''}
              "
            >

              <span>
                Saldo pendiente
              </span>

              <span>
                $${saldo.toFixed(2)}
              </span>

            </div>

          </div>

          <div class="pie">
            Gracias por su preferencia.
          </div>

        </div>

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

  private escaparHtml(
    valor: string | null | undefined
  ): string {

    if (!valor) {
      return '';
    }

    return valor
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#039;');
  }

  private formatearFecha(
    fecha: string
  ): string {

    if (!fecha) {
      return '';
    }

    const fechaNormalizada =
      fecha.includes('T')
        ? fecha
        : `${fecha}T00:00:00`;

    const valor =
      new Date(fechaNormalizada);

    if (
      Number.isNaN(valor.getTime())
    ) {
      return fecha;
    }

    return valor.toLocaleDateString(
      'es-SV',
      {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      }
    );
  }
}