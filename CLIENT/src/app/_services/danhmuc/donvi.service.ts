import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/dmdonvi`;
const baseUrlNS = `${environment.apiURL}/api/nguoidung`;

@Injectable({
  providedIn: 'root'
})
export class DonviService {

  constructor(private http: HttpClient) { }

  get_all() {
      return this.http.get<any[]>(`${baseUrl}/getAll`);
  }
  get_nhansubydv(prmMA_DV: string) {
    return this.http.get<any[]>(`${baseUrlNS}/getall_bydv?prmMA_DV=` + prmMA_DV);
  }
  get_donvigiaoviec(prmMA_DV: string) {
    return this.http.get<any[]>(`${baseUrl}/getdonvigiaoviec?prmMA_DV=` + prmMA_DV);
  }
  get_donvilv3(prmMA_DV: string) {
    return this.http.get<any[]>(`${baseUrl}/getdonviconlv3?prmMA_DV=` + prmMA_DV);
  }

  getbyma(prmMA_DV: string) {
      return this.http.get<any>(`${baseUrl}/prmMA_DV?prmMA_DV=`+prmMA_DV);
  }
  get(prmMA_DV: string) {
    return this.http.get<any>(`${baseUrl}/prmMA_DV?prmMA_DV=`+prmMA_DV);
  }
  get_tree(prmMA_DV: string) {
    return this.http.get<any>(`${baseUrl}/gettreebyma?prmMA_DV=`+prmMA_DV);
  }

  get_trangthai(trangthai: number) {
      return this.http.get<any>(`${baseUrl}/trang-thai/${trangthai}`);
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

  delete(model: any): Observable<any> {
    return this.http.post(
        `${baseUrl}/delete`,
        {
            'prmJsonData': model
        },
        { withCredentials: true }
    );
}
}
