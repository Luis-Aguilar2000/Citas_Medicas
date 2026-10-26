import { inject } from '@angular/core';
import {
  CanActivateFn,
  CanActivateChildFn,
  Router
} from '@angular/router';

import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.estaAutenticado()
    ? true
    : router.createUrlTree(['/login']);
};

export const authChildGuard: CanActivateChildFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return auth.estaAutenticado()
    ? true
    : router.createUrlTree(['/login']);
};