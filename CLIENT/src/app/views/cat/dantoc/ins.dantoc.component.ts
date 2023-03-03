import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DantocService } from '@app/_services/danhmuc/dantoc.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { GlobalConstants } from '@app/_models/config';

@Component({
    selector: 'app-dantoc-ins',
    templateUrl: './ins.dantoc.component.html'
})

export class InsDantocComponent implements OnInit {
    title: string;

    form: FormGroup;
    loading = false;
    submitted = false;

    @Output() event = new EventEmitter();
    constructor(
        private dantocService: DantocService,
        public modalRef: BsModalRef,
        private toastr: ToastrService,
        private formBuilder: FormBuilder
    ) { }

    get f() { return this.form.controls; }

    ngOnInit() {
        this.form = this.formBuilder.group({
            ma_dantoc: ['', Validators.required],
            ten_dantoc: ['', Validators.required],
            ten_goikhac: ['']
        });
    }

    onSubmit(): void {
        this.submitted = true;

        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        var UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
        const model = {
            'MA_DANTOC':this.f.ma_dantoc.value,
            'TEN_DANTOC':this.f.ten_dantoc.value,
            'TEN_GOIKHAC':this.f.ten_goikhac.value,
            'NGUOI_CAPNHAT':UserName
        };

        this.dantocService.insert(JSON.stringify(model))
            .subscribe({
                next: (_data) => {
                    this.toastr.success(GlobalConstants.success_addnew, "",
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