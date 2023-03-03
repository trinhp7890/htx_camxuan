import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap/modal";
import { ToastrService } from "ngx-toastr";
import { DonviService } from "@app/_services/danhmuc/donvi.service";
import { NguonphatsinhService } from "@app/_services/danhmuc/nguonphatsinh.service";
import { QuantrinoidungService } from '@app/_services/quantri/quantrinoidung.service';
import { GlobalConstants } from '@app/_models/config';
import { Query, DataManager, ODataV4Adaptor } from '@syncfusion/ej2-data';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { UserService } from '@app/_services/sys/user.service';
import { CongviecphatsinhService } from '@app/_services/congviec/congviecphatsinh.service';
import { environment } from '@environments/environment';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { RealtimeService } from '@app/_services/realtime.service';
import { UyquyengiaoviecService } from '@app/_services/danhmuc/uyquyengiaoviec.service';
import * as moment from 'moment';
@Component({
    selector: 'app-congvieccuatoi-Ins',
    templateUrl: './ins_cvcuatoi.component.html'
})
export class Ins_cvcuatoiComponent implements OnInit {
    @Input() title: string;
    @Input() data: any;

    @Output() event = new EventEmitter<boolean>();
    serviceBase = `${environment.apiURL}`;
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
    fileattachs: any = [];
    giaothay = false;
    ngayketthuc: any;
    dataitems = [
    ];
    danhsachphoihop = [];
    nguoigiamsats = [
    ];
    nguoiphoihops_select = [
    ];
    donviphoihop_select = [];
    datadonvi = [];
    dsuyquyens = [       
    ];
    danhsachnguoichutri = [
    ];
    danhsachnguoiphoihop = [
    ];
    nguoigiaoviec_select = [
    ];
    donvichutri_select = '';
    nguoichutri_select = '';
    nguoigiamsat_select = '';
    nguonphatsinh_select = '';
    danhsachnguonphatsinh = [
    ];

    donviphoihop_select_in = [
    ];

    nguoiphoihops_select_in = [
    ];

    Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';
    UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
    constructor(
        public modalRef: BsModalRef,
        private toastr: ToastrService,
        private formBuilder: FormBuilder,
        private userService: UserService,
        private donviService: DonviService,
        private congviecPSService: CongviecphatsinhService,
        private quantrinoidungService: QuantrinoidungService,
        private nguonphatsinhService: NguonphatsinhService,
        private confirmService: ConfirmService,
        private realtimeService: RealtimeService,
        private uyquyengiaoviecService: UyquyengiaoviecService
    ) { }

    get f() { return this.form.controls; }

    ngOnInit() {
        if (this.data == null) {
            this.form = this.formBuilder.group({
                tencongviec: ['', Validators.required],
                noidungcongviec: [''],
                trichyeu: [''],
                ngaybatdau: [''],
                ngayketthuc: [''],
                donvichutri: [],
                donviphoihop: [],
                nguoichutri: [],
                nguoiphoihop: [],
                nguoigiaoviec: [],
                nguoigiamsat: [],
                nguonphatsinh: [],
                tinhchat: [],
                fileSource1: [],
                fileSource2: [],
                fileSource3: [],
                fileSource4: [],
                fileSource5: [],
            });
        } else {            
            if(this.data.nguoi_giao.trim() !=  this.Ma_nhanvien){
                
                this.form = this.formBuilder.group({
                    tencongviec: [this.data.ten_cv, Validators.required],
                noidungcongviec: [this.data.noidung],
                trichyeu: [''],
                ngaybatdau: moment(
                    this.data.ngay_batdau,
                    'DD/MM/YYYY'
                ).format('YYYY-MM-DD'),
                ngayketthuc: moment(
                    this.data.ngay_ketthuc,
                    'DD/MM/YYYY'
                ).format('YYYY-MM-DD'),
                donvichutri: [this.data.donvi_chutri],
                donviphoihop: [],
                nguoichutri: [],
                nguoiphoihop: [],
                nguoigiaoviec: [this.data.nguoi_giao],
                nguoigiamsat: [this.data.nguoi_giamsat],
                nguonphatsinh: [this.data.nguon_phatsinh],

                tinhchat: [this.data.tinh_chat],
                fileSource1: [],
                fileSource2: [],
                fileSource3: [],
                fileSource4: [],
                fileSource5: [],
                });
                this.giaothay = true;
                this.nguoigiaoviec_select = this.data.nguoi_giao;
            }else{
                this.form = this.formBuilder.group({
                    tencongviec: [this.data.ten_cv, Validators.required],
                    noidungcongviec: [this.data.noidung],
                    trichyeu: [''],
                    ngaybatdau: moment(
                        this.data.ngay_batdau,
                        'DD/MM/YYYY'
                    ).format('YYYY-MM-DD'),
                    ngayketthuc: moment(
                        this.data.ngay_ketthuc,
                        'DD/MM/YYYY'
                    ).format('YYYY-MM-DD'),
                    donvichutri: [this.data.donvi_chutri],
                    donviphoihop: [],
                    nguoichutri: [],
                    nguoiphoihop: [],
                    nguoigiaoviec: [],
                    nguoigiamsat: [this.data.nguoi_giamsat],
                    nguonphatsinh: [this.data.nguon_phatsinh],
    
                    tinhchat: [this.data.tinh_chat],
                    fileSource1: [],
                    fileSource2: [],
                    fileSource3: [],
                    fileSource4: [],
                    fileSource5: [],
                });
            }
            this.nguonphatsinh_select = this.data.nguon_phatsinh;
            this.donvichutri_select = this.data.donvi_chutri;
            this.nguoichutri_select = this.data.nguoi_chutri;
            this.nguoigiamsat_select = this.data.nguoi_giamsat;
            this.change_donvischutri();            
            //this.get_nguonphatsinh_file();
            this.get_congviec_file();
            this.congviecPSService.get_congviec_donviphoihop(this.data.ma_congviec)
                .subscribe(
                    _data => {
                        _data.forEach(element => {
                            this.donviphoihop_select_in.push(element.ma_dv);
                        });
                        this.donviphoihop_select = this.donviphoihop_select_in;
                        this.change_donviphoihop();
                    }
                );
            //
            this.congviecPSService.get_congviec_nguoiphoihop(this.data.ma_congviec)
                .subscribe(
                    _data => {
                        _data.forEach(element => {
                            this.nguoiphoihops_select_in.push(element.ma_nv_ph);
                        });
                        this.nguoiphoihops_select = this.nguoiphoihops_select_in;
                    }
                );
        }

        // lấy danh sách ủy quyền giao việc
        this.get_danhsachuyquyen();
        // lây danh sách người giám sát
        this.get_nguoigiamsat();
        // lấy danh sách đơn vị giao việc
        this.get_danhsachdonvi();
        // lấy danh sách nguồn phát sinh
        this.get_nguonphatsinh();


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

        if (this.f.tencongviec.value == "" || this.f.tencongviec.value == null) {
            this.toastr.warning("Chưa nhập tên công việc", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (this.f.nguonphatsinh.value == "" || this.f.nguonphatsinh.value == null) {
            this.toastr.warning("Chưa nhập nguồn phát sinh", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (this.f.tinhchat.value == "" || this.f.tinhchat.value == null) {
            this.toastr.warning("Chưa nhập độ ưu tiên", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (this.f.noidungcongviec.value == "" || this.f.noidungcongviec.value == null) {
            this.toastr.warning("Chưa nhập nội dung công việc", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (this.f.ngaybatdau.value == "" || this.f.ngaybatdau.value == null) {
            this.toastr.warning("Chưa nhập ngày bắt đầu", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (this.f.ngayketthuc.value == "" || this.f.ngayketthuc.value == null) {
            this.toastr.warning("Chưa nhập ngày kết thúc", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        
        if (this.giaothay == true && (this.f.nguoigiaoviec.value == "" || this.f.nguoigiaoviec.value == null)) {
            this.toastr.warning("Chưa chọn người giao việc", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (this.donvichutri_select == "" || this.donvichutri_select == null) {
            this.toastr.warning("Chưa chọn đơn vị chủ trì", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (this.nguoichutri_select == "" || this.nguoichutri_select == null) {
            this.toastr.warning("Chưa chọn người chủ trì", "Cảnh báo",
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                });
            return;
        }
        if (
            moment(this.f.ngaybatdau.value).format('YYYY-MM-DD') >
            moment(this.f.ngayketthuc.value).format('YYYY-MM-DD')
        ) {
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
        var crtime = new Date();
        if (
            moment(crtime).format('YYYY-MM-DD') >
            moment(this.f.ngaybatdau.value).format('YYYY-MM-DD')
        ) {
            this.toastr.warning(
                'Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại',
                'Cảnh báo',
                {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right',
                }
            );
            return;
        }

        for (let i = 0; i < this.fileattachs.length; i++) {
            if (this.fileattachs[i].size > 0) {
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
        }
        //
        var prmnguoiphoidhop = '';
        this.nguoiphoihops_select.forEach(element => {
            prmnguoiphoidhop = prmnguoiphoidhop + element + ',';
        });
        var prmdonviphoihop = '';
        this.donviphoihop_select.forEach(element => {
            prmdonviphoihop = prmdonviphoihop + element + ',';
        });

        prmnguoiphoidhop = prmnguoiphoidhop.substring(0, prmnguoiphoidhop.length - 1);
        prmdonviphoihop = prmdonviphoihop.substring(0, prmdonviphoihop.length - 1);

        this.loading = true;
        const formData = new FormData();
        formData.append('file', this.form.get('fileSource1').value);
        formData.append('file', this.form.get('fileSource2').value);
        formData.append('file', this.form.get('fileSource3').value);
        formData.append('file', this.form.get('fileSource4').value);
        formData.append('file', this.form.get('fileSource5').value);
        formData.append('prmTEN_CV', this.f.tencongviec.value);
        formData.append('prmNGUON_PHATSINH', this.f.nguonphatsinh.value);

        // trường hợp giao việc thay
        if (this.giaothay == true && this.f.nguoigiaoviec.value != "" && this.f.nguoigiaoviec.value != null) {
            formData.append('prmNGUOI_GIAO', this.f.nguoigiaoviec.value);
        }else{
            formData.append('prmNGUOI_GIAO', this.Ma_nhanvien);
        }

        formData.append('prmNOIDUNG', this.f.noidungcongviec.value);
        formData.append('prmDONVI_CHUTRI', this.donvichutri_select);
        formData.append('prmNGUOI_CHUTRI', this.nguoichutri_select);
        formData.append('prmMA_CONGVIEC_CHA', ''); // todo
        formData.append('prmMA_DUAN', ''); // todo
        formData.append('prmNGAY_BATDAU', this.f.ngaybatdau.value);
        formData.append('prmNGAY_KETTHUC', this.f.ngayketthuc.value);
        formData.append('prmNGUOI_PHOIHOP', prmnguoiphoidhop);
        formData.append('prmDONVI_PHOIHOP', prmdonviphoihop);
        formData.append('prmNGUOI_CAPNHAT', this.UserName);
        formData.append('prmTINH_CHAT', this.f.tinhchat.value);
        formData.append('prmNGUOI_GIAMSAT', this.nguoigiamsat_select);
        if (this.data == null) {
            this.congviecPSService.congviec_ins_upload(formData)
                .subscribe({
                    next: (_data) => {
                        this.event.emit(true);
                        this.modalRef.hide();
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
        } else {
            formData.append('prmMA_CV', this.data.ma_congviec);

            this.congviecPSService.congviec_up_upload(formData)
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
        var data={
            Type: 'Thông báo',
            Information: this.f.tencongviec.value,
            Id: this.nguoichutri_select + ',' + this.nguoigiamsat_select + ',' + prmnguoiphoidhop,
            Nguoigui: this.Ma_nhanvien,
            Nguoichutri: this.nguoichutri_select,
            Nguoiphoihop: this.nguoigiamsat_select,
            Nguoigiamsat: prmnguoiphoidhop
            
          }
        this.realtimeService.sendmsg_congviec(data);
    }
    closed() {
        this.event.emit(true);
        this.modalRef.hide();
    }
    onCheckboxChange(e) {

        if (e) {
            this.giaothay = true;
            // Get danh sách ủy quyền giao việc
        } else {
            this.giaothay = false;
        }
    }
    // lấy danh sách người giám sát công việc
    get_nguoigiamsat() {
        this.userService.getgiamsat()
            .subscribe(
                _data => {
                    this.nguoigiamsats = _data;
                }
            );
    }
    // lấy danh sách đơn vị giao việc
    get_danhsachdonvi() {
        var madonvi = localStorage.getItem('Ma_donvi') ? localStorage.getItem('Ma_donvi') : sessionStorage.getItem('Ma_donvi') || '';

        this.donviService.get_donvigiaoviec(madonvi)
            .subscribe(
                _data => {
                    this.datadonvi = _data;
                }
            );
    }
    // thay đổi đơn vị chủ trì
    change_donvischutri() {
        this.donviService.get_nhansubydv(this.donvichutri_select)
            .subscribe(
                _data => {
                    this.dataitems = _data;
                }
            );
    }
    // thay đổi đơn vị phối hợp
    change_donviphoihop() {
        if (this.donviphoihop_select.length > 0) {
            this.danhsachphoihop = [];
            for (var i = 0; i < this.donviphoihop_select.length; i++) {
                this.donviService.get_nhansubydv(this.donviphoihop_select[i])
                    .subscribe(
                        _data => {
                            _data.forEach(element => {
                                this.danhsachphoihop.push(element);
                            });
                        }
                    );
            }
        } else {
            this.danhsachphoihop = [];
        }

    }
    // đính kèm file
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
    // xóa file
    deletefile(file, indexfile) {

        if (typeof (file.id_file) == 'undefined') {
            this.fileattachs.splice(indexfile, 1);
        } else {
            let options = {
                prompt: 'Bạn có muốn xóa thông báo [' + file.name + '] này không?',
                title: "Thông báo",
                okText: `Đồng ý`,
                cancelText: `Hủy`,
            };

            this.confirmService.confirm(options).then((res: boolean) => {
                if (res) {
                    this.quantrinoidungService.delete_thongbaofile(file.id_file, this.UserName).subscribe({
                        next: (_data) => {
                            this.toastr.success("Xóa thành công", 'Thông báo', {
                                timeOut: 3000,
                                closeButton: true,
                                positionClass: 'toast-bottom-right',

                            });
                            this.fileattachs.splice(indexfile, 1);
                        },
                        error: (error) => {
                            this.toastr.error(error);
                        },
                    });
                }
            });
        }

    }

    // Get nguồn phát sinh
    get_nguonphatsinh() {
        this.nguonphatsinhService.get_dieukien('1', '0')
            .subscribe(
                _data => {
                    this.danhsachnguonphatsinh = _data;
                }
            );
    }

    get_nguonphatsinh_file() {
        if (this.nguonphatsinh_select == null) {
            this.fileattachs = [''];
            return;
        }
        let nguonps_select = this.danhsachnguonphatsinh.filter(i => i.ma_nguonphatsinh == this.nguonphatsinh_select);

        if (nguonps_select.length > 0) {
            this.form.patchValue({
                noidungcongviec: nguonps_select[0].noidung
            });
        }

        var fileattachs_temp = this.fileattachs;
        this.fileattachs = [];
        for (var i = 0; i < fileattachs_temp.length; i++) {
            if (fileattachs_temp[i].loai == 5)
                this.fileattachs.push(fileattachs_temp[i]);
        }

        this.congviecPSService.get_congviec_file(this.nguonphatsinh_select)
            .subscribe(
                _data => {
                    _data.forEach(element => {
                        this.fileattachs.push(element);
                    });
                }
            );
    }
    // get donvi phoi hop
    get_congviec_donviphoihop() {
        this.congviecPSService.get_congviec_donviphoihop(this.data.ma_congviec)
            .subscribe(
                _data => {
                    _data.forEach(element => {
                        this.donviphoihop_select.push(element.ma_dv);
                    });
                }
            );
    }

    get_congviec_file() {
        this.congviecPSService.get_congviec_file_all(this.data.ma_congviec, this.data.nguon_phatsinh)
            .subscribe(
                _data => {
                    this.fileattachs = _data;
                }
            );
    }

    get_danhsachuyquyen() {
        this.uyquyengiaoviecService.get_bymanv(this.Ma_nhanvien)
            .subscribe(
                _data => {
                    this.dsuyquyens = _data;
                }
            );
    }
}