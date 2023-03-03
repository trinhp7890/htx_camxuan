import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Quyen } from '@app/_models/sys/quyen';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/quyen`;

@Injectable({
  providedIn: 'root'
})
export class QuyenService {

  constructor(private http: HttpClient) { }

  get_all() {
    return this.http.get<Quyen[]>(`${baseUrl}/getall`);
  }

}
