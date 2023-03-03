import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpParams, 
  HttpClient, 
  HttpHeaders, 
  HttpErrorResponse, 
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '@app/_services';
import { environment } from '@environments/environment';
import {  } from '@angular/common/http';
@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let accessToken = localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token') || '';
    let isLoggedIn = localStorage.getItem('isLogged') ? localStorage.getItem('isLogged') : sessionStorage.getItem('isLogged') || '';
   
    const isApiUrl = request.url.startsWith(environment.apiURL);
    if (isLoggedIn==='1' && isApiUrl){
      request = request.clone({
        setHeaders: { 
          // 'Content-Type': 'application/json',
          Authorization: `Bearer ${accessToken}`}
      });
      // console.log(request);
    }
    return next.handle(request);
  }

  //#region nếu set tay thì sử dụng
  // getToken() {
  //   let accessToken = localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token') || '';
  //   if (accessToken !== '') {
  //     return `Bearer ${accessToken}`;
  //   } else {
  //     return '';
  //   } 
  // }
  
  // getRequestOptions() {
  //   var token = this.getToken();
  //   const httpOptions = {
  //     headers: new HttpHeaders({
  //       'Content-Type': 'application/json',
  //       Authorization: token
  //     })
  //   };
  //   return httpOptions;
  // }
  //#endregion
}
