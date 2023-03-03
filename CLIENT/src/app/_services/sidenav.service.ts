import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const base_phanquyen_Url = `${environment.apiURL}/api/nghiepvu/quytrinh_buoc_phanquyen`;

@Injectable({
  providedIn: 'root'
})
export class SidenavService {
  hideSideNav: boolean = false;

  constructor(private http: HttpClient) { }

  toggleSideNav(): void {
    console.log('toggle bar button!');
    this.hideSideNav = !this.hideSideNav;
  }

  public get_menuquytrinh(prmUSER_NAME: string) {
    return this.http.get<object[]>(`${base_phanquyen_Url}/get_menuquytrinh?prmUSER_NAME=`+ prmUSER_NAME);
  }
}
