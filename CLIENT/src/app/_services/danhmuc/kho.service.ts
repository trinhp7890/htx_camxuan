import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/dmkho`;

@Injectable({
  providedIn: 'root'
})
export class KhoService {

  constructor(private http: HttpClient) { }

  get_all() {
      return this.http.get<any[]>(`${baseUrl}/getall`);
  }
  get_byid(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/getbyma`,
      formData
    );
  }

  Del(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/xoa`,
      formData
    );
  }
  kho_ins(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/capnhat`,
      formData
    );
  }

  kho_up(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/capnhat`,
      formData
    );
  } 
}
