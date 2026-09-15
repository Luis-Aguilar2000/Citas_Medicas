import {
  Component,
  EventEmitter,
  Input,
  Output
} from '@angular/core';

import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-botones-acciones',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './botones-acciones.html',
  styleUrl: './botones-acciones.css'
})
export class BotonesAcciones {

  /* =========================================
     CONFIGURACIÓN
  ========================================= */

  @Input() mostrarVer = true;

  @Input() mostrarEditar = true;

  @Input() mostrarEliminar = true;


  /* =========================================
     EVENTOS
  ========================================= */

  @Output() ver = new EventEmitter<void>();

  @Output() editar = new EventEmitter<void>();

  @Output() eliminar = new EventEmitter<void>();


  /* =========================================
     VER
  ========================================= */

  onVer(): void {

    this.ver.emit();

  }


  /* =========================================
     EDITAR
  ========================================= */

  onEditar(): void {

    this.editar.emit();

  }


  /* =========================================
     ELIMINAR
  ========================================= */

  onEliminar(): void {

    this.eliminar.emit();

  }

}