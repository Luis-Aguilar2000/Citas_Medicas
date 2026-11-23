
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  CitasService,
  Cita
} from '../../../core/services/MicroPacientesAgendas/citas';

import {
  PacientesService,
  Paciente
} from '../../../core/services/MicroPacientesAgendas/pacientes';

import {
  ConsultoriosService,
  Consultorio
} from '../../../core/services/MicroPacientesAgendas/consultorios';

import {
  EstadoCitasService,
  EstadoCita
} from '../../../core/services/MicroPacientesAgendas/estado-citas';

import {
  HorariosMedicosService,
  HorarioDisponible
} from '../../../core/services/MicroIdentidadUsuarios/horarios-medicos';

import {
  MedicosService,
  Medico
} from '../../../core/services/MicroIdentidadUsuarios/medicos';

import {
  EspecialidadesService,
  Especialidad
} from '../../../core/services/MicroIdentidadUsuarios/especialidades';

import {
  MedicoEspecialidadesService,
  MedicoEspecialidad
} from '../../../core/services/MicroIdentidadUsuarios/medico-especialidad';

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
  medicos = signal<Medico[]>([]);
  consultorios = signal<Consultorio[]>([]);
  estadosCitas = signal<EstadoCita[]>([]);

  especialidades = signal<Especialidad[]>([]);
  relacionesMedicoEspecialidad = signal<MedicoEspecialidad[]>([]);
  especialidadesDisponibles = signal<Especialidad[]>([]);

  horariosDisponibles = signal<HorarioDisponible[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  mostrarFormulario = false;
  guardando = false;
  cargandoHorarios = false;
  errorHorarios = '';

  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';

  citaSeleccionada: Cita | null = null;
  nuevaCita: Cita = this.crearCitaVacia();

  constructor(
    private citasService: CitasService,
    private pacientesService: PacientesService,
    private medicosService: MedicosService,
    private consultoriosService: ConsultoriosService,
    private estadoCitasService: EstadoCitasService,
    private especialidadesService: EspecialidadesService,
    private medicoEspecialidadesService: MedicoEspecialidadesService,
    private horariosMedicosService: HorariosMedicosService,
    private busquedaService: BusquedaService
  ) {}

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar citas...');

    this.cargarCitas();
    this.cargarPacientes();
    this.cargarMedicos();
    this.cargarConsultorios();
    this.cargarEstadosCitas();
    this.cargarEspecialidades();
    this.cargarRelacionesMedicoEspecialidad();
  }

  // =========================================
  // CARGAR INFORMACIÓN
  // =========================================

  cargarCitas(): void {
    this.citasService.getCitas(
      this.paginaActual,
      this.tamanoPagina
    ).subscribe({
      next: resultado => {
        this.citas.set(resultado.data ?? []);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
      },
      error: error => console.error('Error cargando citas:', error)
    });
  }

  cargarPacientes(): void {
    this.pacientesService.getPacientes(1, 1000).subscribe({
      next: resultado => this.pacientes.set(resultado.data ?? []),
      error: error => console.error('Error cargando pacientes:', error)
    });
  }

  cargarMedicos(): void {
    this.medicosService.getMedicos().subscribe({
      next: resultado => {
        this.medicos.set(
          resultado.filter(medico => medico.estado)
        );
      },
      error: error => {
        console.error('Error cargando médicos:', error);
        this.medicos.set([]);
      }
    });
  }

  cargarConsultorios(): void {
    this.consultoriosService.getConsultorios(1, 1000).subscribe({
      next: resultado => this.consultorios.set(resultado.data ?? []),
      error: error => console.error('Error cargando consultorios:', error)
    });
  }

  cargarEstadosCitas(): void {
    this.estadoCitasService.getEstadosCitas(1, 1000).subscribe({
      next: resultado => this.estadosCitas.set(resultado.data ?? []),
      error: error => console.error('Error cargando estados:', error)
    });
  }

  cargarEspecialidades(): void {
    this.especialidadesService.getEspecialidades(1, 1000).subscribe({
      next: resultado => {
        this.especialidades.set(resultado.data ?? []);
        this.actualizarEspecialidadesDisponibles();
      },
      error: error => {
        console.error('Error cargando especialidades:', error);
        this.especialidades.set([]);
      }
    });
  }

  cargarRelacionesMedicoEspecialidad(): void {
    this.medicoEspecialidadesService
      .getMedicoEspecialidades(1, 1000)
      .subscribe({
        next: resultado => {
          this.relacionesMedicoEspecialidad.set(resultado.data ?? []);
          this.actualizarEspecialidadesDisponibles();
        },
        error: error => {
          console.error('Error cargando relaciones:', error);
          this.relacionesMedicoEspecialidad.set([]);
        }
      });
  }

  // =========================================
  // MÉDICO Y ESPECIALIDAD
  // =========================================

  actualizarEspecialidadesDisponibles(): void {
    const idMedico = Number(this.nuevaCita.idMedico);

    if (idMedico <= 0) {
      this.especialidadesDisponibles.set([]);
      return;
    }

    const ids = this.relacionesMedicoEspecialidad()
      .filter(r => Number(r.idMedico) === idMedico)
      .map(r => Number(r.idEspecialidad));

    this.especialidadesDisponibles.set(
      this.especialidades().filter(e =>
        ids.includes(Number(e.idEspecialidad))
      )
    );
  }

  cambiarMedico(idMedico: number): void {
    this.nuevaCita.idMedico = Number(idMedico);
    this.nuevaCita.idEspecialidad = 0;
    this.nuevaCita.idConsultorio = 0;
    this.nuevaCita.horaInicio = '';
    this.nuevaCita.horaFin = '';

    this.especialidadesDisponibles.set([]);
    this.limpiarHorarios();

    this.actualizarEspecialidadesDisponibles();
  }

  cambiarEspecialidad(): void {
    this.nuevaCita.idConsultorio = 0;
    this.nuevaCita.horaInicio = '';
    this.nuevaCita.horaFin = '';

    this.limpiarHorarios();
    this.consultarHorariosDisponibles();
  }

  cambiarFecha(): void {
    this.nuevaCita.idConsultorio = 0;
    this.nuevaCita.horaInicio = '';
    this.nuevaCita.horaFin = '';

    this.limpiarHorarios();
    this.consultarHorariosDisponibles();
  }

  obtenerNombreEspecialidad(id: number): string {
    const especialidad = this.especialidades().find(
      e => Number(e.idEspecialidad) === Number(id)
    );

    return especialidad?.nombreEspecialidad
      || especialidad?.nombre
      || `Especialidad #${id}`;
  }

  // =========================================
  // DISPONIBILIDAD DE HORARIOS
  // =========================================

  consultarHorariosDisponibles(): void {
    const idMedico = Number(this.nuevaCita.idMedico);
    const idEspecialidad = Number(this.nuevaCita.idEspecialidad);
    const fecha = this.nuevaCita.fechaCita;

    if (
      idMedico <= 0 ||
      idEspecialidad <= 0 ||
      !fecha
    ) {
      return;
    }

    this.cargandoHorarios = true;
    this.errorHorarios = '';
    this.horariosDisponibles.set([]);

    this.horariosMedicosService.getHorariosDisponibles(
      idMedico,
      idEspecialidad,
      fecha
    ).subscribe({
      next: horarios => {
        this.horariosDisponibles.set(horarios ?? []);
        this.cargandoHorarios = false;
      },
      error: error => {
        console.error('Error consultando horarios:', error);
        this.errorHorarios =
          'No fue posible consultar los horarios disponibles.';
        this.cargandoHorarios = false;
      }
    });
  }

  seleccionarHorario(horario: HorarioDisponible): void {
    if (this.modoFormulario === 'ver' || this.guardando) {
      return;
    }

    this.nuevaCita.horaInicio =
      this.formatearHora(horario.horaInicio);

    this.nuevaCita.horaFin =
      this.formatearHora(horario.horaFin);

    this.nuevaCita.idConsultorio = horario.idConsultorio;
  }

  esHorarioSeleccionado(horario: HorarioDisponible): boolean {
    return (
      this.nuevaCita.horaInicio ===
        this.formatearHora(horario.horaInicio) &&
      this.nuevaCita.horaFin ===
        this.formatearHora(horario.horaFin) &&
      Number(this.nuevaCita.idConsultorio) ===
        Number(horario.idConsultorio)
    );
  }

  limpiarHorarios(): void {
    this.horariosDisponibles.set([]);
    this.cargandoHorarios = false;
    this.errorHorarios = '';
  }

  // =========================================
  // NOMBRES PARA LA TABLA
  // =========================================

  obtenerNombrePaciente(id: number): string {
    const paciente = this.pacientes().find(
      p => p.idPaciente === id
    );

    return paciente
      ? `${paciente.nombres} ${paciente.apellidos}`
      : `Paciente #${id}`;
  }

  obtenerNombreMedico(id: number): string {
    const medico = this.medicos().find(
      m => m.idMedico === id
    );

    return medico
      ? `${medico.nombres} ${medico.apellidos}`
      : `Médico #${id}`;
  }

  obtenerNombreConsultorio(id: number): string {
    const consultorio = this.consultorios().find(
      c => c.idConsultorio === id
    );

    return consultorio
      ? consultorio.nombre
      : `Consultorio #${id}`;
  }

  obtenerNombreEstado(id: number): string {
    const estado = this.estadosCitas().find(
      e => e.idEstadoCita === id
    );

    return estado
      ? estado.nombreEstado
      : `Estado #${id}`;
  }

  // =========================================
  // FORMULARIO
  // =========================================

  abrirNuevaCita(): void {
    this.modoFormulario = 'nuevo';
    this.citaSeleccionada = null;
    this.nuevaCita = this.crearCitaVacia();

    this.especialidadesDisponibles.set([]);
    this.limpiarHorarios();

    this.mostrarFormulario = true;
  }

  verCita(cita: Cita): void {
    this.citasService.getCitaById(cita.idCita).subscribe({
      next: resultado => {
        this.modoFormulario = 'ver';
        this.citaSeleccionada = resultado;

        this.nuevaCita = {
          ...resultado,
          horaInicio: this.formatearHora(resultado.horaInicio),
          horaFin: this.formatearHora(resultado.horaFin)
        };

        this.actualizarEspecialidadesDisponibles();
        this.mostrarFormulario = true;
      },
      error: error => console.error('Error obteniendo cita:', error)
    });
  }

  editarCita(cita: Cita): void {
    this.citasService.getCitaById(cita.idCita).subscribe({
      next: resultado => {
        this.modoFormulario = 'editar';
        this.citaSeleccionada = resultado;

        this.nuevaCita = {
          ...resultado,
          horaInicio: this.formatearHora(resultado.horaInicio),
          horaFin: this.formatearHora(resultado.horaFin)
        };

        this.actualizarEspecialidadesDisponibles();
        this.mostrarFormulario = true;

        this.consultarHorariosDisponibles();
      },
      error: error => console.error('Error obteniendo cita:', error)
    });
  }

  cerrarFormulario(): void {
    if (this.guardando) return;

    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.citaSeleccionada = null;
    this.nuevaCita = this.crearCitaVacia();

    this.especialidadesDisponibles.set([]);
    this.limpiarHorarios();
  }

  // =========================================
  // VALIDAR Y GUARDAR
  // =========================================

  guardarCita(): void {
    if (this.guardando) return;

    if (this.nuevaCita.idPaciente <= 0) {
      alert('Seleccione un paciente.');
      return;
    }

    if (this.nuevaCita.idMedico <= 0) {
      alert('Seleccione un médico.');
      return;
    }

    if (this.nuevaCita.idEspecialidad <= 0) {
      alert('Seleccione una especialidad.');
      return;
    }

    if (!this.especialidadesDisponibles().some(
      e => Number(e.idEspecialidad) ===
        Number(this.nuevaCita.idEspecialidad)
    )) {
      alert('La especialidad no corresponde al médico seleccionado.');
      return;
    }

    if (!this.nuevaCita.fechaCita) {
      alert('Seleccione la fecha de la cita.');
      return;
    }

    if (!this.nuevaCita.horaInicio || !this.nuevaCita.horaFin) {
      alert('Seleccione un horario disponible.');
      return;
    }

    if (this.nuevaCita.idConsultorio <= 0) {
      alert('No se ha asignado un consultorio.');
      return;
    }

    if (!this.nuevaCita.motivoConsulta?.trim()) {
      alert('Ingrese el motivo de consulta.');
      return;
    }

    if (this.modoFormulario === 'nuevo') {
      this.agregarCita();
    } else if (this.modoFormulario === 'editar') {
      this.actualizarCita();
    }
  }

  private agregarCita(): void {
    this.guardando = true;

    const cita = this.prepararCitaParaApi(this.nuevaCita);

    this.citasService.addCita(cita).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarCitas();
      },
      error: error => {
        this.guardando = false;
        console.error('Error agregando cita:', error);
        alert(
          error.error?.mensaje ||
          error.error?.message ||
          'No se pudo registrar la cita.'
        );
      }
    });
  }

  private actualizarCita(): void {
    this.guardando = true;

    const cita = this.prepararCitaParaApi(this.nuevaCita);

    this.citasService.updateCita(cita).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.cargarCitas();
      },
      error: error => {
        this.guardando = false;
        console.error('Error actualizando cita:', error);
        alert(
          error.error?.mensaje ||
          error.error?.message ||
          'No se pudo actualizar la cita.'
        );
      }
    });
  }

  private prepararCitaParaApi(cita: Cita): Cita {
    return {
      ...cita,
      idMedico: Number(cita.idMedico),
      idEspecialidad: Number(cita.idEspecialidad),
      idConsultorio: Number(cita.idConsultorio),
      horaInicio: this.convertirHoraParaApi(cita.horaInicio),
      horaFin: this.convertirHoraParaApi(cita.horaFin)
    };
  }

  // =========================================
  // ELIMINACIÓN ACTUAL
  // =========================================

  eliminarCita(cita: Cita): void {
    const paciente = this.obtenerNombrePaciente(cita.idPaciente);

    if (!confirm(
      `¿Desea eliminar la cita #${cita.idCita} de ${paciente}?`
    )) {
      return;
    }

    this.citasService.deleteCita(cita.idCita).subscribe({
      next: () => {
        if (this.citas().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }

        this.cargarCitas();
      },
      error: error => console.error('Error eliminando cita:', error)
    });
  }

  // =========================================
  // PAGINACIÓN Y UTILIDADES
  // =========================================

  private convertirHoraParaApi(hora: string): string {
    if (!hora) return '';
    return hora.length === 5 ? `${hora}:00` : hora;
  }

  formatearHora(hora: string): string {
    if (!hora) return '';
    return hora.substring(0, 5);
  }

  cambiarPagina(pagina: number): void {
    if (
      pagina < 1 ||
      pagina > this.totalPaginas ||
      pagina === this.paginaActual
    ) {
      return;
    }

    this.paginaActual = pagina;
    this.cargarCitas();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarCitas();
  }

  private crearCitaVacia(): Cita {
    return {
      idCita: 0,
      idPaciente: 0,
      idMedico: 0,
      idEspecialidad: 0,
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
