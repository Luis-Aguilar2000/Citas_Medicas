import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ConsultoriosService, Consultorio } from '../../../core/services/consultorios';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';

@Component({
  selector: 'app-consultorios',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones
  ],
  templateUrl: './consultorios.html',
  styleUrl: './consultorios.css'
})
export class Consultorios implements OnInit {

  consultorios = signal<Consultorio[]>([]);

  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  consultorioSeleccionado: Consultorio | null = null;
  nuevoConsultorio: Consultorio = this.crearConsultorioVacio();

  constructor(private consultoriosService: ConsultoriosService) {}

  ngOnInit(): void {
    this.cargarConsultorios();
  }

  cargarConsultorios(): void {
    this.consultoriosService.getConsultorios().subscribe({
      next: (data) => {
        console.log('CONSULTORIOS RECIBIDOS:', data);
        this.consultorios.set(data);
      },
      error: (error) => {
        console.error('Error cargando consultorios:', error);
      }
    });
  }

  abrirNuevoConsultorio(): void {
    this.modoFormulario = 'nuevo';
    this.consultorioSeleccionado = null;
    this.nuevoConsultorio = this.crearConsultorioVacio();
    this.mostrarFormulario = true;
  }

  verConsultorio(consultorio: Consultorio): void {
    this.consultoriosService.getConsultorioById(consultorio.idConsultorio).subscribe({
      next: (data) => {
        this.modoFormulario = 'ver';
        this.consultorioSeleccionado = data;
        this.nuevoConsultorio = { ...data };
        this.mostrarFormulario = true;
      },
      error: (error) => {
        console.error('Error obteniendo consultorio:', error);
      }
    });
  }

  editarConsultorio(consultorio: Consultorio): void {
    this.consultoriosService.getConsultorioById(consultorio.idConsultorio).subscribe({
      next: (data) => {
        this.modoFormulario = 'editar';
        this.consultorioSeleccionado = data;
        this.nuevoConsultorio = { ...data };
        this.mostrarFormulario = true;
      },
      error: (error) => {
        console.error('Error obteniendo consultorio:', error);
      }
    });
  }

  eliminarConsultorio(consultorio: Consultorio): void {
    const confirmar = confirm(`¿Desea eliminar el consultorio ${consultorio.nombre}?`);
    if (!confirmar) {
      return;
    }

    this.consultoriosService.deleteConsultorio(consultorio.idConsultorio).subscribe({
      next: () => {
        console.log('Consultorio eliminado correctamente');
        this.cargarConsultorios();
      },
      error: (error) => {
        console.error('Error eliminando consultorio:', error);
      }
    });
  }

  guardarConsultorio(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarConsultorio();
      return;
    }

    if (this.modoFormulario === 'editar') {
      this.actualizarConsultorio();
    }
  }

  agregarConsultorio(): void {
    this.consultoriosService.addConsultorio(this.nuevoConsultorio).subscribe({
      next: () => {
        console.log('Consultorio agregado correctamente');
        this.cerrarFormulario();
        this.cargarConsultorios();
      },
      error: (error) => {
        console.error('Error agregando consultorio:', error);
      }
    });
  }

  actualizarConsultorio(): void {
    this.consultoriosService.updateConsultorio(this.nuevoConsultorio).subscribe({
      next: () => {
        console.log('Consultorio actualizado correctamente');
        this.cerrarFormulario();
        this.cargarConsultorios();
      },
      error: (error) => {
        console.error('Error actualizando consultorio:', error);
      }
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.consultorioSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoConsultorio = this.crearConsultorioVacio();
  }

  private crearConsultorioVacio(): Consultorio {
    return {
      idConsultorio: 0,
      nombre: '',
      numeroConsultorio: '',
      piso: '',
      ubicacion: '',
      descripcion: '',
      estado: true,
      fechaRegistro: new Date().toISOString()
    };
  }

}