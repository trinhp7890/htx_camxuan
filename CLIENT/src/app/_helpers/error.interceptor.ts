import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '@app/_services';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(catchError(err => {
        if ([401, 403].includes(err.status)) {
          this.authService.logout();
        }
        var error = '';
        if ([400].includes(err.status)) {
          error = err.error.error_description;
        }
        else {
          error = (err && err.error && err.error.message) || err.statusText;
        }
        return throwError(error);
      }));
  }
}
