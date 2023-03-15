import { Component, ViewEncapsulation } from '@angular/core';
import { AuthService } from '@app/_services';
import { CardSettingsModel, SwimlaneSettingsModel, DialogSettingsModel, DialogEventArgs, DragEventArgs  } from '@syncfusion/ej2-angular-kanban';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { QuantrinoidungService } from '@app/_services/quantri/quantrinoidung.service';
import { environment } from '@environments/environment';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { ToastrService } from "ngx-toastr";
import { HttpClient, HttpUrlEncodingCodec } from '@angular/common/http';
// module realtime
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

import { Bc_donviComponent } from './bc_donvicomponent';

@Component({
  selector: 'app-bc_congviecphatsinh',
  templateUrl: './bc_congviecphatsinh.component.html',
  styleUrls: ['./bc_congviecphatsinh.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class Bc_congviecphatsinhComponent {
  constructor(
    // private dashboardService: DashboardService
    private modalService: BsModalService,
    private quantriService: QuantrinoidungService,
    private confirmService: ConfirmService,
    private toastr: ToastrService,
    private http: HttpClient
  ) { }
  public allowDragAndDrop: Boolean = false;

  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
  serviceBase = `${environment.apiURL}`; 

  modalRef: BsModalRef;
  hide_title = true;
  hide_menu = false;
  id_baocao = 1;
  
  ngOnInit(): void {
  }
  show_hide_menu(){
    if(this.hide_menu){
      this.hide_menu = false;
    }else{
      this.hide_menu = true;
    }
  }

  Hide_menu(){
   if(this.hide_menu){
      this.hide_menu = false;
    }
  }
  Baocao(item){
    if(this.hide_menu){
      this.hide_menu = false;
    }
    this.id_baocao = item;
  }
 
}
