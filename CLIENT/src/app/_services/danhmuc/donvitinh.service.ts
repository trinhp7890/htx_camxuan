import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/dmdvtinh`;

@Injectable({
  providedIn: 'root'
})
export class DonvitinhService {

  constructor(private http: HttpClient) { }

  get_all() {
      return this.http.get<any[]>(`${baseUrl}/getAll`);
  }
}
