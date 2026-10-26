import { inject } from '@angular/core';
import {
  HttpErrorResponse,
  HttpInterceptorFn
} from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);

  const apiOrigins = [
    auth.apiUrl
  ];

  const esApiPropia = apiOrigins.some(
    url => req.url === url || req.url.startsWith(`${url}/`)
  );

  const esLogin = req.url === `${auth.apiUrl}/Auth/login`;

  if (!esApiPropia || esLogin) {
    return next(req);
  }

  const token = auth.obtenerToken();

  const solicitud = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(solicitud).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        auth.cerrarSesion();
      }

      return throwError(() => error);
    })
  );
};