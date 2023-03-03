import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/dmuyquyengiaoviec`;

@Injectable({
  providedIn: 'root'
})
export class UyquyengiaoviecService {

  constructor(private http: HttpClient) { }

  get_all() {
      return this.http.get<any[]>(`${baseUrl}/GETALL`);
  }
  
  //DK = 1 GET ALL, = 0 GET KEM DIEU KIEN MA DON VI
  get_byid(prmID_UYQUYEN_GV: number) {
    return this.http.get<any[]>(`${baseUrl}/Getbyid?prmID_UYQUYEN_GV=` + prmID_UYQUYEN_GV);
  } 
  //DK = 1 GET ALL, = 0 GET KEM DIEU KIEN MA DON VI
  get_bymanv(prmMA_NV: string) {
    return this.http.get<any[]>(`${baseUrl}/GETBYMNV?prmMA_NV=` + prmMA_NV);
  } 
  //DK = 1 GET ALL, = 0 GET KEM DIEU KIEN MA DON VI
  get_dieukien(prmDK: string, prmMA_DONVI : string) {
    return this.http.get<any[]>(`${baseUrl}/getnguonphatsinh?prmDK=` + prmDK+`&prmMA_DONVI=`+prmMA_DONVI);
  }
  
  insert(prmMA_NV_UYQUYEN: string, prmMA_NV_DUOCUYQUYEN: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/UYQUYENGIAOVIEC_INS?prmMA_NV_UYQUYEN=` + prmMA_NV_UYQUYEN + `&prmMA_NV_DUOCUYQUYEN=` + prmMA_NV_DUOCUYQUYEN,      
      { withCredentials: true }
    );
  }
  update( prmID_UYQUYEN_GV: number, prmMA_NV_UYQUYEN: string, prmMA_NV_DUOCUYQUYEN: string, prmTRANGTHAI: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/UYQUYENGIAOVIEC_UP?prmID_UYQUYEN_GV=` + prmID_UYQUYEN_GV +  `&prmMA_NV_UYQUYEN=` + prmMA_NV_UYQUYEN + `&prmMA_NV_DUOCUYQUYEN=` + prmMA_NV_DUOCUYQUYEN + `&prmTRANGTHAI=` + prmTRANGTHAI,      
      { withCredentials: true }
    );
  }
  chuyen_trangthai(prmID: number, prmNGUOI_CAPNHAT: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/trangthai?prmID=`+prmID+`&prmNGUOI_CAPNHAT=`+prmNGUOI_CAPNHAT,      
      { withCredentials: true }
    );
  }

  Del(prmID_UYQUYEN_GV: number): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete?prmID_UYQUYEN_GV=`+prmID_UYQUYEN_GV,      
      { withCredentials: true }
    );
  }  
}
