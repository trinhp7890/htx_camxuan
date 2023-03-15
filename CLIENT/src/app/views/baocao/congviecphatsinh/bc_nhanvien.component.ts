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
import { BaocaoService } from '@app/_services/Baocao/baocao.service';
// module realtime
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { DonviService } from "@app/_services/danhmuc/donvi.service";
import * as moment from 'moment';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-bc-nhanvien',
  templateUrl: './bc_nhanvien.component.html',
  styleUrls: ['./bc_donvi.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [DatePipe]
})

export class Bc_nhanvienComponent {
  constructor(
    // private dashboardService: DashboardService
    private modalService: BsModalService,
    private baocaoService: BaocaoService,
    private confirmService: ConfirmService,
    private toastr: ToastrService,
    private http: HttpClient,
    private donviService: DonviService,
    private datePipe: DatePipe
  ) { }
  public allowDragAndDrop: Boolean = false;

  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
  Ma_donvi = localStorage.getItem('Ma_donvi') ? localStorage.getItem('Ma_donvi') : sessionStorage.getItem('Ma_donvi') || '';
  serviceBase = `${environment.apiURL}`;

  modalRef: BsModalRef;
  hide_title = true;
  ngaybatdau_tk = '';
  ngayketthuc_tk = '';
  donvichutri_select = '';
  datadonvi = [];
  tinhchat = 0;
  data_baocao = [];
  title = 'angular-app';
  fileName = 'ExcelSheet.xlsx';

  ngOnInit(): void {
    this.get_danhsachdonvi();
    this.ngayketthuc_tk = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
    this.ngaybatdau_tk = moment(
      "01/" + (new Date().getMonth() + 1) + "/" + new Date().getFullYear(),
      'DD/MM/YYYY'
    ).format('YYYY-MM-DD');
    this.donvichutri_select = this.Ma_donvi;

  }
  xembaocao() {
    if (this.donvichutri_select == "" || this.donvichutri_select == null) {
      this.toastr.warning("Chưa chọn đơn vị", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    const model = {
      MA_DV: this.donvichutri_select,
      LOAI_CV: 3,
      // ID_DONVI: this.f.id_donvi.value,
      TUNGAY: moment(
        this.ngaybatdau_tk,
        'YYYY-MM-DD'
      ).format('DD/MM/YYYY'),
      DENNGAY: moment(
        this.ngayketthuc_tk,
        'YYYY-MM-DD'
      ).format('DD/MM/YYYY'),
      TINHCHAT: this.tinhchat
    };
    this.baocaoService.baocao_th_nhanvien(JSON.stringify(model)).subscribe({
      next: (_data) => {
        this.data_baocao = _data;
      },
      error: (error) => {
        this.toastr.error(error);
      },
    });
  }
  taifile() {
    if (this.donvichutri_select == "" || this.donvichutri_select == null) {
      this.toastr.warning("Chưa chọn đơn vị", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    //  /* pass here the table id */
    //  let element = document.getElementById('baocao-result');
    //  const ws: XLSX.WorkSheet =XLSX.utils.table_to_sheet(element);

    //  /* generate workbook and add the worksheet */
    //  const wb: XLSX.WorkBook = XLSX.utils.book_new();
    //  XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    //  /* save to file */  
    //  XLSX.writeFile(wb, "Báo cáo theo đơn vị.xlsx");

    this.baocaoService.baocao_th_nv_exp(this.donvichutri_select, moment(
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
        var fileName = "BaocaoTonghopDonvi.xlsx";
        var a = document.createElement("a");
        document.body.appendChild(a);
        a.href = fileURL;
        a.download = fileName;
        a.click();
      });

  }

  // lấy danh sách đơn vị giao việc
  get_danhsachdonvi() {
    //var madonvi = localStorage.getItem('Ma_donvi') ? localStorage.getItem('Ma_donvi') : sessionStorage.getItem('Ma_donvi') || '';
    this.donviService.get_donvilv3()
      .subscribe(
        _data => {
          this.datadonvi = _data;
        }
      );
  }
 
}
