import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '@environments/environment';
import { Tinh } from '@app/_models/dm/tinh';
import { Huyen } from '@app/_models/dm/huyen';
import { Phuongxa } from '@app/_models/dm/phuongxa';

const baseUrl = `${environment.apiURL}/api/catalogs`;

@Injectable({
  providedIn: 'root'
})
export class TinhthanhService {

  constructor(private http: HttpClient) { }

  get_tinh() {
    return this.http.get<Tinh[]>(`${baseUrl}/tinh/getall`);
  }

  get_tinh_by_ma(ma: string) {
    return this.http.get<Tinh>(`${baseUrl}/tinh/get/${ma}`);
  }

  get_huyen(matinh: string) {
    return this.http.get<Huyen[]>(`${baseUrl}/huyen/matinh/${matinh}`);
  }

  get_phuongxa(mahuyen: string) {
    return this.http.get<Phuongxa[]>(`${baseUrl}/phuongxa/mahuyen/${mahuyen}`);
  }
  get_diachi(matinh: string, mahuyen: string, maphuongxa: string) {
    return this.http.get<any[]>(`${baseUrl}/get_diachi/?prmMATINH=` + matinh + `&prmMAHUYEN=` + mahuyen + `&prmMAPHUONGXA=` + maphuongxa);
  }
}
