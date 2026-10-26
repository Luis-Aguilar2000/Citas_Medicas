import {
  Component
} from '@angular/core';

import {
  RouterOutlet
} from '@angular/router';

import {
  Sidebar
} from '../sidebar/sidebar';

import {
  Header
} from '../header/header';
import { inject } from '@angular/core';
import { AuthService } from '../../core/Auth/auth.service';


@Component({
  selector: 'app-main-layout',
  standalone: true,

  imports: [
    RouterOutlet,
    Sidebar,
    Header
  ],

  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css'
})
export class MainLayout {

  sidebarColapsado = false;


  cambiarSidebar(): void {

    this.sidebarColapsado =
      !this.sidebarColapsado;

  }

}