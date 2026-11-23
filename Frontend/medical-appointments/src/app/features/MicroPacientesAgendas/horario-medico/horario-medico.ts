
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  HorarioMedicoService,
  HorarioMedico as HorarioMedicoModel
} from '../../../core/services/MicroPacientesAgendas/horario-medico';

import {
  ConsultoriosService,
  Consultorio
} from '../../../core/services/MicroPacientesAgendas/consultorios';

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

import { BusquedaService } from '../../../core/services/Compartidos/busqueda';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';
import { Paginacion } from '../../../shared/components/paginacion/paginacion';

@Component({
  selector: 'app-horario-medico',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones,
    Paginacion
  ],
  templateUrl: './horario-medico.html',
  styleUrl: './horario-medico.css'
})
export class HorarioMedico implements OnInit {

  horarios = signal<HorarioMedicoModel[]>([]);
  medicos = signal<Medico[]>([]);
  consultorios = signal<Consultorio[]>([]);
  especialidades = signal<Especialidad[]>([]);
  relacionesMedicoEspecialidad = signal<MedicoEspecialidad[]>([]);

  especialidadesDisponibles = signal<Especialidad[]>([]);

  paginaActual = 1;
  tamanoPagina = 10;
  totalRegistros = 0;
  totalPaginas = 0;

  mostrarFormulario = false;
  cargandoMedicos = false;
  cargandoEspecialidades = false;
  cargandoRelaciones = false;
  guardando = false;

  modoFormulario: 'nuevo' | 'ver' | 'editar' = 'nuevo';

  horarioSeleccionado: HorarioMedicoModel | null = null;
  nuevoHorario: HorarioMedicoModel = this.crearHorarioVacio();

  diasSemana: string[] = [
    'Lunes',
    'Martes',
    'Miércoles',
    'Jueves',
    'Viernes',
    'Sábado',
    'Domingo'
  ];

  constructor(
    private horarioService: HorarioMedicoService,
    private consultoriosService: ConsultoriosService,
    private medicosService: MedicosService,
    private especialidadesService: EspecialidadesService,
    private medicoEspecialidadesService: MedicoEspecialidadesService,
    private busquedaService: BusquedaService
  ) {}

  ngOnInit(): void {
    this.busquedaService.configurar('Buscar horarios médicos...');

    this.cargarHorarios();
    this.cargarConsultorios();
    this.cargarMedicos();
    this.cargarEspecialidades();
    this.cargarRelacionesMedicoEspecialidad();
  }

  cargarHorarios(): void {
    this.horarioService.getHorarios(
      this.paginaActual,
      this.tamanoPagina
    ).subscribe({
      next: resultado => {
        this.horarios.set(resultado.data ?? []);
        this.totalRegistros = resultado.totalRecords;
        this.totalPaginas = resultado.totalPages;
        this.paginaActual = resultado.currentPage;
        this.tamanoPagina = resultado.pageSize;
      },
      error: error => console.error('Error cargando horarios:', error)
    });
  }

  cargarMedicos(): void {
    this.cargandoMedicos = true;

    this.medicosService.getMedicos().subscribe({
      next: resultado => {
        this.medicos.set(
          resultado.filter(medico => medico.estado)
        );
        this.cargandoMedicos = false;
      },
      error: error => {
        console.error('Error cargando médicos:', error);
        this.cargandoMedicos = false;
      }
    });
  }

  cargarConsultorios(): void {
    this.consultoriosService.getConsultorios(1, 1000).subscribe({
      next: resultado => {
        this.consultorios.set(resultado.data ?? []);
      },
      error: error => console.error('Error cargando consultorios:', error)
    });
  }

  cargarEspecialidades(): void {
    this.cargandoEspecialidades = true;

    this.especialidadesService.getEspecialidades(1, 1000).subscribe({
      next: resultado => {
        this.especialidades.set(
          (resultado.data ?? []).filter(e => e.estado !== false)
        );

        this.cargandoEspecialidades = false;
        this.actualizarEspecialidadesDisponibles();
      },
      error: error => {
        console.error('Error cargando especialidades:', error);
        this.cargandoEspecialidades = false;
      }
    });
  }

  cargarRelacionesMedicoEspecialidad(): void {
    this.cargandoRelaciones = true;

    this.medicoEspecialidadesService
      .getMedicoEspecialidades(1, 1000)
      .subscribe({
        next: resultado => {
          this.relacionesMedicoEspecialidad.set(resultado.data ?? []);
          this.cargandoRelaciones = false;
          this.actualizarEspecialidadesDisponibles();
        },
        error: error => {
          console.error('Error cargando relaciones médico-especialidad:', error);
          this.cargandoRelaciones = false;
        }
      });
  }

  cambiarMedico(idMedico: number): void {
    this.nuevoHorario.idMedico = Number(idMedico);
    this.nuevoHorario.idEspecialidad = 0;
    this.actualizarEspecialidadesDisponibles();
  }

  actualizarEspecialidadesDisponibles(): void {
    const idMedico = Number(this.nuevoHorario.idMedico);

    if (!idMedico) {
      this.especialidadesDisponibles.set([]);
      return;
    }

    const idsEspecialidades = this.relacionesMedicoEspecialidad()
      .filter(relacion => Number(relacion.idMedico) === idMedico)
      .map(relacion => Number(relacion.idEspecialidad));

    this.especialidadesDisponibles.set(
      this.especialidades().filter(especialidad =>
        idsEspecialidades.includes(Number(especialidad.idEspecialidad))
      )
    );

    if (
      this.nuevoHorario.idEspecialidad &&
      !idsEspecialidades.includes(Number(this.nuevoHorario.idEspecialidad))
    ) {
      this.nuevoHorario.idEspecialidad = 0;
    }
  }

  obtenerNombreMedico(idMedico: number): string {
    const medico = this.medicos().find(
      m => Number(m.idMedico) === Number(idMedico)
    );

    return medico
      ? `${medico.nombres} ${medico.apellidos}`
      : `Médico #${idMedico}`;
  }

  obtenerNombreEspecialidad(idEspecialidad: number): string {
    const especialidad = this.especialidades().find(
      e => Number(e.idEspecialidad) === Number(idEspecialidad)
    );

    return especialidad
      ? especialidad.nombreEspecialidad || especialidad.nombre || `Especialidad #${idEspecialidad}`
      : `Especialidad #${idEspecialidad}`;
  }

  obtenerNombreConsultorio(idConsultorio: number): string {
    const consultorio = this.consultorios().find(
      c => c.idConsultorio === idConsultorio
    );

    return consultorio
      ? consultorio.nombre
      : `Consultorio #${idConsultorio}`;
  }

  cambiarPagina(pagina: number): void {
    if (
      pagina < 1 ||
      pagina > this.totalPaginas ||
      pagina === this.paginaActual
    ) return;

    this.paginaActual = pagina;
    this.cargarHorarios();
  }

  cambiarTamanoPagina(tamano: number): void {
    this.tamanoPagina = tamano;
    this.paginaActual = 1;
    this.cargarHorarios();
  }

  abrirNuevoHorario(): void {
    this.modoFormulario = 'nuevo';
    this.horarioSeleccionado = null;
    this.nuevoHorario = this.crearHorarioVacio();
    this.actualizarEspecialidadesDisponibles();
    this.mostrarFormulario = true;
  }

  verHorario(horario: HorarioMedicoModel): void {
    this.horarioService.getHorarioById(horario.idHorario).subscribe({
      next: data => {
        this.modoFormulario = 'ver';
        this.horarioSeleccionado = data;

        this.nuevoHorario = {
          ...data,
          horaInicio: this.formatearHora(data.horaInicio),
          horaFin: this.formatearHora(data.horaFin)
        };

        this.actualizarEspecialidadesDisponibles();
        this.mostrarFormulario = true;
      },
      error: error => console.error('Error obteniendo horario:', error)
    });
  }

  editarHorario(horario: HorarioMedicoModel): void {
    this.horarioService.getHorarioById(horario.idHorario).subscribe({
      next: data => {
        this.modoFormulario = 'editar';
        this.horarioSeleccionado = data;

        this.nuevoHorario = {
          ...data,
          horaInicio: this.formatearHora(data.horaInicio),
          horaFin: this.formatearHora(data.horaFin)
        };

        this.actualizarEspecialidadesDisponibles();
        this.mostrarFormulario = true;
      },
      error: error => console.error('Error obteniendo horario:', error)
    });
  }

  eliminarHorario(horario: HorarioMedicoModel): void {
    const medico = this.obtenerNombreMedico(horario.idMedico);

    if (!confirm(
      `¿Desea eliminar el horario de ${medico} del ${horario.diaSemana}?`
    )) return;

    this.horarioService.deleteHorario(horario.idHorario).subscribe({
      next: () => {
        if (this.horarios().length === 1 && this.paginaActual > 1) {
          this.paginaActual--;
        }
        this.cargarHorarios();
      },
      error: error => console.error('Error eliminando horario:', error)
    });
  }

  guardarHorario(): void {
    if (this.guardando) return;

    if (
      this.nuevoHorario.idMedico <= 0 ||
      this.nuevoHorario.idEspecialidad <= 0 ||
      this.nuevoHorario.idConsultorio <= 0 ||
      !this.nuevoHorario.diaSemana ||
      !this.nuevoHorario.horaInicio ||
      !this.nuevoHorario.horaFin
    ) {
      alert('Complete todos los campos del horario.');
      return;
    }

    if (
      this.cargandoMedicos ||
      this.cargandoEspecialidades ||
      this.cargandoRelaciones
    ) {
      alert('Espere a que terminen de cargar los datos.');
      return;
    }

    const relacionValida = this.relacionesMedicoEspecialidad().some(
      relacion =>
        Number(relacion.idMedico) === Number(this.nuevoHorario.idMedico) &&
        Number(relacion.idEspecialidad) === Number(this.nuevoHorario.idEspecialidad)
    );

    if (!relacionValida) {
      alert('La especialidad seleccionada no pertenece al médico.');
      return;
    }

    if (this.nuevoHorario.horaInicio >= this.nuevoHorario.horaFin) {
      alert('La hora de inicio debe ser menor que la hora de fin.');
      return;
    }

    if (this.modoFormulario === 'nuevo') {
      this.agregarHorario();
    } else if (this.modoFormulario === 'editar') {
      this.actualizarHorario();
    }
  }

  agregarHorario(): void {
    this.guardando = true;

    const horario = this.prepararHorarioParaApi(this.nuevoHorario);

    this.horarioService.addHorario(horario).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.paginaActual = 1;
        this.cargarHorarios();
      },
      error: error => {
        this.guardando = false;
        console.error('Error agregando horario:', error);
        alert(error.error?.message || 'No se pudo registrar el horario.');
      }
    });
  }

  actualizarHorario(): void {
    this.guardando = true;

    const horario = this.prepararHorarioParaApi(this.nuevoHorario);

    this.horarioService.updateHorario(horario).subscribe({
      next: () => {
        this.guardando = false;
        this.cerrarFormulario();
        this.cargarHorarios();
      },
      error: error => {
        this.guardando = false;
        console.error('Error actualizando horario:', error);
        alert(error.error?.message || 'No se pudo actualizar el horario.');
      }
    });
  }

  cerrarFormulario(): void {
    if (this.guardando) return;

    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.horarioSeleccionado = null;
    this.limpiarFormulario();
  }

  limpiarFormulario(): void {
    this.nuevoHorario = this.crearHorarioVacio();
    this.especialidadesDisponibles.set([]);
  }

  private prepararHorarioParaApi(
    horario: HorarioMedicoModel
  ): HorarioMedicoModel {
    return {
      ...horario,
      idMedico: Number(horario.idMedico),
      idEspecialidad: Number(horario.idEspecialidad),
      idConsultorio: Number(horario.idConsultorio),
      horaInicio: this.convertirHoraParaApi(horario.horaInicio),
      horaFin: this.convertirHoraParaApi(horario.horaFin)
    };
  }

  private convertirHoraParaApi(hora: string): string {
    if (!hora) return '00:00:00';
    return hora.length === 5 ? `${hora}:00` : hora;
  }

  formatearHora(hora: string): string {
    if (!hora) return '';
    return hora.substring(0, 5);
  }

  private crearHorarioVacio(): HorarioMedicoModel {
    return {
      idHorario: 0,
      idMedico: 0,
      idEspecialidad: 0,
      idConsultorio: 0,
      diaSemana: '',
      horaInicio: '',
      horaFin: '',
      estado: true
    };
  }
}
