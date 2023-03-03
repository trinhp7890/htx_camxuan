import { Component, ViewEncapsulation } from '@angular/core';
import { AuthService } from '@app/_services';
import { CardSettingsModel, SwimlaneSettingsModel, DialogSettingsModel, DialogEventArgs, DragEventArgs } from '@syncfusion/ej2-angular-kanban';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Ins_cvphoihopComponent } from './ins_cvphoihop.component';
import { View_cvcuatoiComponent } from '../cuatoi/view_cvcuatoi.component';
import { Options } from '@angular-slider/ngx-slider';
import { CongviecphatsinhService } from '@app/_services/congviec/congviecphatsinh.service';
import { environment } from '@environments/environment';
import { ToastrService } from "ngx-toastr";
import { RealtimeService } from '@app/_services/realtime.service';
import { Router } from '@angular/router';
import { ActivatedRoute, ParamMap } from '@angular/router';
@Component({
  selector: 'app-congviecduocgiao',
  templateUrl: './cvphoihop.component.html',
  styleUrls: ['./cvphoihop.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class CvphoihopComponent {

  constructor(
    // private dashboardService: DashboardService
    private modalService: BsModalService,
    private congviecPSService: CongviecphatsinhService,
    private toastr: ToastrService,
    private realtimeService: RealtimeService,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  macongviec_input = '';
  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
  serviceBase = `${environment.apiURL}`;
  ngaybatdau_tk = '';
  ngayketthuc_tk = '';

  ds_list_moi: any = [];
  ds_list_choxl: any = [];
  dang_xuli: number = 0;
  moi_tiepnhan: number = 0;
  xuli_dunghan: number = 0;
  xuli_trehan: number = 0;
  modalRef: BsModalRef;
  hide_title = true;

  options: Options = {
    floor: 0,
    ceil: 100,
    step: 10,
    showTicks: true,
    disabled: true,
    translate: (value: number): string => {
      return value + '%';
    }
  };


  public data: any[] = [];
  public data_search : any[] = [];
  public cardSettings: CardSettingsModel = {
    contentField: 'noidung',
    headerField: 'ma_congviec',
    showHeader: true
  };
  public allowDragAndDrop: Boolean = false;
  public swimlaneSettings: SwimlaneSettingsModel = {
    keyField: '',
  };
  dialogOpen(args: DialogEventArgs): void {
    args.cancel = true;
    this.editcard(args.data);
  }
  viewbyboard() {
    this.swimlaneSettings = {
      keyField: '',
    };
    console.log('Assignee');
  }

  viewbynhansu() {
    this.swimlaneSettings = {
      keyField: 'ngay_batdau',
      textField: 'ngay_batdau'
    };
  }

  ins_cvcuatoi() {
    const initialState = { title: "Thêm mới công việc", data: null };
    this.hide_title = false;
    this.modalRef = this.modalService.show(
      Ins_cvphoihopComponent,
      Object.assign({}, {
        animated: true, keyboard: false, backdrop: false, ignoreBackdropClick: true
      }, {
        class: 'modal-lg xlg', initialState
      }));

    this.modalRef.content.event
      .subscribe(arg => {
        if (arg) {
          //this.get_all();
          this.hide_title = true;
        }
      });
  }
  editcard(data) {
    const initialState = { title: "Chi tiết công việc", data: data };
    this.hide_title = false;
    this.modalRef = this.modalService.show(
      View_cvcuatoiComponent,
      Object.assign({}, {
        animated: true, keyboard: false, backdrop: false, ignoreBackdropClick: true
      }, {
        class: 'modal-lg xlg', initialState
      }));

    this.modalRef.content.event
      .subscribe(arg => {
        if (arg) {
          //this.get_all();
          this.macongviec_input = '';
          this.get_danhsachcongviecphoihop();
          this.hide_title = true;
        }
      });


  }
  public Delete(key) {
    console.log(key);
  }
  deleteCard(data) {
    console.log('delete');
    console.log(data);
  }
  onKanbanBDragStop(args: DragEventArgs) {
    console.log(args);
  };
  // public swimlaneSettings: SwimlaneSettingsModel = { keyField: 'Assignee' };
  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.macongviec_input = params.get('id')
    })
    this.get_danhsachcongviecphoihop();
    this.connect_realtime();
  }
  // lấy danh sách công việc chu tri
  get_danhsachcongviecphoihop() {
    this.congviecPSService.get_congviecphoihop(this.Ma_nhanvien)
      .subscribe(
        _data => {
          this.data = _data;
          this.data_search = _data;
          if(this.macongviec_input != '' && this.macongviec_input != null){            
            var congviecview = this.data.filter((x) => x.ma_congviec == this.macongviec_input);
            if (congviecview.length > 0) {
              this.editcard(congviecview[0]);
            }
          }
        }
      );
  }
  timkiemcongviec_bydate() {
    if (this.ngaybatdau_tk == '' || this.ngayketthuc_tk == '') {
      this.toastr.warning(
        'Vui lòng chọn ngày',
        'Cảnh báo',
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right',
        }
      );
      return;
    }
    if (this.ngaybatdau_tk > this.ngayketthuc_tk) {
      this.toastr.warning(
        'Ngày bắt đầu phải nhỏ hơn ngày kết thúc',
        'Cảnh báo',
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right',
        }
      );
      return;
    }
    this.congviecPSService.get_congviec_bydate(this.Ma_nhanvien, '3', this.ngaybatdau_tk, this.ngayketthuc_tk)
      .subscribe(
        _data => {
          this.data = _data;
          this.data_search = _data;
        }
      );
  }

  // realtime
  connect_realtime(): void {
    //this.realtimeService.connect_realtime(this.UserName);   
    try {
      this.realtimeService._hubConnection.on('Congviec_phatsinh', (message) => {
        this.get_danhsachcongviecphoihop();
        //this.getConnectionId();
      })
    } catch (e) {
      this.router.navigate(['/']);
    }
  }
  onSearchChange(event) {
    if (event == '' || event == null) {
      this.data = this.data_search;
    } else {
      this.data = this.data_search.filter((x) => x.ten_cv.toUpperCase().includes(event.toUpperCase()));
    }
  }
}
