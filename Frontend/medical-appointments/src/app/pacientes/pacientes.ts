import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PacientesService, Paciente } from '../services/pacientes';
import { BotonesAcciones } from '../botones-acciones/botones-acciones';

@Component({
  selector: 'app-pacientes',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonesAcciones],
  templateUrl: './pacientes.html',
  styleUrl: './pacientes.css'
})
export class Pacientes implements OnInit {
  
  // ESTADOS Y DATOS
  pacientes = signal<Paciente[]>([]);
  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  pacienteSeleccionado: Paciente | null = null;
  nuevoPaciente: Paciente = this.crearPacienteVacio();

  constructor(private pacientesService: PacientesService) {}

  ngOnInit(): void {
    this.cargarPacientes();
  }

  // CARGAR LISTADO
  cargarPacientes(): void {
    this.pacientesService.getPacientes().subscribe({
      next: (data) => this.pacientes.set(data),
      error: (error) => console.error('Error cargando pacientes:', error)
    });
  }

  // ACCIONES DEL MODAL
  abrirNuevoPaciente(): void {
    this.modoFormulario = 'nuevo';
    this.pacienteSeleccionado = null;
    this.nuevoPaciente = this.crearPacienteVacio();
    this.mostrarFormulario = true;
  }

  verPaciente(paciente: Paciente): void {
    this.modoFormulario = 'ver';
    this.pacienteSeleccionado = paciente;
    this.nuevoPaciente = { ...paciente };
    this.mostrarFormulario = true;
  }

  editarPaciente(paciente: Paciente): void {
    this.modoFormulario = 'editar';
    this.pacienteSeleccionado = paciente;
    this.nuevoPaciente = { ...paciente };
    this.mostrarFormulario = true;
  }

  // ELIMINAR
  eliminarPaciente(paciente: Paciente): void {
    const nombreCompleto = `${paciente.nombres} ${paciente.apellidos}`;
    if (!confirm(`¿Desea eliminar al paciente ${nombreCompleto}?`)) return;

    this.pacientesService.deletePaciente(paciente.idPaciente).subscribe({
      next: () => this.cargarPacientes(),
      error: (error) => console.error('Error eliminando paciente:', error)
    });
  }

  // GUARDAR (CREAR / EDITAR)
  guardarPaciente(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarPaciente();
    } else if (this.modoFormulario === 'editar') {
      this.actualizarPaciente();
    }
  }

  private agregarPaciente(): void {
    this.pacientesService.addPaciente(this.nuevoPaciente).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarPacientes();
      },
      error: (error) => console.error('Error agregando paciente:', error)
    });
  }

  private actualizarPaciente(): void {
    this.pacientesService.updatePaciente(this.nuevoPaciente).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarPacientes();
      },
      error: (error) => console.error('Error actualizando paciente:', error)
    });
  }

  // CERRAR Y LIMPIAR
  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.pacienteSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoPaciente = this.crearPacienteVacio();
  }

  private crearPacienteVacio(): Paciente {
    return {
      idPaciente: 0,
      nombres: '',
      apellidos: '',
      fechaNacimiento: '',
      sexo: '',
      dui: '',
      telefono: '',
      correo: '',
      direccion: '',
      fechaRegistro: new Date().toISOString()
    };
  }
}