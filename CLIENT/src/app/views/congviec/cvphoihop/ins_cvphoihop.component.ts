import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap/modal";
import { ToastrService } from "ngx-toastr";
import { DantocService } from "@app/_services/danhmuc/dantoc.service";
import { GlobalConstants } from '@app/_models/config';
import { Query, DataManager, ODataV4Adaptor } from '@syncfusion/ej2-data';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';


@Component({
    selector: 'app-congviecduocgiao-Ins',
    templateUrl: './ins_cvphoihop.component.html'
})
export class Ins_cvphoihopComponent implements OnInit {
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
    giaothay = false;

    dataitems = [
        { id: 1, name: 'Trần Văn Thi' },
        { id: 2, name: 'Bùi Hương Đông' },
        { id: 3, name: 'Phan Trình' },
        { id: 4, name: 'Lê Việt Tuấn', disabled: true },
        { id: 5, name: 'Trần Khôi Nguyên' },
        { id: 6, name: 'Nguyễn Văn Nhật' },
        { id: 7, name: 'Thái Văn Tám' },
        { id: 8, name: 'Nguyễn Mạnh Ngọc' },
    ];

    dsuyquyens = [
        { id: 1, name: 'Nguyễn Nhật Quang' },
        { id: 2, name: 'Lê Ngọc Anh' },
        { id: 3, name: 'Phạm Tuấn Hùng' }
    ];

    nguoigiaoviec_select = [
        1
    ];

    nguoiphoihops_select = [
        1,6,8
    ];
    donviphoihop_select = [
        4, 6
    ];
    datadonvi = [
        { id: 1, name: 'Trung tâm Công nghệ Thông tin' },
        { id: 2, name: 'Trung tâm ĐHTT' },
        { id: 3, name: 'Phòng Nhân sự Tổng hợp' },
        { id: 4, name: 'Trung tâm VT Nam Sông Hương', disabled: true },
        { id: 5, name: 'Trung tâm VT Bắc Sông Hương' },
        { id: 6, name: 'Trung tâm VT Hương Phú' },
        { id: 7, name: 'Trung tâm VT Hương Điền' },
        { id: 8, name: 'Trung tâm VT Phong Điền' },
    ];
    donvichutri_select = [
        2
    ];
    nguoichutri_select = [
        2
    ];
    constructor(
        public modalRef: BsModalRef,
        private toastr: ToastrService,
        private formBuilder: FormBuilder
    ) { }

    get f() { return this.form.controls; }

    ngOnInit() {
        console.log("Thêm công việc");
        this.form = this.formBuilder.group({
            tencongviec: ['', Validators.required],
            ten_goikhac: [''],
            donvichutri: [],
            donviphoihop: [],
            nguoichutri: [],
            nguoiphoihop: [],
            nguoigiaoviec : [],
        });

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
    attachfile() {
        console.log("attachfile");

    }
    closed() {
        this.event.emit(true);
        this.modalRef.hide();
    }
    onCheckboxChange(e) {
        this.giaothay = e.target.value;
        console.log(this.giaothay);
        // var obj = {
        //     ID: e.target.value,
        // };
        // this.ds_loaihinh_donvi_gan.push(obj);
      
        if (e.target.checked) {           
            // Get danh sách ủy quyền giao việc
        }
    }
}