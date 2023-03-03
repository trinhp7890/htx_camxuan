import {  HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/dashboard`;

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  [x: string]: any;

  constructor(private http: HttpClient) { }

  list_ds_theoquytrinh(prmDK: number) {
      return this.http.get<any[]>(`${baseUrl}/list_ds_theoquytrinh?prmDK=` + prmDK);
  }
  tonghop() {
    return this.http.get<any[]>(`${baseUrl}/tonghop`);
}
}
