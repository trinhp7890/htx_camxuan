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
@Component({
  selector: 'app-qlnhansu-Edit',
  templateUrl: './edit_nhansu.component.html'
})
export class Edit_nhansuComponent implements OnInit {
  @Input() title: string;
  @Input() data: any;


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
    private confirmService: ConfirmService,
  ) { }

  html: string;


  get f() { return this.form.controls; }

  ngOnInit() {    
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
    console.log(this.data);
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
