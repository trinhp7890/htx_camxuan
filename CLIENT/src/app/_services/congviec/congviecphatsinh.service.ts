import { HttpClient, HttpHeaders, HttpRequest, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { FileToUpload } from '@app/_models/file-to-upload';

const baseUrl = `${environment.apiURL}/api/congviecphatsinh`;
const baseUrl_thaoluan = `${environment.apiURL}/api/thaoluan`;

@Injectable({
  providedIn: 'root'
})
export class CongviecphatsinhService {

  constructor(private http: HttpClient) { }

  get_congviecgiao(prmMA_NV: string, prmUsername: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviecgiao?prmMA_NV=` + prmMA_NV + `&prmUsername=` + prmUsername);
  }
  get_congviecchutri(prmMA_NV: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviecchutri?prmMA_NV=` + prmMA_NV);
  }
  get_congviecphoihop(prmMA_NV: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviecphoihop?prmMA_NV=` + prmMA_NV);
  }
  get_congviecgiamsat(prmMA_NV: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviecgiamsat?prmMA_NV=` + prmMA_NV);
  }
  get_congviecchitiet(prmMA_CONGVIEC: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviecchitiet?prmMA_CONGVIEC=` + prmMA_CONGVIEC);
  }

  get_congviec_file(prmMA_CONGVIEC: string) {
    return this.http.get<any[]>(`${baseUrl}/getfilecongviec?prmMA_CONGVIEC=` + prmMA_CONGVIEC);
  }

  get_congviec_file_all(prmMA_CONGVIEC: string, prmMA_PHATSINH: string) {
    return this.http.get<any[]>(`${baseUrl}/getfilecongviec_all?prmMA_CONGVIEC=` + prmMA_CONGVIEC+ `&prmMA_PHATSINH=` + prmMA_PHATSINH);
  }
  
  get_congviec_nguoiphoihop(prmMA_CONGVIEC: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviecnguoiphoihop?prmMA_CONGVIEC=` + prmMA_CONGVIEC);
  }
  get_congviec_donviphoihop(prmMA_CONGVIEC: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviecdonviphoihop?prmMA_CONGVIEC=` + prmMA_CONGVIEC);
  }

  get_congviec_bydate(prmMA_NV: string, prmDK: string, prmNGAYBATDAU: string, prmNGAYKETTHUC: string) {
    return this.http.get<any[]>(`${baseUrl}/getcongviec_bydate?prmMA_NV=` + prmMA_NV + `&prmDK=` + prmDK + `&prmNGAYBATDAU=` + prmNGAYBATDAU + `&prmNGAYKETTHUC=` + prmNGAYKETTHUC);
  }

  Del(prmMA_CONGVIEC: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/delete?prmMA_CONGVIEC=`+prmMA_CONGVIEC,      
      { withCredentials: true }
    );
  }
  // getfile_thongbao(prmMA_TB: string) {
  //   return this.http.get<Dantoc>(`${baseUrl}/getfilebyma?prmMA_TB=${prmMA_TB}`);
  // }

  // get_trangthai(trangthai: number) {
  //   return this.http.get<Dantoc>(`${baseUrl}/trang-thai/${trangthai}`);
  // }


  // chuyen_trangthai(prmID: number, prmNGUOI_CAPNHAT: string): Observable<any> {
  //   return this.http.post(
  //     `${baseUrl}/trangthai?prmID=` + prmID + `&prmNGUOI_CAPNHAT=` + prmNGUOI_CAPNHAT,
  //     { withCredentials: true }
  //   );
  // }


  // insert(model: any): Observable<any> {
  //   return this.http.post(
  //     `${baseUrl}/create`,
  //     {
  //       'prmJsonData': model
  //     },
  //     { withCredentials: true }
  //   );
  // }

  // notifi(model: any): Observable<any> {
  //   return this.http.post(
  //     `${baseUrl}/notifi`,
  //     {
  //       model
  //     },
  //     { withCredentials: true }
  //   );
  // }
  congviec_ins_upload(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/congviec_ins_upload`,
      formData
    );
  }

  congviec_up_upload(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl}/congviec_up_upload`,
      formData
    );
  }

  
  up_trangthai(prmMA_CONGVIEC: string, prmTRANGTHAI: string, prmTYLE: string): Observable<any> {
    return this.http.post(
      `${baseUrl}/up_trangthai?prmMA_CONGVIEC=` + prmMA_CONGVIEC + `&prmTRANGTHAI=` + prmTRANGTHAI + `&prmTYLE=` + prmTYLE,
      { withCredentials: true }
    );
  }


  // thảo luận
  thaoluan_ins_upload(formData: any): Observable<any> {
    return this.http.post(
      `${baseUrl_thaoluan}/thaoluan_ins_upload`,
      formData
    );
  }

  get_thaoluan_bycongviec(prmMA_CONGVIEC: string) {
    return this.http.get<any[]>(`${baseUrl_thaoluan}/getbymacv?prmMA_CONGVIEC=` + prmMA_CONGVIEC);
  }

  // insert_upload_thongbao(theFile: any, prmTIEUDE: string, prmNOIDUNG: string, prmTENFILE: string, prmLOAI: string, prmGHICHU: string, prmNGUOI_CAPNHAT: string, prmMOTA: string): Observable<any> {
  //   var urlupload = `${baseUrl_upload}/thongbao_ins_upload`
  //   const formData = new FormData();
  //   formData.append('file', theFile);
  //   formData.append('prmTIEUDE', prmTIEUDE);
  //   formData.append('prmNOIDUNG', prmNOIDUNG);
  //   formData.append('prmTENFILE', prmTENFILE);
  //   this.http.post(urlupload, formData)
  //     .subscribe(res => {
  //       console.log(res);

  //     });
  //   return;

  //   // const params = new HttpParams()
  //   // .set('prmTIEUDE', prmTIEUDE)
  //   // .set('prmNOIDUNG', prmNOIDUNG)
  //   // .set('prmTENFILE', prmTENFILE)
  //   // .set('prmLOAI', prmLOAI)
  //   // .set('prmGHICHU', prmGHICHU)
  //   // .set('prmNGUOI_CAPNHAT', prmNGUOI_CAPNHAT)
  //   // .set('prmMOTA', prmMOTA);
  //   // var httpOptions = {
  //   //   headers: new HttpHeaders({
  //   //       'Content-Type': 'application/json'
  //   //   }),
  //   //   params: params,
  //   //   withCredentials: true
  //   // };
  //   // console.log(theFile);
  //   // console.log(httpOptions);
  //   // const req = new HttpRequest('POST', urlupload, theFile, httpOptions);
  //   // return this.http.request(req);
  // }
  // update(model: any): Observable<any> {
  //   return this.http.post(
  //     `${baseUrl}/update`,
  //     {
  //       'prmJsonData': model
  //     },
  //     { withCredentials: true }
  //   );
  // }

  // delete_thongbao(prmMA_TB: string, prmNGUOI_NHAP: string): Observable<any> {
  //   return this.http.post(
  //     `${baseUrl}/delete?prmMA_TB=` + prmMA_TB + `&prmNGUOI_NHAP=` + prmNGUOI_NHAP,
  //     { withCredentials: true }
  //   );
  // }

  // //delete_thongbaofile
  // delete_thongbaofile(prmIDFILE: string, prmNGUOI_NHAP: string): Observable<any> {
  //   return this.http.post(
  //     `${baseUrl}/deletefile?prmIDFILE=` + prmIDFILE + `&prmNGUOI_NHAP=` + prmNGUOI_NHAP,
  //     { withCredentials: true }
  //   );
  // }
}
