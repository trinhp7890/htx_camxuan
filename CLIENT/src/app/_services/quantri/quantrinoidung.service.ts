import { HttpClient, HttpHeaders, HttpRequest, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Dantoc } from '@app/_models/dm/dantoc';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { FileToUpload } from '@app/_models/file-to-upload';

const baseUrl = `${environment.apiURL}/api/thongbao`;
const baseUrl_upload = `${environment.apiURL}/api/upload`;

@Injectable({
  providedIn: 'root'
})
export class QuantrinoidungService {

  constructor(private http: HttpClient) { }

  get_all(prmDK: number) {
    return this.http.get<any[]>(`${baseUrl}/getall`);
  }

  getfile_thongbao(prmMA_TB: string) {
    return this.http.get<Dantoc>(`${baseUrl}/getfilebyma?prmMA_TB=${prmMA_TB}`);
  }

  get_trangthai(trangthai: number) {
    return this.http.get<Dantoc>(`${baseUrl}/trang-thai/${trangthai}`);
  }


  chuyen_trangthai(prmID: number, prmNGUOI_CAPNHAT: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/trangthai?prmID=` + prmID + `&prmNGUOI_CAPNHAT=` + prmNGUOI_CAPNHAT,
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

  notifi(model: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/notifi`,
      {
        model
      },
      { withCredentials: true }
    );
  }
  thongbao_ins_upload(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl_upload}/thongbao_ins_upload`,
      formData
    );
  }
  insert_upload_thongbao(theFile: any, prmTIEUDE: string, prmNOIDUNG: string, prmTENFILE: string, prmLOAI: string, prmGHICHU: string, prmNGUOI_CAPNHAT: string, prmMOTA: string): Observable<any> {
    var urlupload = `${baseUrl_upload}/thongbao_ins_upload`
    const formData = new FormData();
    formData.append('file', theFile);
    formData.append('prmTIEUDE', prmTIEUDE);
    formData.append('prmNOIDUNG', prmNOIDUNG);
    formData.append('prmTENFILE', prmTENFILE);
    this.http.post(urlupload, formData)
      .subscribe(res => {
        console.log(res);

      });
    return;

    // const params = new HttpParams()
    // .set('prmTIEUDE', prmTIEUDE)
    // .set('prmNOIDUNG', prmNOIDUNG)
    // .set('prmTENFILE', prmTENFILE)
    // .set('prmLOAI', prmLOAI)
    // .set('prmGHICHU', prmGHICHU)
    // .set('prmNGUOI_CAPNHAT', prmNGUOI_CAPNHAT)
    // .set('prmMOTA', prmMOTA);
    // var httpOptions = {
    //   headers: new HttpHeaders({
    //       'Content-Type': 'application/json'
    //   }),
    //   params: params,
    //   withCredentials: true
    // };
    // console.log(theFile);
    // console.log(httpOptions);
    // const req = new HttpRequest('POST', urlupload, theFile, httpOptions);
    // return this.http.request(req);
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

  delete_thongbao(prmMA_TB: string, prmNGUOI_NHAP: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete?prmMA_TB=` + prmMA_TB + `&prmNGUOI_NHAP=` + prmNGUOI_NHAP,
      { withCredentials: true }
    );
  }

  //delete_thongbaofile
  delete_thongbaofile(prmIDFILE: string, prmNGUOI_NHAP: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/deletefile?prmIDFILE=` + prmIDFILE + `&prmNGUOI_NHAP=` + prmNGUOI_NHAP,
      { withCredentials: true }
    );
  }
}
