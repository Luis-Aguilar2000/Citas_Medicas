import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CitasService, Cita } from '../../../core/services/MicroPacientesAgendas/citas';
import { PacientesService, Paciente } from '../../../core/services/MicroPacientesAgendas/pacientes';
import { ConsultoriosService, Consultorio } from '../../../core/services/MicroPacientesAgendas/consultorios';
import { EstadoCitasService, EstadoCita } from '../../../core/services/MicroPacientesAgendas/estado-citas';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';
import { BusquedaService } from '../../../core/services/Compartidos/busqueda';

@Component({
  selector: 'app-agenda-citas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],
  templateUrl: './agenda-citas.html',
  styleUrl: './agenda-citas.css'
})
export class AgendaCitas implements OnInit {
  citas = signal<Cita[]>([]);
  pacientes = signal<Paciente[]>([]);
  consultorios = signal<Consultorio[]>([]);
  estadosCitas = signal<EstadoCita[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  mostrarFormulario = false;
  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';
  citaSeleccionada: Cita | null = null;
  nuevaCita: Cita = this.crearCitaVacia();

  constructor(
    private citasService: CitasService,
    private pacientesService: PacientesService,
    private consultoriosService: ConsultoriosService,
    private estadoCitasService: EstadoCitasService,
    private busquedaService: BusquedaService
  ) {}

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar citas...');
    this.cargarCitas();
    this.cargarPacientes();
    this.cargarConsultorios();
    this.cargarEstadosCitas();
  }

  // Carga el listado paginado de citas
  cargarCitas(): void {
    this.citasService.getCitas(this.paginaActual, this.tamanoPagina).subscribe({
      next: (resultado) => {
        this.citas.set(resultado.data);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: (error) => console.error('Error cargando citas:', error)
    });
  }

  // Carga el catÃ¡logo de pacientes
  cargarPacientes(): void {
    this.pacientesService.getPacientes(1, 1000).subscribe({
      next: (resultado) => this.pacientes.set(resultado.data),
      error: (error) => console.error('Error cargando pacientes:', error)
    });
  }

  // Carga el catÃ¡logo de consultorios
  cargarConsultorios(): void {
    this.consultoriosService.getConsultorios(1, 1000).subscribe({
      next: (resultado) => this.consultorios.set(resultado.data),
      error: (error) => console.error('Error cargando consultorios:', error)
    });
  }

  // Carga el catÃ¡logo de estados de citas
  cargarEstadosCitas(): void {
    this.estadoCitasService.getEstadosCitas(1, 1000).subscribe({
      next: (resultado) => this.estadosCitas.set(resultado.data),
      error: (error) => console.error('Error cargando estados:', error)
    });
  }

  // Obtiene el nombre completo del paciente por su ID
  obtenerNombrePaciente(idPaciente: number): string {
    const paciente = this.pacientes().find(p => p.idPaciente === idPaciente);
    return paciente ? `${paciente.nombres} ${paciente.apellidos}` : `Paciente #${idPaciente}`;
  }

  // Obtiene el nombre del consultorio por su ID
  obtenerNombreConsultorio(idConsultorio: number): string {
    const consultorio = this.consultorios().find(c => c.idConsultorio === idConsultorio);
    return consultorio ? consultorio.nombre : `Consultorio #${idConsultorio}`;
  }

  // Obtiene el nombre del estado por su ID
  obtenerNombreEstado(idEstadoCita: number): string {
    const estado = this.estadosCitas().find(e => e.idEstadoCita === idEstadoCita);
    return estado ? estado.nombreEstado : `Estado #${idEstadoCita}`;
  }

  // Abre el formulario para registrar una nueva cita
  abrirNuevaCita(): void {
    this.modoFormulario = 'nuevo';
    this.citaSeleccionada = null;
    this.nuevaCita = this.crearCitaVacia();
    this.mostrarFormulario = true;
  }

  // Abre el formulario en modo lectura para una cita existente
  verCita(cita: Cita): void {
    this.citasService.getCitaById(cita.idCita).subscribe({
      next: (resultado) => {
        this.modoFormulario = 'ver';
        this.citaSeleccionada = resultado;
        this.nuevaCita = {
          ...resultado,
          horaInicio: this.formatearHora(resultado.horaInicio),
          horaFin: this.formatearHora(resultado.horaFin)
        };
        this.mostrarFormulario = true;
      },
      error: (error) => console.error('Error obteniendo cita:', error)
    });
  }

  // Abre el formulario en modo ediciÃ³n para una cita existente
  editarCita(cita: Cita): void {
    this.citasService.getCitaById(cita.idCita).subscribe({
      next: (resultado) => {
        this.modoFormulario = 'editar';
        this.citaSeleccionada = resultado;
        this.nuevaCita = {
          ...resultado,
          horaInicio: this.formatearHora(resultado.horaInicio),
          horaFin: this.formatearHora(resultado.horaFin)
        };
        this.mostrarFormulario = true;
      },
      error: (error) => console.error('Error obteniendo cita:', error)
    });
  }

  // Elimina una cita previa confirmaciÃ³n
  eliminarCita(cita: Cita): void {
    const paciente = this.obtenerNombrePaciente(cita.idPaciente);
    if (!confirm(`Â¿Desea eliminar la cita #${cita.idCita} de ${paciente}?`)) {
      return;
    }

    this.citasService.deleteCita(cita.idCita).subscribe({
      next: () => {
        if (this.citas().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarCitas();
      },
      error: (error) => console.error('Error eliminando cita:', error)
    });
  }

  // Valida y procesa el guardado o actualizaciÃ³n de la cita
  guardarCita(): void {
    if (this.nuevaCita.idPaciente <= 0) {
      alert('Seleccione un paciente.');
      return;
    }
    if (this.nuevaCita.idMedico <= 0) {
      alert('Ingrese un mÃ©dico vÃ¡lido.');
      return;
    }
    if (this.nuevaCita.idConsultorio <= 0) {
      alert('Seleccione un consultorio.');
      return;
    }
    if (this.nuevaCita.idEstadoCita <= 0) {
      alert('Seleccione un estado.');
      return;
    }
    if (!this.nuevaCita.fechaCita) {
      alert('Seleccione la fecha de la cita.');
      return;
    }
    if (!this.nuevaCita.horaInicio || !this.nuevaCita.horaFin) {
      alert('Ingrese la hora de inicio y fin.');
      return;
    }
    if (this.nuevaCita.horaInicio >= this.nuevaCita.horaFin) {
      alert('La hora de inicio debe ser menor que la hora de fin.');
      return;
    }

    if (this.modoFormulario === 'nuevo') {
      this.agregarCita();
    } else if (this.modoFormulario === 'editar') {
      this.actualizarCita();
    }
  }

  private agregarCita(): void {
    const cita = this.prepararCitaParaApi(this.nuevaCita);
    this.citasService.addCita(cita).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarCitas();
      },
      error: (error) => console.error('Error agregando cita:', error)
    });
  }

  private actualizarCita(): void {
    const cita = this.prepararCitaParaApi(this.nuevaCita);
    this.citasService.updateCita(cita).subscribe({
      next: () => {
        this.cerrarFormulario();
        this.cargarCitas();
      },
      error: (error) => console.error('Error actualizando cita:', error)
    });
  }

  // Cierra y limpia el formulario
  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.citaSeleccionada = null;
    this.nuevaCita = this.crearCitaVacia();
  }

  // Cambia la pÃ¡gina actual de la tabla
  cambiarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas || pagina === this.paginaActual) {
      return;
    }
    this.paginaActual = pagina;
    this.cargarCitas();
  }

  // Modifica el tamaÃ±o de la pÃ¡gina y reinicia a la primera
  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarCitas();
  }

  private prepararCitaParaApi(cita: Cita): Cita {
    return {
      ...cita,
      horaInicio: this.convertirHoraParaApi(cita.horaInicio),
      horaFin: this.convertirHoraParaApi(cita.horaFin)
    };
  }

  private convertirHoraParaApi(hora: string): string {
    if (!hora) return '';
    return hora.length === 5 ? `${hora}:00` : hora;
  }

  // Formatea la hora para mostrar solo HH:mm en la interfaz
  formatearHora(hora: string): string {
    if (!hora) return '';
    return hora.substring(0, 5);
  }

  private crearCitaVacia(): Cita {
    return {
      idCita: 0,
      idPaciente: 0,
      idMedico: 0,
      idConsultorio: 0,
      idEstadoCita: 0,
      fechaCita: '',
      horaInicio: '',
      horaFin: '',
      motivoConsulta: '',
      observaciones: '',
      fechaRegistro: new Date().toISOString()
    };
  }
}
