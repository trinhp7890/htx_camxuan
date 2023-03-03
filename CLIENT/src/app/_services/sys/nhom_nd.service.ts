import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Nhom_nd } from '@app/_models/sys/nhom_nd';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { Nguoidung } from '@app/_models/sys/nguoidung';

const baseUrl = `${environment.apiURL}/api/nhomnd`;

@Injectable({
  providedIn: 'root'
})
export class Nhom_ndService {

  constructor(private http: HttpClient) { }

  get_all() {
      return this.http.get<Nhom_nd[]>(`${baseUrl}/getall`);
  }

  get(ma: string) {
      return this.http.get<Nhom_nd>(`${baseUrl}/ma/${ma}`);
  }

  get_trangthai(trangthai: number) {
      return this.http.get<Nhom_nd>(`${baseUrl}/trang-thai/${trangthai}`);
  }


chuyen_trangthai(prmID: number, prmNGUOI_CAPNHAT: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/trangthai?prmID=`+prmID+`&prmNGUOI_CAPNHAT=`+prmNGUOI_CAPNHAT,      
      { withCredentials: true }
    );
  }

  insert(model: any): Observable<any> {
      return this.http.post(
          `${baseUrl}/create`,
          {
              'prmJsonData': model
          },
          { withCredentials: true }
      );
  }

  update(model: any): Observable<any> {
      return this.http.post(
          `${baseUrl}/update`,
          {
              'prmJsonData': model
          },
          { withCredentials: true }
      );
  }

  delete(prmID: number, prmNGUOI_NHAP: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete?prmID=`+ prmID + `&prmNGUOI_NHAP=` + prmNGUOI_NHAP ,
      { withCredentials: true }
    );
  }

  add_user(model: any): Observable<any> {
    return this.http.post(
      `${environment.apiURL}/api/nguoidung/nguoidung_nhom/add_user`,
      {
          'prmJsonData': model
      },
      { withCredentials: true }
    );
  }

  remove_user(model: any): Observable<any> {
    return this.http.post(
      `${environment.apiURL}/api/nguoidung/nguoidung_nhom/remove_user`,
      {
          'prmJsonData': model
      },
      { withCredentials: true }
    );
  }

  get_users(model: any): Observable<any> {
    return this.http.post(
      `${environment.apiURL}/api/nguoidung/nguoidung_nhom/getndnhom_bynsd`,
      {
        'prmJsonData': model
      } ,
      { withCredentials: true }
      );
  }

}
