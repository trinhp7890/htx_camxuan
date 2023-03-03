import { HttpClient, HttpHeaders, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
const baseUrl = `${environment.apiURL}/api/baocao`;

@Injectable({
  providedIn: 'root'
})
export class BaocaoService {

  constructor(private http: HttpClient) { }

  baocaodonvi(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/congviecdonvi`,
      {
        'prmJsonData': model
      },      
      { withCredentials: true },
    );
  }
  baocao_th_nhanvien(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/cv_tonghop_nv`,
      {
        'prmJsonData': model
      },      
      { withCredentials: true },
    );
  }

  baocao_nguonps(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/cv_theo_nguonps`,
      {
        'prmJsonData': model
      },      
      { withCredentials: true },
    );
  }

  baocaodonvi_exp( MA_DV: string,  LOAI_CV: number,TUNGAY: string, DENNGAY: string, TINHCHAT: number ) {
    return this.http.get(`${baseUrl}/congviecdonvi_exp?MA_DV=` + MA_DV + `&LOAI_CV=` + LOAI_CV + `&TUNGAY=` + TUNGAY + `&DENNGAY=` + DENNGAY + `&TINHCHAT=` + TINHCHAT,
      this.getDownloadOptions(),
      ).pipe(catchError(this.handleError));
  }

  baocao_th_nv_exp( MA_DV: string, TUNGAY: string, DENNGAY: string ) {
    return this.http.get(`${baseUrl}/cv_tonghop_nv_exp?MA_DV=` + MA_DV + `&TUNGAY=` + TUNGAY + `&DENNGAY=` + DENNGAY,
      this.getDownloadOptions(),
      ).pipe(catchError(this.handleError));
  }

  baocaonguonps_exp( NGUON_PHATSINH: string, TUNGAY: string, DENNGAY: string ) {
    return this.http.get(`${baseUrl}/cv_theo_nguonps_exp?NGUON_PHATSINH=` + NGUON_PHATSINH +  `&TUNGAY=` + TUNGAY + `&DENNGAY=` + DENNGAY,
      this.getDownloadOptions(),
      ).pipe(catchError(this.handleError));
  }
  getDownloadOptions() {
    var token = this.getToken();
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: token,
        'Cache-Control': 'no-cache',
        Pragma: 'no-cache'
      }),
      responseType: 'blob' as 'blob'    
    };

    return httpOptions;
  }
  getToken() {
    let accessToken = localStorage.getItem('token') ? localStorage.getItem('token') : sessionStorage.getItem('token') || '';
    if (accessToken !== '') {
      return `Bearer ${accessToken}`;
    } else {
      return '';
    }
  }
  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(`Backend returned code ${error.status}, ` + `body was: ${error.message}`);
    }
    return throwError(error);
  }
}
