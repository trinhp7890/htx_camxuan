import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap/modal";
import { ToastrService } from "ngx-toastr";
import { DantocService } from "@app/_services/danhmuc/dantoc.service";
import { GlobalConstants } from '@app/_models/config';
import { Query, DataManager, ODataV4Adaptor } from '@syncfusion/ej2-data';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClientModule } from '@angular/common/http';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { QuantrinoidungService } from '@app/_services/quantri/quantrinoidung.service';
import { FileToUpload } from '@app/_models/file-to-upload';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { DonviService } from "@app/_services/danhmuc/donvi.service";
import * as moment from 'moment';
@Component({
  selector: 'app-qlnhansu-Edit',
  templateUrl: './edit_nhansu.component.html'
})
export class Edit_nhansuComponent implements OnInit {
  @Input() title: string;
  @Input() data: any;


  @Output() event = new EventEmitter<boolean>();
  form: FormGroup;

  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
  Ma_donvi = localStorage.getItem('Ma_donvi') ? localStorage.getItem('Ma_donvi') : sessionStorage.getItem('Ma_donvi') || '';

  loading = false;
  submitted = false;
  ds_header: any = [];
  ilevels: any = [];
  level_header: any = [];
  level_max = 0;
  ds_data: any = [];
  countitem_data: any = [];
  itype = 0;
  filepreview = '';
  file: any = null;
  fileinput = '';
  fileattachs: any = [];
  danhsachfile: any = [];
  htmlContent = '';
  imagePreviewSrc: string | ArrayBuffer | null | undefined = '';
  isImageSelected: boolean = false;
  serviceBase = `${environment.apiURL}`;
  donvi_select = '';
  datadonvi = [];
  ngaycap_cmnd = '';
  ngaysinh = '';
  constructor(
    public modalRef: BsModalRef,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private quantriService: QuantrinoidungService,
    private http: HttpClient,
    private confirmService: ConfirmService,
    private donviService: DonviService,
  ) { }

  html: string;


  get f() { return this.form.controls; }

  ngOnInit() {
    this.form = this.formBuilder.group({
      ten_nd: [this.data.ten_nd, Validators.required],
      ma_nv: [this.data.ma_nv],
      ma_nd: [this.data.ma_nd],
      sodt: [this.data.so_dt],
      email: [this.data.email],
      ma_dv: [this.data.ma_dv],
      chucdanh: [this.data.chucdanh],
      gioitinh: [this.data.gioitinh],
      
      ngaysinh: [moment(
        this.data.ngaysinh,'DD/MM/YYYY').format('YYYY-MM-DD')],

      nguyenquan: [this.data.nguyenquan],
      so_cmnd: [this.data.so_cmnd],

      ngaycap_cmnd: [moment(
        this.data.ngaycap_cmnd,'DD/MM/YYYY').format('YYYY-MM-DD')],

      noicap_cmnd: [this.data.noicap_cmnd],
      attachfile: [],
      filepreview: [],
      fileSource: [],
    });
    if (this.data.duongdan_file != "" && this.data.duongdan_file != null && this.data.ten_file != null && this.data.ten_file != "") {
      this.imagePreviewSrc = this.serviceBase + "/" + this.data.duongdan_file + this.data.ten_file;
      this.isImageSelected = true;
    }
    this.donvi_select = this.data.ma_dv;
    this.get_danhsachdonvi();
  }


  onContentChanged = (event) => {
    //console.log(event.html);
  }

  public logValue(): void {
    const element = document.querySelector('.ql-editor');
    this.html = element.innerHTML;
  }

  public logForm(): void {
    setTimeout(() => {
      console.log(this.form);
      console.log(`DIRTY: ${this.form.dirty}`);
      console.log(`TOUCHED: ${this.form.touched}`);
    });
  }

  public blur(): void {
    console.log('blur');
  }

  public onSelectionChanged(): void {
    console.log('onSelectionChanged');
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
    if (this.f.ten_nd.value == "" || this.f.ten_nd.value == null) {
      this.toastr.warning("Chưa nhập tên nhân viên", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.ma_nv.value == "" || this.f.ma_nv.value == null) {
      this.toastr.warning("Chưa nhập Mã nhân viên", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.ma_nd.value == "" || this.f.ma_nd.value == null) {
      this.toastr.warning("Chưa nhập Username", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    console.log(this.f.ngaysinh.value);
    if (this.f.ngaysinh.value == "" || this.f.ngaysinh.value == null) {
      this.toastr.warning("Chưa nhập ngày sinh", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.gioitinh.value == "" || this.f.gioitinh.value == null) {
      this.toastr.warning("Chưa chọn giới tính", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.nguyenquan.value == "" || this.f.nguyenquan.value == null) {
      this.toastr.warning("Chưa nhập địa chỉ", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.so_cmnd.value == "" || this.f.so_cmnd.value == null) {
      this.toastr.warning("Chưa nhập số chứng minh/căn cước", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.ngaycap_cmnd.value == "" || this.f.ngaycap_cmnd.value == null) {
      this.toastr.warning("Chưa nhập ngày cấp số chứng minh/căn cước", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.noicap_cmnd.value == "" || this.f.noicap_cmnd.value == null) {
      this.toastr.warning("Chưa nhập nơi cấp số chứng minh/căn cước", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.sodt.value == "" || this.f.sodt.value == null) {
      this.toastr.warning("Chưa nhập số điện thoại", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.email.value == "" || this.f.email.value == null) {
      this.toastr.warning("Chưa nhập email", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.ma_dv.value == "" || this.f.ma_dv.value == null) {
      this.toastr.warning("Chưa chọn đơn vị", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.chucdanh.value == "" || this.f.chucdanh.value == null) {
      this.toastr.warning("Chưa nhập chức danh nhân viên", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    this.loading = true;
    const formData = new FormData();
    formData.append('file', this.form.get('fileSource').value);
    formData.append('prmMA_NV', this.data.ma_nv);
    formData.append('prmTEN_NV', this.f.ten_nd.value);
    formData.append('prmSODT', this.f.sodt.value);
    formData.append('prmEMAIL', this.f.email.value);
    formData.append('prmMA_DV', this.f.ma_dv.value);
    formData.append('prmCHUCDANH', this.f.chucdanh.value);
    formData.append('prmGHICHU', 'tạo ảnh mới');
    formData.append('prmNGUOI_CAPNHAT', this.UserName);

    formData.append('prmACTIVE', '1');
    formData.append('prmMA_ND', this.f.ma_nd.value);
    formData.append('prmNGAYSINH', moment(this.f.ngaysinh.value,'YYYY-MM-DD').format('DD/MM/YYYY'));
    formData.append('prmGIOITINH', this.f.gioitinh.value);
    formData.append('prmQUEQUAN', this.f.nguyenquan.value);
    formData.append('prmSO_CMND', this.f.so_cmnd.value);
    formData.append('prmNGAYCAP_CMND', moment(this.f.ngaycap_cmnd.value,'YYYY-MM-DD').format('DD/MM/YYYY'));
    formData.append('prmNOICAP_CMDN', this.f.noicap_cmnd.value);

    if (this.file != null) {
      formData.append('ISIMG', '1');
    }
    if(this.data == null){
      formData.append('prmLOAIEDIT', '1');
    }else{
      formData.append('prmLOAIEDIT', '0');
    }
    this.http.post(this.serviceBase + 'api/upload/nguoidung_upd_upload', formData)
      .subscribe({
        next: (_data) => {
          this.event.emit(true);
          this.modalRef.hide();
          this.toastr.success("Cập nhật thông báo thành công", "",
            {
              timeOut: 3000,
              closeButton: true,
              positionClass: 'toast-bottom-right'
            });
        },
        error: error => {
          this.toastr.error(error)
        }
      });
  }
  onChange(event) {
    this.file = event.target.files[0];
    if (this.file) {
      let fileReader = new FileReader();
      fileReader.readAsDataURL(this.file);

      fileReader.addEventListener('load', (event) => {
        this.imagePreviewSrc = event.target?.result;
        this.isImageSelected = true
      })
      this.form.patchValue({
        fileSource: event.target.files[0]
      });
    }
  }
  del_img() {
    if (this.file) {
      this.file = null;
      this.imagePreviewSrc = null;
      this.isImageSelected = false;
    }
  }
  attachfile(event) {
    if ((event.target.files.length + this.danhsachfile.length) > 5) {
      this.toastr.error('Số file đính kèm không được quá 5 files')
      return;
    }

    // kiem tra trung ten
    if (this.danhsachfile.length > 0) {
      for (let i = 0; i < event.target.files.length; i++) {
        for (let j = 0; j < this.danhsachfile.length; j++) {
          if (this.danhsachfile[j].name == event.target.files[i].name) {
            this.toastr.error('File vừa chọn đã tồn tại trong danh sách file đính kèm')
            return;
          }
        }
      }
    }

    for (let i = 0; i < event.target.files.length; i++) {
      this.fileattachs.push(event.target.files[i]);
      this.danhsachfile.push(event.target.files[i]);
    }
  }
  deletefile(datadel, indexfile) {
    if (typeof (datadel.id_file) == 'undefined') {
      this.fileattachs.splice(indexfile, 1);
      this.danhsachfile.splice(indexfile, 1)
    } else {
      let options = {
        prompt: 'Bạn có muốn xóa thông báo [' + datadel.ten_file + '] này không?',
        title: "Thông báo",
        okText: `Đồng ý`,
        cancelText: `Hủy`,
      };

      this.confirmService.confirm(options).then((res: boolean) => {
        if (res) {
          this.quantriService.delete_thongbaofile(datadel.id_file, "admin").subscribe({
            next: (_data) => {
              this.toastr.success("Xóa thành công", 'Thông báo', {
                timeOut: 3000,
                closeButton: true,
                positionClass: 'toast-bottom-right',
              });
              this.danhsachfile.splice(indexfile, 1)
            },
            error: (error) => {
              this.toastr.error(error);
            },
          });
        }
      });
    }
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

  get_filethongbao(matb): void {
    this.quantriService.getfile_thongbao(matb)
      .subscribe(
        _data => {
          this.danhsachfile = _data;
        }
      )
  }

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
}
