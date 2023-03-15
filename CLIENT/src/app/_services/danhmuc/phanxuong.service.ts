import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/dmphanxuong`;
const baseUrlNS = `${environment.apiURL}/api/nguoidung`;

@Injectable({
  providedIn: 'root'
})
export class PhanxuongService {

  constructor(private http: HttpClient) { }

  get_all() {
      return this.http.get<any[]>(`${baseUrl}/getall`);
  }

  //DK = 1 GET ALL, = 0 GET KEM DIEU KIEN MA DON VI
  get_dieukien(prmDK: string, prmMA_DONVI : string) {
    return this.http.get<any[]>(`${baseUrl}/getnguonphatsinh?prmDK=` + prmDK+`&prmMA_DONVI=`+prmMA_DONVI);
  }
  
  chuyen_trangthai(prmID: number, prmNGUOI_CAPNHAT: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/trangthai?prmID=`+prmID+`&prmNGUOI_CAPNHAT=`+prmNGUOI_CAPNHAT,      
      { withCredentials: true }
    );
  }

  Del(prmMA_NGUON_PS: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete?prmMA_NGUON_PS=`+prmMA_NGUON_PS,      
      { withCredentials: true }
    );
  }

  nguonphatsinh_ins_upload(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/nguonphatsinh_ins_upload`,
      formData
    );
  }

  nguonphatsinh_up_upload(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/nguonphatsinh_up_upload`,
      formData
    );
  } 
}
