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
import { HttpUrlEncodingCodec } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { RealtimeService } from '@app/_services/realtime.service';
@Component({
  selector: 'app-qlthongbao-Ins',
  templateUrl: './ins_qlthongbao.component.html'
})
export class Ins_qlthongbaoComponent implements OnInit {
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
  htmlContent = '';
  imagePreviewSrc: string | ArrayBuffer | null | undefined = '';
  isImageSelected: boolean = false;
  constructor(
    public modalRef: BsModalRef,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,
    private quantriService: QuantrinoidungService,
    private http: HttpClient,
    private realtimeService: RealtimeService
  ) { }

  html: string;


  get f() { return this.form.controls; }

  ngOnInit() {
    this.form = this.formBuilder.group({
      tieude: ['', Validators.required],
      ten_goikhac: [''],
      danhsachgiao: [],
      mota: [],
      attachfile: [],
      filepreview: [],
      htmlContent: [],
      fileSource: [],
      fileSource1: [],
      fileSource2: [],
      fileSource3: [],
      fileSource4: [],
      fileSource5: [],
    });

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

    if (this.f.tieude.value == "" || this.f.tieude.value == null) {
      this.toastr.warning("Chưa nhập tiêu đề thông báo", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.mota.value == "" || this.f.mota.value == null) {
      this.toastr.warning("Chưa nhập mô tả thông báo", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.f.htmlContent.value == "" || this.f.htmlContent.value == null) {
      this.toastr.warning("Chưa nhập nội dung thông báo", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }
    if (this.file == null) {
      this.toastr.warning("Chưa chọn ảnh", "Cảnh báo",
        {
          timeOut: 3000,
          closeButton: true,
          positionClass: 'toast-bottom-right'
        });
      return;
    }

    for (let i = 0; i < this.fileattachs.length; i++) {
      if (i == 0) this.form.patchValue({
        fileSource1: this.fileattachs[i]
      });
      if (i == 1) this.form.patchValue({
        fileSource2: this.fileattachs[i]
      });
      if (i == 2) this.form.patchValue({
        fileSource3: this.fileattachs[i]
      });
      if (i == 3) this.form.patchValue({
        fileSource4: this.fileattachs[i]
      });
      if (i == 4) this.form.patchValue({
        fileSource5: this.fileattachs[i]
      });
    }

    this.loading = true;
    const formData = new FormData();
    formData.append('file', this.form.get('fileSource').value);
    formData.append('file', this.form.get('fileSource1').value);
    formData.append('file', this.form.get('fileSource2').value);
    formData.append('file', this.form.get('fileSource3').value);
    formData.append('file', this.form.get('fileSource4').value);
    formData.append('file', this.form.get('fileSource5').value);
    formData.append('prmTIEUDE', this.f.tieude.value);
    formData.append('prmMOTA', this.f.mota.value);
    formData.append('prmGHICHU', 'tạo ảnh mới');
    formData.set('prmNOIDUNG', this.f.htmlContent.value);
    if (this.file != null) {
      formData.append('ISIMG', '1');
    }

    this.quantriService.thongbao_ins_upload(formData)
      .subscribe({
        next: (_data) => {
          this.event.emit(true);
          this.modalRef.hide();
          this.notifi();
          this.toastr.success("Thêm mới thông báo thành công", "",
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
  notifi() {
    var obj = {
      "priority": "high",
      "notification": {
        "title": this.f.tieude.value,
        "body": this.f.mota.value,
        "sound": "default"
      },
      'to': "/topics/vnpts"
    };

    this.quantriService.notifi(obj)
      .subscribe({
        next: (_data) => {
        },
        error: error => {
          this.toastr.error(error)
        }
      });

      var data={
        Type: 'Thông báo',
        Information: this.f.tieude.value,
        Id: this.f.mota.value
      }
      
      console.log(data);
      this.realtimeService.sendmsg(data);
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
    if ((this.fileattachs.length + event.target.files.length) > 5) {
      this.toastr.error('Số file đính kèm không được quá 5 files')
      return;
    }
    // kiem tra trung ten
    for (let i = 0; i < event.target.files.length; i++) {
      if (this.fileattachs.length > 0)
        for (let j = 0; j < this.fileattachs.length; j++) {
          if (this.fileattachs[j].name == event.target.files[i].name) {
            this.toastr.error('File vừa chọn đã tồn tại trong danh sách file đính kèm')
            return;
          }
        }
    }

    for (let i = 0; i < event.target.files.length; i++) {
      this.fileattachs.push(event.target.files[i]);
    }
  }
  deletefile(indexfile) {
    this.fileattachs.splice(indexfile, 1)
  }

  closed() {
    this.event.emit(true);
    this.modalRef.hide();
  }

  editorConfig: AngularEditorConfig = {
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
    uploadWithCredentials: false,
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['bold', 'italic'],
      ['fontSize']
    ]
  };
}
