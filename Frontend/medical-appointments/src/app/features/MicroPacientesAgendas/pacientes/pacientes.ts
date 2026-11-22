import { Component, OnInit, OnDestroy, signal, effect, untracked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PacientesService, Paciente } from '../../../core/services/MicroPacientesAgendas/pacientes';
import { BusquedaService, SugerenciaBusqueda } from '../../../core/services/Compartidos/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-pacientes',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonesAcciones, Paginacion],
  templateUrl: './pacientes.html',
  styleUrl: './pacientes.css'
})
export class Pacientes implements OnInit, OnDestroy {
  // DATOS Y PAGINACIÃ“N
  pacientes = signal<Paciente[]>([]);
  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  // TEMPORIZADOR DE PREDICCIONES
  private temporizadorPredicciones: ReturnType<typeof setTimeout> | null = null;

  // FORMULARIO Y ESTADOS
  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  pacienteSeleccionado: Paciente | null = null;
  nuevoPaciente: Paciente = this.crearPacienteVacio();

  constructor(
    private pacientesService: PacientesService,
    private busquedaService: BusquedaService
  ) {
    // BÃšSQUEDA CONFIRMADA
    effect(() => {
      this.busquedaService.texto();

      untracked(() => {
        this.paginaActual = 1;
        this.cargarPacientes();
      });
    });

    // PREDICCIONES CON DEBOUNCE
    effect(() => {
      const texto = this.busquedaService.entrada();

      if (this.temporizadorPredicciones) {
        clearTimeout(this.temporizadorPredicciones);
      }

      if (texto.trim().length < 2) {
        this.busquedaService.establecerSugerencias([]);
        return;
      }

      this.temporizadorPredicciones = setTimeout(() => {
        this.cargarPredicciones(texto);
      }, 350);
    });
  }

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar pacientes...');
  }

  ngOnDestroy(): void {
    if (this.temporizadorPredicciones) {
      clearTimeout(this.temporizadorPredicciones);
    }
    this.busquedaService.establecerSugerencias([]);
  }

  // CARGAR PACIENTES PAGINADOS
  cargarPacientes(): void {
    const textoBusqueda = this.busquedaService.texto();
    const filtroSeleccionado = this.busquedaService.filtroSeleccionado();
    const filtro = filtroSeleccionado !== '' ? filtroSeleccionado : this.crearFiltroBusqueda(textoBusqueda);

    this.pacientesService.getPacientes(this.paginaActual, this.tamanoPagina, filtro).subscribe({
      next: (resultado) => {
        this.pacientes.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: (error) => console.error('Error cargando pacientes:', error)
    });
  }

  // CARGAR PREDICCIONES DE BÃšSQUEDA
  private cargarPredicciones(texto: string): void {
    const filtro = this.crearFiltroBusqueda(texto);

    if (filtro === '') {
      this.busquedaService.establecerSugerencias([]);
      return;
    }

    this.pacientesService.getPacientes(1, 5, filtro).subscribe({
      next: (resultado) => {
        if (this.busquedaService.entrada().trim() !== texto.trim()) {
          return;
        }

        const sugerencias: SugerenciaBusqueda[] = resultado.data.map((paciente) => ({
          texto: `${paciente.nombres} ${paciente.apellidos}`.trim(),
          descripcion: paciente.dui,
          filtro: `IdPaciente == ${paciente.idPaciente}`
        }));

        this.busquedaService.establecerSugerencias(sugerencias);
      },
      error: (error) => {
        console.error('Error cargando predicciones:', error);
        this.busquedaService.establecerSugerencias([]);
      }
    });
  }

  // CREAR FILTRO DE BÃšSQUEDA DINÃMICO
  private crearFiltroBusqueda(texto: string): string {
    const valor = texto.trim();
    if (valor === '') return '';

    const palabras = valor.split(/\s+/).filter(palabra => palabra.trim() !== '');

    const filtros = palabras.map(palabra => {
      const palabraSegura = palabra.replace(/\\/g, '\\\\').replace(/"/g, '\\"');
      return `(Nombres.Contains("${palabraSegura}") || Apellidos.Contains("${palabraSegura}") || DUI.Contains("${palabraSegura}") || Telefono.Contains("${palabraSegura}") || Correo.Contains("${palabraSegura}"))`;
    });

    return filtros.join(' && ');
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

  eliminarPaciente(paciente: Paciente): void {
    const nombreCompleto = `${paciente.nombres} ${paciente.apellidos}`;
    if (!confirm(`Â¿Desea eliminar al paciente ${nombreCompleto}?`)) return;

    this.pacientesService.deletePaciente(paciente.idPaciente).subscribe({
      next: () => {
        if (this.pacientes().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarPacientes();
      },
      error: (error) => console.error('Error eliminando paciente:', error)
    });
  }

  guardarPaciente(): void {
    if (this.modoFormulario === 'nuevo') {
      this.agregarPaciente();
      return;
    }
    if (this.modoFormulario === 'editar') {
      this.actualizarPaciente();
    }
  }

  private agregarPaciente(): void {
    this.pacientesService.addPaciente(this.nuevoPaciente).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.paginaActual = 1;
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

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.pacienteSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoPaciente = this.crearPacienteVacio();
  }

  // PAGINACIÃ“N
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) return;
    this.paginaActual = pagina;
    this.cargarPacientes();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarPacientes();
  }

  // MODELO VACÃO
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
