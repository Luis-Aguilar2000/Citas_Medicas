import { Routes } from '@angular/router';

import { MainLayout } from './layout/main-layout/main-layout';

import { Inicio } from './layout/inicio/inicio';
import { Pacientes } from './features/MicroPacientesAgendas/pacientes/pacientes';
import { Consultorios } from './features/MicroPacientesAgendas/consultorios/consultorios';
import { HorarioMedico } from './features/MicroPacientesAgendas/horario-medico/horario-medico';
import { AgendaCitas } from './features/MicroPacientesAgendas/agenda-citas/agenda-citas';

export const routes: Routes = [

  {
    path: '',
    component: MainLayout,

    children: [

      {
        path: 'inicio',
        component: Inicio
      },

      {
        path: 'pacientes',
        component: Pacientes
      },

      {
        path: 'consultorios',
        component: Consultorios
      },

      {
        path: 'horarios-medicos',
        component: HorarioMedico
      },
      {
        path: 'agenda-citas',
        component: AgendaCitas
      },

      {
        path: '',
        redirectTo: 'inicio',
        pathMatch: 'full'
      }

    ]
  }

];