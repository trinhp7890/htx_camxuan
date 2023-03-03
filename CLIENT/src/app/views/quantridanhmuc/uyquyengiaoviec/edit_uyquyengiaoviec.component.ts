import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap/modal";
import { ToastrService } from "ngx-toastr";
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { QuantrinoidungService } from '@app/_services/quantri/quantrinoidung.service';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { DonviService } from "@app/_services/danhmuc/donvi.service";
import { UyquyengiaoviecService } from "@app/_services/danhmuc/uyquyengiaoviec.service";
import { CongviecphatsinhService } from '@app/_services/congviec/congviecphatsinh.service';
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";

@Component({
  selector: 'app-qlnguonphatsinh-Edit',
  templateUrl: './edit_uyquyengiaoviec.component.html'
})
export class Edit_uyquyengiaoviecComponent implements OnInit {
  @Input() title: string;
  @Input() data: any;


  @Output() event = new EventEmitter<boolean>();
  form: FormGroup;
  loading = false;
  submitted = false;


  donviuyquyen_select = "";
  nguoiuyquyen_select = "";
  donviduocuyquyen_select = "";
  nguoiduocuyquyen_select = "";
  datanguoiuyquyen = [];
  datanguoiduocuyquyen = [];
  datadonvi = [];
  serviceBase = `${environment.apiURL}`;
  viewtrangthai = false;
  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
  constructor(
    public modalRef: BsModalRef,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private quantriService: QuantrinoidungService,
    private confirmService: ConfirmService,
    private donviService: DonviService,
    private uyquyengiaoviecService: UyquyengiaoviecService,
    private congviecPSService: CongviecphatsinhService,
  ) { }

  html: string;


  get f() { return this.form.controls; }

  ngOnInit() {
    console.log(this.data);
    this.get_danhsachdonvi();
    if (this.data == '0') {
      this.form = this.formBuilder.group({
        donviuyquyen: ['', Validators.required],
        nguoiuyquyen: ['', Validators.required],
        donviduocuyquyen: ['', Validators.required],
        nguoiduocuyquyen: ['', Validators.required]
      });
    } else {
      this.form = this.formBuilder.group({
        donviuyquyen: [this.data.dv_nguoiuyquyen, Validators.required],
        nguoiuyquyen: [this.data.ma_nv_uyquyen, Validators.required],
        donviduocuyquyen: [this.data.dv_nguoiduocuyquyen, Validators.required],
        nguoiduocuyquyen: [this.data.ma_nv_duocuyquyen, Validators.required]
      });
      this.donviuyquyen_select = this.data.dv_nguoiuyquyen;      
      this.donviduocuyquyen_select = this.data.dv_nguoiduocuyquyen;
      this.nguoiduocuyquyen_select = this.data.ma_nv_duocuyquyen;
      this.nguoiuyquyen_select = this.data.ma_nv_uyquyen;
      this.change_donvisduocuyquyen();
      this.change_donvisuyquyen();
      this.viewtrangthai = true;
    }
  }


  public logValue(): void {
    const element = document.querySelector('.ql-editor');
    this.html = element.innerHTML;
  }


  checknull(text): boolean {
    if (text == null || text == '' || text == undefined) {
      return true;
    }
    else {
      return false;
    }
  }
  groupByKey(array, key) {
    return array
      .reduce((hash, obj) => {
        if (obj[key] === undefined) return hash;
        return Object.assign(hash, { [obj[key]]: (hash[obj[key]] || []).concat(obj) })
      }, {})
  };
  checkiteminlist(aryy_, ilevel): boolean {
    for (let k = 0; k < aryy_.length; k++) {
      if (aryy_[k] == ilevel) {
        return false;
      }
    }
    return true;
  }

  onSubmit(): void {

    this.submitted = true;
    if (this.f.nguoiuyquyen.value == "" || this.f.nguoiuyquyen.value == null) {
      this.toastr.warning("Chưa chọn người ủy quyền", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.nguoiduocuyquyen.value == "" || this.f.nguoiduocuyquyen.value == null) {
      this.toastr.warning("Chưa người được ủy quyền", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }

    this.loading = true;
    if (this.data == '0') {
      this.uyquyengiaoviecService.insert(this.f.nguoiuyquyen.value, this.f.nguoiduocuyquyen.value).subscribe({
        next: (_data) => {
          this.event.emit(true);
          this.toastr.success("Thêm mới thành công", 'Thông báo', {
            timeOut: 3000,
            closeButton: true,
            positionClass: 'toast-bottom-right',
          });
        },
        error: (error) => {
          this.toastr.error(error);
        },
      });
    } else {
      this.uyquyengiaoviecService.update(this.data.id_uyquyen_gv, this.f.nguoiuyquyen.value, this.f.nguoiduocuyquyen.value, '0').subscribe({
        next: (_data) => {
          this.event.emit(true);
          this.toastr.success("Cập nhật thành công", 'Thông báo', {
            timeOut: 3000,
            closeButton: true,
            positionClass: 'toast-bottom-right',
          });
        },
        error: (error) => {
          this.toastr.error(error);
        },
      });
    }
    this.modalRef.hide();
  }

  closed() {
    this.event.emit(true);
    this.modalRef.hide();
  }

  editorConfigs: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: 'auto',
    minHeight: '0',
    maxHeight: 'auto',
    width: 'auto',
    minWidth: '0',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: true,
    placeholder: 'Enter text here...',
    defaultParagraphSeparator: '',
    defaultFontName: '',
    defaultFontSize: '',
    fonts: [
      { class: 'arial', name: 'Arial' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'calibri', name: 'Calibri' },
      { class: 'comic-sans-ms', name: 'Comic Sans MS' }
    ],
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText'
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
    uploadUrl: '"http://cskhhue-api.vnpthue.com.vn/api/upload/upload_anhnoidung"',
    uploadWithCredentials: false,
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['bold', 'italic'],
      ['fontSize']
    ]
  };
  // lấy danh sách đơn vị giao việc
  get_danhsachdonvi() {
    //var madonvi = localStorage.getItem('Ma_donvi') ? localStorage.getItem('Ma_donvi') : sessionStorage.getItem('Ma_donvi') || '';
    var madonvi = 'HUE000000';
    console.log(madonvi);
    this.donviService.get_donvilv3(madonvi)
      .subscribe(
        _data => {
          this.datadonvi = _data;
        }
      );
  }
  get_congviec_file() {
    // this.congviecPSService.get_congviec_file(this.data.ma_nguonphatsinh)
    //   .subscribe(
    //     _data => {
    //       this.danhsachfile = _data;
    //       console.log(_data);
    //     }
    //   );
  }
  change_donvisuyquyen(){
    this.donviService.get_nhansubydv(this.donviuyquyen_select)
            .subscribe(
                _data => {
                    this.datanguoiuyquyen = _data;
                }
            );
  }
  change_donvisduocuyquyen(){
    this.donviService.get_nhansubydv(this.donviduocuyquyen_select)
            .subscribe(
                _data => {
                    this.datanguoiduocuyquyen = _data;
                }
            );
  }
}
