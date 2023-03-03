import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Quyen_nd } from '@app/_models/sys/quyen_nd';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/nguoidung/quyen`;

@Injectable({
  providedIn: 'root'
})
export class Quyen_ndService {

  constructor(private http: HttpClient) { }

  get_by_NSD(prmID_ND: number, prmNSD : string, prmDK: number) {
    return this.http.get<Quyen_nd[]>(
      `${baseUrl}/load_quyen_byNSD?prmID_ND=`+ prmID_ND + `&prmNSD=` + prmNSD + `&prmDK=` + prmDK
      );
  }
 
  get_by_nhom_NSD(prmID: number, prmNSD : string, prmDK: number) {
    return this.http.get<Quyen_nd[]>(
      `${baseUrl}/load_quyennhom_byNSD?prmID_NHOM=`+ prmID + `&prmNSD=` + prmNSD + `&prmDK=` + prmDK
      );
  }

  insert(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/create_quyennd?prmID_ND=`+ model.id_nd + `&prmID_QUYEN=`
                  + model.id_quyen+`&prmNGUOI_CN=`+ model.nguoi_cn,
      { withCredentials: true }
    );
  }

  delete(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete_quyennd?prmID_ND=`+ model.id_nd + `&prmID_QUYEN=`
                + model.id_quyen+`&prmNGUOI_CN=`+ model.nguoi_cn,
      { withCredentials: true }
      );
  }
  
  insert_nhom(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/create_quyennhom?prmID_NHOM=`+ model.id + `&prmID_QUYEN=`
                  + model.id_quyen+`&prmNGUOI_CN=`+ model.nguoi_cn,
      { withCredentials: true }
    );
  }

  delete_nhom(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete_quyennhom?prmID_NHOM=`+ model.id + `&prmID_QUYEN=`
                + model.id_quyen+`&prmNGUOI_CN=`+ model.nguoi_cn,
      { withCredentials: true }
      );
  }
  

}
