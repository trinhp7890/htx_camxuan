import { Component, OnInit } from '@angular/core';
import { Dantoc } from '@app/_models/dm/dantoc';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { DantocService } from '@app/_services/danhmuc/dantoc.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { first } from 'rxjs/operators';
import { InsDantocComponent } from './ins.dantoc.component';
import { UpdDantocComponent } from './upd.dantoc.component';
import { GlobalConstants } from '@app/_models/config';

@Component({
  selector: 'app-dantoc',
  templateUrl: './dantoc.component.html',
  styleUrls: ['./dantoc.component.scss']
})
export class DantocComponent implements OnInit {

  dantocs: Dantoc[] = [];
  totalItems = 0;
  term : string = '';
  p: number = 1;
  modalRef: BsModalRef;

  constructor(
    private dantocService: DantocService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private confirmService: ConfirmService
    ) { }

  ngOnInit(): void {
    this.get_all();
  }

  get_all(): void {
    this.dantocService.get_all(1)
      .subscribe(
        _data => {
          this.dantocs = _data;
          this.totalItems = _data.length;
        }
      )
  }

  toggle(dantoc: Dantoc) {
    let UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
    if (dantoc.ma_dantoc != '00') {
      this.dantocService.chuyen_trangthai(dantoc.id, UserName) //Sửa lại user sau
      .subscribe({
        next: (_data) => {
          this.toastr.success(GlobalConstants.success_toggle, "",
          {
            timeOut: 3000,
            closeButton: true,
            positionClass: 'toast-bottom-right'
          });
          this.get_all();
        },
        error: error => {
          this.toastr.error(error)
        }
      });
    }
    else {
      this.toastr.error(GlobalConstants.warn_dulieu_macdinh, "",
      {
        timeOut: 3000,
        closeButton: true,
        positionClass: 'toast-bottom-right'
      });
    };   
   }

  add() {
    const initialState = { title: GlobalConstants.THEMMOI };
    this.modalRef = this.modalService.show(
      InsDantocComponent,
      Object.assign({}, {
        animated: true, keyboard: false, backdrop: false, ignoreBackdropClick: true
      }, {
        class: 'modal-md', initialState
      }));

    this.modalRef.content.event
      .subscribe(arg => {
        if (arg) {
          this.get_all();
        }
      });
  }

  edit(dantoc: Dantoc) {
    const initialState = { title: GlobalConstants.DIEUCHINH, data: dantoc };
    this.modalRef = this.modalService.show(
      UpdDantocComponent,
      Object.assign({}, {
        animated: true, keyboard: false, backdrop: false, ignoreBackdropClick: true
      }, {
        class: 'modal-md', initialState
      }));

    this.modalRef.content.event
      .subscribe(arg => {
        if (arg) {
          this.get_all();
        }
      });
  }

   delete(dantoc: Dantoc) {
    
    let options = {
      prompt: `Bạn có muốn xóa đối tượng ${dantoc.ma_dantoc}` + ": " + `${dantoc.ten_dantoc} + không?`,
      title: GlobalConstants.title_hethong,
      okText:`Đồng ý`,
      cancelText:`Hủy`
    };

    this.confirmService.confirm(options).then((res: boolean) => {
      var UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
      if (res) {
        this.dantocService.delete(dantoc.id, UserName)
            .subscribe({
                next: (_data) => {
                  this.toastr.success(GlobalConstants.success_delete, "",
                  {
                    timeOut: 3000,
                    closeButton: true,
                    positionClass: 'toast-bottom-right'
                  });                    
                    this.get_all();
                  },
                  error: error => {
                    this.toastr.error(error)
                  }
            });
      }
    });

      
    
  }
}
