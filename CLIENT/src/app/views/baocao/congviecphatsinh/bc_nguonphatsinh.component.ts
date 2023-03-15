import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { AuthService } from '@app/_services';
import { CardSettingsModel, SwimlaneSettingsModel, DialogSettingsModel, DialogEventArgs, DragEventArgs } from '@syncfusion/ej2-angular-kanban';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { QuantrinoidungService } from '@app/_services/quantri/quantrinoidung.service';
import { environment } from '@environments/environment';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { ToastrService } from "ngx-toastr";
import { HttpClient, HttpUrlEncodingCodec } from '@angular/common/http';
import { BaocaoService } from '@app/_services/Baocao/baocao.service';
// module realtime
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { PhanxuongService } from "@app/_services/danhmuc/phanxuong.service";
import * as moment from 'moment';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-bc-nguonps',
  templateUrl: './bc_nguonphatsinh.component.html',
  styleUrls: ['./bc_donvi.component.scss'],
  providers: [DatePipe]
})

export class Bc_nguonphatsinhComponent implements OnInit {
  constructor(
    // private dashboardService: DashboardService
    private modalService: BsModalService,
    private baocaoService: BaocaoService,
    private confirmService: ConfirmService,
    private toastr: ToastrService,
    private http: HttpClient,
    private phanxuongService: PhanxuongService,
    private datePipe: DatePipe    
  ) { }
  public allowDragAndDrop: Boolean = false;
  @Input() title: string;
  @Input() data: any;

  public modalRef: BsModalRef;

  @Output() event = new EventEmitter<boolean>();
  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
  Ma_donvi = localStorage.getItem('Ma_donvi') ? localStorage.getItem('Ma_donvi') : sessionStorage.getItem('Ma_donvi') || '';
  serviceBase = `${environment.apiURL}`;

  hide_title = true;
  ngaybatdau_tk = '';
  ngayketthuc_tk = '';
  nguonphatsinh_select = '';
  danhsachnguonphatsinh = [];
  tinhchat = 0;
  data_baocao = [];
  ngOnInit(): void {
    this.get_nguonphatsinh();
    this.ngayketthuc_tk = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
    this.ngaybatdau_tk = moment(
      "01/" + (new Date().getMonth() + 1) + "/" + new Date().getFullYear(),
      'DD/MM/YYYY'
    ).format('YYYY-MM-DD');
    if(this.data == null ){
      this.hide_title = false;
    }else{
      this.nguonphatsinh_select = this.data.ma_nguonphatsinh;
      this.xembaocao();
    }
    

  }
  xembaocao() {
    if (this.nguonphatsinh_select == "" || this.nguonphatsinh_select == null) {
      this.toastr.warning("Chưa chọn nguồn phát sinh", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    const model = {
      NGUON_PHATSINH: this.nguonphatsinh_select,
      // ID_DONVI: this.f.id_donvi.value,
      TUNGAY: moment(
        this.ngaybatdau_tk,
        'YYYY-MM-DD'
      ).format('DD/MM/YYYY'),
      DENNGAY: moment(
        this.ngayketthuc_tk,
        'YYYY-MM-DD'
      ).format('DD/MM/YYYY')
    };
    this.baocaoService.baocao_nguonps(JSON.stringify(model)).subscribe({
      next: (_data) => {
        this.data_baocao = _data;
      },
      error: (error) => {
        this.toastr.error(error);
      },
    });
  }
  taifile() {

    //  /* pass here the table id */
    //  let element = document.getElementById('baocao-result');
    //  const ws: XLSX.WorkSheet =XLSX.utils.table_to_sheet(element);

    //  /* generate workbook and add the worksheet */
    //  const wb: XLSX.WorkBook = XLSX.utils.book_new();
    //  XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    //  /* save to file */  
    //  XLSX.writeFile(wb, "Báo cáo theo đơn vị.xlsx");

    this.baocaoService.baocaonguonps_exp(this.nguonphatsinh_select, moment(
      this.ngaybatdau_tk,
      'YYYY-MM-DD'
    ).format('DD/MM/YYYY'), moment(
      this.ngayketthuc_tk,
      'YYYY-MM-DD'
    ).format('DD/MM/YYYY')).subscribe(
      response => {
        var file = new Blob([response], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64'
        });
        var fileURL = window.URL.createObjectURL(file);
        var seconds = new Date().getTime() / 1000;
        var fileName = "BaocaoNguonphatsinh.xlsx";
        var a = document.createElement("a");
        document.body.appendChild(a);
        a.href = fileURL;
        a.download = fileName;
        a.click();
      });

  }

  // lấy danh sách đơn vị giao việc
  get_nguonphatsinh() {
    this.phanxuongService.get_dieukien('1', '0')
      .subscribe(
        _data => {
          this.danhsachnguonphatsinh = _data;
        }
      );
  }
  closed() {
    this.event.emit(true);
  }
}
