import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HorarioMedicoService, HorarioMedico as HorarioMedicoModel } from '../../../core/services/horario-medico';
import { ConsultoriosService, Consultorio } from '../../../core/services/consultorios';
import { BotonesAcciones } from '../../../shared/components/botones-acciones/botones-acciones';

@Component({
  selector: 'app-horario-medico',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    BotonesAcciones
  ],
  templateUrl: './horario-medico.html',
  styleUrl: './horario-medico.css'
})
export class HorarioMedico implements OnInit {

  horarios = signal<HorarioMedicoModel[]>([]);
  consultorios = signal<Consultorio[]>([]);

  mostrarFormulario = false;
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
    private consultoriosService: ConsultoriosService
  ) {}

  ngOnInit(): void {
    this.cargarHorarios();
    this.cargarConsultorios();
  }

  cargarHorarios(): void {
    this.horarioService.getHorarios().subscribe({
      next: (data) => {
        console.log('HORARIOS RECIBIDOS:', data);
        this.horarios.set(data);
      },
      error: (error) => {
        console.error('Error cargando horarios:', error);
      }
    });
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

  obtenerNombreConsultorio(idConsultorio: number): string {
    const consultorio = this.consultorios().find(c => c.idConsultorio === idConsultorio);
    if (!consultorio) {
      return `Consultorio #${idConsultorio}`;
    }
    return consultorio.nombre;
  }

  abrirNuevoHorario(): void {
    this.modoFormulario = 'nuevo';
    this.horarioSeleccionado = null;
    this.nuevoHorario = this.crearHorarioVacio();
    this.mostrarFormulario = true;
  }

  verHorario(horario: HorarioMedicoModel): void {
    this.horarioService.getHorarioById(horario.idHorario).subscribe({
      next: (data) => {
        this.modoFormulario = 'ver';
        this.horarioSeleccionado = data;
        this.nuevoHorario = {
          ...data,
          horaInicio: this.formatearHora(data.horaInicio),
          horaFin: this.formatearHora(data.horaFin)
        };
        this.mostrarFormulario = true;
      },
      error: (error) => {
        console.error('Error obteniendo horario:', error);
      }
    });
  }

  editarHorario(horario: HorarioMedicoModel): void {
    this.horarioService.getHorarioById(horario.idHorario).subscribe({
      next: (data) => {
        this.modoFormulario = 'editar';
        this.horarioSeleccionado = data;
        this.nuevoHorario = {
          ...data,
          horaInicio: this.formatearHora(data.horaInicio),
          horaFin: this.formatearHora(data.horaFin)
        };
        this.mostrarFormulario = true;
      },
      error: (error) => {
        console.error('Error obteniendo horario:', error);
      }
    });
  }

  eliminarHorario(horario: HorarioMedicoModel): void {
    const consultorio = this.obtenerNombreConsultorio(horario.idConsultorio);
    const confirmar = confirm(`¿Desea eliminar el horario del ${horario.diaSemana} en ${consultorio}?`);
    if (!confirmar) {
      return;
    }

    this.horarioService.deleteHorario(horario.idHorario).subscribe({
      next: () => {
        console.log('Horario eliminado correctamente');
        this.cargarHorarios();
      },
      error: (error) => {
        console.error('Error eliminando horario:', error);
      }
    });
  }

  guardarHorario(): void {
    if (
      this.nuevoHorario.idMedico <= 0 ||
      this.nuevoHorario.idConsultorio <= 0 ||
      !this.nuevoHorario.diaSemana ||
      !this.nuevoHorario.horaInicio ||
      !this.nuevoHorario.horaFin
    ) {
      alert('Complete todos los campos del horario.');
      return;
    }

    if (this.nuevoHorario.horaInicio >= this.nuevoHorario.horaFin) {
      alert('La hora de inicio debe ser menor que la hora de fin.');
      return;
    }

    if (this.modoFormulario === 'nuevo') {
      this.agregarHorario();
      return;
    }

    if (this.modoFormulario === 'editar') {
      this.actualizarHorario();
    }
  }

  agregarHorario(): void {
    const horario = this.prepararHorarioParaApi(this.nuevoHorario);

    this.horarioService.addHorario(horario).subscribe({
      next: () => {
        console.log('Horario agregado correctamente');
        this.cerrarFormulario();
        this.cargarHorarios();
      },
      error: (error) => {
        console.error('Error agregando horario:', error);
      }
    });
  }

  actualizarHorario(): void {
    const horario = this.prepararHorarioParaApi(this.nuevoHorario);

    this.horarioService.updateHorario(horario).subscribe({
      next: () => {
        console.log('Horario actualizado correctamente');
        this.cerrarFormulario();
        this.cargarHorarios();
      },
      error: (error) => {
        console.error('Error actualizando horario:', error);
      }
    });
  }

  cerrarFormulario(): void {
    this.mostrarFormulario = false;
    this.modoFormulario = 'nuevo';
    this.horarioSeleccionado = null;
    this.nuevoHorario = this.crearHorarioVacio();
  }

  private prepararHorarioParaApi(horario: HorarioMedicoModel): HorarioMedicoModel {
    return {
      ...horario,
      horaInicio: this.convertirHoraParaApi(horario.horaInicio),
      horaFin: this.convertirHoraParaApi(horario.horaFin)
    };
  }

  private convertirHoraParaApi(hora: string): string {
    if (!hora) {
      return '00:00:00';
    }
    if (hora.length === 5) {
      return `${hora}:00`;
    }
    return hora;
  }

  formatearHora(hora: string): string {
    if (!hora) {
      return '';
    }
    return hora.substring(0, 5);
  }

  private crearHorarioVacio(): HorarioMedicoModel {
    return {
      idHorario: 0,
      idMedico: 0,
      idConsultorio: 0,
      diaSemana: '',
      horaInicio: '',
      horaFin: '',
      estado: true
    };
  }

}