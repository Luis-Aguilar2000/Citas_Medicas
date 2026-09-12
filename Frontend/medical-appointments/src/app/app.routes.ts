import { Routes } from '@angular/router';

import { MainLayout } from './layout/main-layout/main-layout';

import { Inicio } from './inicio/inicio';
import { Pacientes } from './pacientes/pacientes';
import { Consultorios } from './consultorios/consultorios';
import { HorarioMedico } from './horario-medico/horario-medico';

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
        path: '',
        redirectTo: 'inicio',
        pathMatch: 'full'
      }

    ]
  }

];