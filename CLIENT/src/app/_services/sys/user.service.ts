import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Nguoidung } from '@app/_models/sys/nguoidung';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/nguoidung`;

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}

  get(donvi: number) {
    return this.http.get<Nguoidung[]>(
      `${baseUrl}/getall_bydv?prmDONVI=` + donvi
    );
  }

  getgiamsat() {
    return this.http.get<Nguoidung[]>(
      `${baseUrl}/getall_giamsat`
    );
  }

  get_nguoidungchitiet(prmUSERNAME: string) {
    return this.http.get<any[]>(`${baseUrl}/getchitiet?prmUSERNAME=` + prmUSERNAME);
  }

  load_bynsd(prmNSD) {
    return this.http.get<Nguoidung[]>(
      `${baseUrl}/load_bynsd?prmNSD=` + prmNSD
    );
  }

  change_password(
    prmOldPW: string,
    prmNewPW: string,
    prmUsername: string
  ): Observable<any> {
    return this.http.post(
      `${baseUrl}/cpw?prmUsername=` +
        prmUsername +
        `&prmOldPW=` +
        prmOldPW +
        `&prmNewPW=` +
        prmNewPW,
      { withCredentials: true }
    );
  }

  insert(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/create`,
      {
        prmJsonData: model
      },
      { withCredentials: true }
    );
  }

  update(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/update`,
      {
        prmJsonData: model,
      },
      { withCredentials: true }
    );
  }

  delete(prmID: number, prmNGUOI_NHAP: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete/?prmID_ND=` + prmID + `&prmNGUOI_NHAP=` + prmNGUOI_NHAP,
      { withCredentials: true }
    );
  }

  nguoigiamsat_add(prmMA_NV: string, prmTRANGTHAI: number): Observable<any> {
    return this.http.post(
      `${baseUrl}/nguoigiamsat_add?prmMA_NV=` + prmMA_NV + `&prmTRANGTHAI=` + prmTRANGTHAI,
      { withCredentials: true }
    );
  }

  active(prmID: number, prmNGUOI_CAPNHAT: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/active?prmID_ND=`+prmID+`&prmNGUOI_CAPNHAT=`+prmNGUOI_CAPNHAT,      
      { withCredentials: true }
    );
  }

  reset(prmID_ND: number, prmNGUOI_CAPNHAT: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/reset?prmID_ND=`+prmID_ND+`&prmNGUOI_CAPNHAT=`+prmNGUOI_CAPNHAT,      
      { withCredentials: true }
    );
  }

}