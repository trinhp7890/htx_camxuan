import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap/modal";
import { ToastrService } from "ngx-toastr";

import { Dantoc } from "@app/_models/dm/dantoc";
import { DantocService } from "@app/_services/danhmuc/dantoc.service";
import { GlobalConstants } from '@app/_models/config';

@Component({
    selector: 'app-dantoc-upd',
    templateUrl: './upd.dantoc.component.html'
})
export class UpdDantocComponent implements OnInit {
    @Input() title: string;
    @Input() data: Dantoc;

    @Output() event = new EventEmitter<boolean>();

    form: FormGroup;
    loading = false;
    submitted = false;
    
    constructor(
        private dantocService: DantocService,
        public modalRef: BsModalRef,
        private toastr: ToastrService,
        private formBuilder: FormBuilder
    ) {}

    get f() { return this.form.controls; }

    ngOnInit() {
        this.form = this.formBuilder.group({
            id: [''],
            ma_dantoc: ['', Validators.required],
            ten_dantoc: ['', Validators.required],
            ten_goikhac: [''],
            trangthai: 0
        });
        // console.log(this.form.controls);
        this.form.patchValue({
            'id': this.data.id,
            'ma_dantoc': this.data.ma_dantoc,
            'ten_dantoc': this.data.ten_dantoc,
            'ten_goikhac': this.data.ten_goikhac,
            'trangthai': this.data.trangthai
        })
    }

    onSubmit(): void {
        this.submitted = true;

        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        var UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
        const model = {
            'ID': this.f.id.value,
            'MA_DANTOC':this.f.ma_dantoc.value,
            'TEN_DANTOC':this.f.ten_dantoc.value,
            'TEN_GOIKHAC': this.f.ten_goikhac.value,
            'TRANGTHAI': this.f.trangthai.value,
            'NGUOI_CAPNHAT':UserName
        };
        // // console.log(model);
        this.dantocService.update(JSON.stringify(model))
            .subscribe({
                next: (_data) => {
                    this.toastr.success(GlobalConstants.success_update, "",
                    {
                      timeOut: 3000,
                      closeButton: true,
                      positionClass: 'toast-bottom-right'
                    });
                    this.event.emit(true);
                    this.modalRef.hide();
                  },
                  error: error => {
                    this.toastr.error(error)
                  }
            });
    }
}