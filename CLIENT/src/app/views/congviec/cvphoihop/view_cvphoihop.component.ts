import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap/modal";
import { ToastrService } from "ngx-toastr";
import { DantocService } from "@app/_services/danhmuc/dantoc.service";
import { GlobalConstants } from '@app/_models/config';
import { Query, DataManager, ODataV4Adaptor } from '@syncfusion/ej2-data';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { Options } from '@angular-slider/ngx-slider';

@Component({
    selector: 'app-congvieccuatoi-view',
    templateUrl: './view_cvphoihop.component.html'
})
export class View_cvphoihopComponent implements OnInit {
    @Input() title: string;
    @Input() data: any;

    @Output() event = new EventEmitter<boolean>();

    form: FormGroup;
    loading = false;
    submitted = false;
    ds_header: any = [];
    ilevels : any = [];
    level_header: any = [];
    level_max = 0;
    ds_data : any = []; 
    countitem_data : any = [];
    itype = 0;

    //
    showthemnguoi = false;
    showfollwer = false;
    showbtngui =  false;
    showtraodoi = true;
    dataitems = [
        {id: 1, name: 'Trần Văn Thi'},
        {id: 2, name: 'Bùi Hương Đông'},
        {id: 3, name: 'Phan Trình'},
        {id: 4, name: 'Lê Việt Tuấn', disabled: true},
        {id: 5, name: 'Trần Khôi Nguyên'},
        {id: 6, name: 'Nguyễn Văn Nhật'},
        {id: 7, name: 'Thái Văn Tám'},
        {id: 8, name: 'Nguyễn Mạnh Ngọc'},
      ];
      selecteds = [
        2,8
      ];

      // Tiến độ
      sliderControl: FormControl = new FormControl(45);
      options: Options = {
        floor: 0,
        ceil: 100,
        step: 10,
        showTicks: true,
        translate: (value: number): string => {
            return value + '%';
          }
      }; 
    
    constructor(        
        public modalRef: BsModalRef,
        private toastr: ToastrService,
        private formBuilder: FormBuilder
    ) {}

    get f() { return this.form.controls; }

    ngOnInit() {
        console.log("Xem chi tiết");
        this.form = this.formBuilder.group({
             tencongviec: ['', Validators.required],
            ten_goikhac: [''],
            danhsachgiao: [],
        });       
    
    }   
   
    showAssignTosTask(){
        if(this.showthemnguoi){
            this.showthemnguoi =false;
        } else{
            this.showthemnguoi =true;
        }        
    }

    antraodoi(){
        if(!this.showtraodoi){           
            this.showtraodoi =true;
        }else{
            this.showtraodoi =false;
        }
    }

    hienbuttongui(){
        if(!this.showbtngui){           
            this.showbtngui =true;
        }
    }
    anbuttongui(){
        if(this.showbtngui){           
            this.showbtngui =false;
        }
    }
    guitraodoi(){
        if(this.showbtngui){           
            this.showbtngui =false;
        } 
    }
    //Hiển thị dữ liệu text sang dạng edit
 showEditTitle(e) {
	var permision = true;
    // kiểm tra quyền
	// for (var i = 0; i < taskRole.length; i++) {
	// 	if (permissonTask['edit_content'][taskRole[i]]) {
	// 		permision = true;
	// 		break;
	// 	}
	// }
    console.log('sửa title');
	if (permision) {
		$(e).hide();
		$(e).parents('.modal-title').find('input').val($(e).text().trim());
		$(e).parents('.modal-title').find('input').show();
		$(e).parents('.modal-title').find('input').focus();
        console.log(e);
	}
}

    showfollwerTosTask(){
        if(this.showfollwer){
            this.showfollwer =false;
        } else{
            this.showfollwer =true;
        }        
    }
    checknull(text):boolean {
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
            if(obj[key] === undefined) return hash; 
            return Object.assign(hash, { [obj[key]]:( hash[obj[key]] || [] ).concat(obj)})
          }, {})
     };
    checkiteminlist(aryy_, ilevel): boolean {
        for(let k = 0; k < aryy_.length; k ++){
            if (aryy_[k] == ilevel) {
                return false;
            }
        }      
        return true;
    }

    onSubmit(): void {
        console.log(this.selecteds);
        this.submitted = true;

        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        // var UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
        // const model = {
        //     'ID': this.f.id.value,
        //     'MA_DANTOC':this.f.ma_dantoc.value,
        //     'TEN_DANTOC':this.f.ten_dantoc.value,
        //     'TEN_GOIKHAC': this.f.ten_goikhac.value,
        //     'TRANGTHAI': this.f.trangthai.value,
        //     'NGUOI_CAPNHAT':UserName
        // };
        
    }
    attachfile(){
        console.log("attachfile");

    }
    closed(){
        this.event.emit(true);
        this.modalRef.hide();
    }    
}