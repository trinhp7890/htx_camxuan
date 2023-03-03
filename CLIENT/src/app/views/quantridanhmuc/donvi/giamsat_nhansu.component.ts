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
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Chart } from 'chart.js';
import { DashboardService } from "@app/_services/dashboard/dashboard.service";
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
@Component({
  selector: 'app-qlnhansu-Edit',
  styleUrls: ['./donvi.component.scss'],
  templateUrl: './giamsat_nhansu.component.html'
})
export class Giamsat_nhansuComponent implements OnInit {
  @Input() title: string;
  @Input() data: any;
  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
  UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
 

  @Output() event = new EventEmitter<boolean>();
  form: FormGroup;
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
  constructor(
    public modalRef: BsModalRef,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private quantriService: QuantrinoidungService,
    private http: HttpClient,
    private dashboardService: DashboardService,
    private confirmService: ConfirmService,
  ) { }

  html: string;

  // giamsatcanhan
  canvas: any;
  ctx: any;
  canvas2: any;
  ctx2: any;
  ds_list_moi: any = [];
  ds_list_choxl: any = [];
  dang_xuli: number = 0;
  moi_tiepnhan: number = 0;
  xuli_dunghan: number = 0;
  xuli_trehan: number = 0;
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
  get f() { return this.form.controls; }

  ngOnInit() { 
    this.get_box();   
    this.get_chitiet();
    this.get_bieudo();
    this.form = this.formBuilder.group({
      ten_nd: [this.data.ten_nd, Validators.required],
      ma_nv: [this.data.ma_nv],
      sodt: [this.data.so_dt],
      email: [this.data.email],
      donvi: [this.data.ma_dv],
      chucdanh: [this.data.chucdanh],
      attachfile: [],
      filepreview: [],
      fileSource: [],
    });
    if (this.data.duongdan_file != "" && this.data.duongdan_file != null && this.data.ten_file != null && this.data.ten_file != "") {
      this.imagePreviewSrc = this.serviceBase + "/" + this.data.duongdan_file + this.data.ten_file;
      this.isImageSelected = true;
    }
  }

    get_box(): void {
    this.dashboardService.get_box(this.data.ma_nv)
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
    this.dashboardService.get_chitiet(this.data.ma_nv)
      .subscribe(
        _data => {
          this.list_quahan = _data.table;
          this.list_dagiao = _data.table1;
          this.list_duocgiao = _data.table2;
        }
      )
  }
  get_bieudo(): void {
    this.dashboardService.get_bieudo(this.data.ma_nv)
      .subscribe(
        _data => {
          this.list_dagiao_bieudo = _data.table;
          this.list_duocgiao_bieudo = _data.table1;
          this.canvas = document.getElementById('congviec_duocgiao_canhan');
          console.log(this.canvas)
          this.ctx = this.canvas.getContext('2d');
          this.canvas2 = document.getElementById('congviec_dagiao_canhan');
          this.ctx2 = this.canvas2.getContext('2d');
          let congviec_dagiao_canhan = new Chart(this.ctx2, {
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
          let congviec_duocgiao_canhan = new Chart(this.ctx, {
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
    console.log("123" );
    if (this.f.sodt.value == "" || this.f.sodt.value == null) {
      this.toastr.warning("Chưa nhập số điện thoại", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
        console.log("1333" );
      return;
    }
    console.log("234" );
    if (this.f.email.value == "" || this.f.email.value == null) {
      this.toastr.warning("Chưa nhập email", "Cảnh báo",
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
    formData.append('prmMA_DV', this.data.ma_dv);
    formData.append('prmCHUCDANH', this.f.chucdanh.value);
    formData.append('prmGHICHU', 'tạo ảnh mới');
    formData.append('prmNGUOI_CAPNHAT', 'admin');

    if (this.file != null) {
      formData.append('ISIMG', '1');
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
}
