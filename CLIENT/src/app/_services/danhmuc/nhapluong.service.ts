import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/nhapluong`;

@Injectable({
  providedIn: 'root'
})
export class NhapluongService {

  constructor(private http: HttpClient) { }

  get_all() {
      return this.http.get<any[]>(`${baseUrl}/getall`);
  }
  get_byloaivattu(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/getbyloaivt`,
      formData
    );
  }
  get_bymakho(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/getbymakho`,
      formData
    );
  }
  Del(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/xoa`,
      formData
    );
  }
  nhapkho(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/nhapkho`,
      formData
    );
  }

  nhapkho_up(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/nhapkho`,
      formData
    );
  } 
}
