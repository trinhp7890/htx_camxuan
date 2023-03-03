import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Dantoc } from '@app/_models/dm/dantoc';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/dm/dantoc`;

@Injectable({
  providedIn: 'root'
})
export class DantocService {

  constructor(private http: HttpClient) { }

  get_all(prmDK: number) {
      return this.http.get<Dantoc[]>(`${baseUrl}?prmDK=` + prmDK);
  }

  get(ma: string) {
      return this.http.get<Dantoc>(`${baseUrl}/ma/${ma}`);
  }

  get_trangthai(trangthai: number) {
      return this.http.get<Dantoc>(`${baseUrl}/trang-thai/${trangthai}`);
  }


  chuyen_trangthai(prmID: number, prmNGUOI_CAPNHAT: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/trangthai?prmID=`+prmID+`&prmNGUOI_CAPNHAT=`+prmNGUOI_CAPNHAT,      
      { withCredentials: true }
    );
  }

  // chuyen_trangthai(Dantoc: Dantoc): Observable<any> {
  //   return this.http.post(
  //     `${baseUrl}/cap-nhat-trang-thai`,
  //     {
  //       'ma': Dantoc.MA,
  //       'trangthai': Dantoc.TRANGTHAI == 0 ? 1 : 0
  //     },
  //     { withCredentials: true }
  //   );
  // }

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
}
