import { Routes } from '@angular/router';

import { MainLayout } from './layout/main-layout/main-layout';
import { Inicio } from './layout/inicio/inicio';

import { Pacientes } from './features/MicroPacientesAgendas/pacientes/pacientes';
import { Consultorios } from './features/MicroPacientesAgendas/consultorios/consultorios';
import { HorarioMedico } from './features/MicroPacientesAgendas/horario-medico/horario-medico';
import { AgendaCitas } from './features/MicroPacientesAgendas/agenda-citas/agenda-citas';

import { Login } from './features/Auth/login/login';
import {
  authGuard,
  authChildGuard
} from './core/Auth/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: Login
  },
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    canActivateChild: [authChildGuard],
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
  },
  {
    path: '**',
    redirectTo: 'inicio'
  }
];