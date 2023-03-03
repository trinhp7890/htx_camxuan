import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Gio_phutService {
  constructor(private http: HttpClient) { }

  load_gio() {
    var gio = [];
    var tt = '0';
    for (var i = 0; i < 24; i++) {
      if (i < 10) {
        var obj = {
          ma: tt + (i).toString(),
          ten: tt + (i).toString()
        }
      }
      else {
        var obj = {
          ma: (i).toString(),
          ten: (i).toString()
        }
      }
      gio.push(obj);
    }
    return gio;
  }

  load_phut() {
    var phut = [];
    var tt = '0';
    for (var i = 0; i <= 59; i++) {
      if (i < 10) {
        var obj = {
          ma: tt + (i).toString(),
          ten: tt + (i).toString()
        }
      }
      else {
        var obj = {
          ma: (i).toString(),
          ten: (i).toString()
        }
      }
      phut.push(obj);
    }
    return phut;
  }
}
