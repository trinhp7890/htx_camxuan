import { Component, ViewEncapsulation } from '@angular/core';
import { Dashboard_CV_Chitiet } from '@app/_models/dashboard/dashboard';
import { AuthService } from '@app/_services';
import { DashboardService } from '@app/_services/dashboard/dashboard.service';
import { CardSettingsModel, SwimlaneSettingsModel, DialogSettingsModel, DialogEventArgs, DragEventArgs } from '@syncfusion/ej2-angular-kanban';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { Chart } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { BsModalService } from 'ngx-bootstrap/modal';
import { View_cvcuatoiComponent } from '../congviec/cuatoi/view_cvcuatoi.component';

// module realtime
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class DashboardComponent {

  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
 
  canvas: any;
  ctx: any;
  canvas2: any;
  ctx2: any;
  constructor(
    private modalService: BsModalService,
    private dashboardService: DashboardService
  ) { }
  ds_list_moi: any = [];
  ds_list_choxl: any = [];
  data_box: any = [];
  data_chitiet: any = [];
  tongso: number = 0;
  tongso_dagiao: number = 0;
  tongso_duocgiao: number = 0;
  quahan: number = 0;
  dangthuchien : number = 0;
  quahan_dagiao: number = 0;
  quahan_duocgiao: number = 0;
  chuathuchien: number = 0;
  chuathuchien_dagiao: number = 0;
  chuathuchien_duocgiao: number = 0;
  hoanthanh: number = 0;
  hoanthanh_dagiao: number = 0;
  hoanthanh_duocgiao: number = 0;
  list_quahan: any = [];
  list_dagiao: any = [];
  list_duocgiao: any = [];
  list_dagiao_bieudo: any = [];
  list_duocgiao_bieudo: any = [];
  totalItems = 0;
  term: string = '';
  p: number = 1;
  ngOnInit(): void {
    this.get_box();
    this.get_chitiet();
    this.get_bieudo();    
  }
  get_box(): void {
    this.dashboardService.get_box(this.Ma_nhanvien)
      .subscribe(
        _data => {
          this.data_box = _data;
          this.tongso = _data[0].tongso;
          this.quahan = _data[0].quahan;
          this.chuathuchien = _data[0].chuahoanthanh;
          this.dangthuchien = _data[0].dangthuchien;
          this.hoanthanh = _data[0].hoanthanh
          this.tongso_dagiao = _data[0].tongso_dagiao;
          this.tongso_duocgiao = _data[0].tongso_duocgiao;
          this.quahan_dagiao = _data[0].quahan_dagiao;
          this.quahan_duocgiao = _data[0].quahan_duocgiao;
          this.chuathuchien_dagiao = _data[0].chuahoanthanh_dagiao;
          this.chuathuchien_duocgiao = _data[0].chuahoanthanh_duocgiao;
          this.hoanthanh_dagiao = _data[0].hoanthanh_dagiao;
          this.hoanthanh_duocgiao = _data[0].hoanthanh_duocgiao
        }
      )
  }
  get_chitiet(): void {
    console.log(this.Ma_nhanvien)
    this.dashboardService.get_chitiet(this.Ma_nhanvien)
      .subscribe(
        _data => {
          this.list_quahan = _data.table;
          this.list_dagiao = _data.table1;
          this.list_duocgiao = _data.table2;
        }
      )
  }
  editContact(contact) {
    console.log(contact)
    //this.router.navigate([route], { queryParams: { id: contact.id } });
  }
  get_bieudo(): void {
    this.dashboardService.get_bieudo(this.Ma_nhanvien)
      .subscribe(
        _data => {
          this.list_dagiao_bieudo = _data.table;
          this.list_duocgiao_bieudo = _data.table1;
          this.canvas = document.getElementById('congviec_duocgiao');
          this.ctx = this.canvas.getContext('2d');
          this.canvas2 = document.getElementById('congviec_dagiao');
          this.ctx2 = this.canvas2.getContext('2d');
          let congviec_dagiao = new Chart(this.ctx2, {
            type: 'doughnut',
            data: {
              labels: ["Hoàn thành" + "(" + this.list_dagiao_bieudo[0].hoanthanh + ")", "Quá hạn" + "(" + this.list_dagiao_bieudo[0].quahan + ")", "Đang thực hiện" + "(" + this.list_dagiao_bieudo[0].dangthuchien + ")", "Chưa thực hiện" + "(" + this.list_dagiao_bieudo[0].chuathuchien + ")"],
              datasets: [{
                label: 'Tiến độ hoàn thành công việc',
                data: [this.list_dagiao_bieudo[0].hoanthanh, this.list_dagiao_bieudo[0].quahan, this.list_dagiao_bieudo[0].dangthuchien, this.list_dagiao_bieudo[0].chuathuchien],
                backgroundColor: ["green", "red", "#42A5F5", "#df6031"],
                datalabels: {
                  color: '#fff'
                }
              }]
            },
            options: {
              responsive: true,
              plugins: [ChartDataLabels],
              legend: {
                display: true,
                position: 'right',
              }
            }
          });
          let congviec_duocgiao = new Chart(this.ctx, {
            type: 'doughnut',
            data: {
              labels: ["Hoàn thành" + "(" + this.list_duocgiao_bieudo[0].hoanthanh + ")", "Quá hạn" + "(" + this.list_duocgiao_bieudo[0].quahan + ")", "Đang thực hiện" + "(" + this.list_duocgiao_bieudo[0].dangthuchien + ")", "Chưa thực hiện" + "(" + this.list_duocgiao_bieudo[0].chuathuchien + ")"],
              datasets: [{
                label: 'Tiến độ hoàn thành công việc',
                data: [this.list_duocgiao_bieudo[0].hoanthanh, this.list_duocgiao_bieudo[0].quahan, this.list_duocgiao_bieudo[0].dangthuchien, this.list_duocgiao_bieudo[0].chuathuchien],
                backgroundColor: ["green", "red", "#42A5F5", "#df6031"],
                datalabels: {
                  color: '#fff'
                }
              }]
            },
            options: {
              responsive: true,
              plugins: [ChartDataLabels],
              legend: {
                display: true,
                position: 'right',
              }
            },

          });
        }
      )
  }
  onSelect(value){
    console.log(value);
  }
}
