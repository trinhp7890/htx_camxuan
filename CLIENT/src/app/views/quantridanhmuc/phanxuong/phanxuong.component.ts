import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { PhanxuongService } from '@app/_services/danhmuc/phanxuong.service';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { GlobalConstants } from '@app/_models/config';
import {Edit_phanxuongComponent  } from './edit_phanxuong.component';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-phanxuong',
  templateUrl: './phanxuong.component.html',
  styleUrls: ['./phanxuong.component.scss'],
  providers: [
  ]
})
export class PhanxuongComponent implements OnInit {
  donvis: any[];
  sonhansu: "10";
  totalItems = 0;
  term : string = '';
  p: number = 1;
  TreeNode: [];
  node: [];
  items: any;
  options = {
  };
  donvi = [];
  modalRef: BsModalRef;
  id_donvi: any;
  isDataAvailable: boolean = false;
  phanxuongs = [];
  serviceBase = `${environment.apiURL}`;
  type_view = false;  

  constructor(
    private phanxuongService: PhanxuongService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private confirmService: ConfirmService,

  ) { }

  ngOnInit(): void {
    this.getValueWithAsync().then(() =>
      this.isDataAvailable = true);      
  }

  async getValueWithAsync() {
    this.items = await this.get_all();    
    this.node = this.items;
  }

  get_all() { 
    return new Promise<any>((resolve) => {
      this.phanxuongService.get_all()
        .subscribe(
          _data => {
            this.phanxuongs = _data;     
                this.totalItems = _data.length;
            this.p = 1;
            console.log(this.phanxuongs)
          }
        );
    })
  }

  onInitialized(tree) {
    setTimeout(() => {
      tree.treeModel.expandAll();
    });
  }


  add() {
    console.log("add");
      const initialState = { title: GlobalConstants.THEMMOI + " nguồn phát sinh", data:'0' };
      this.modalRef = this.modalService.show(
        Edit_phanxuongComponent,
        Object.assign({}, {
          animated: true, keyboard: false, backdrop: false, ignoreBackdropClick: true
        }, {
          class: 'modal-lg xlg', initialState
        }));

      this.modalRef.content.event
        .subscribe(arg => {
          if (arg) {
            this.getValueWithAsync().then(() =>            
                this.isDataAvailable = true
            );
          }
        });
  }

  edit(nhansu) {    
    console.log("Edit");
      const initialState = { title: GlobalConstants.DIEUCHINH + " nhân sự", data:nhansu };
      this.modalRef = this.modalService.show(
        Edit_phanxuongComponent,
        Object.assign({}, {
          animated: true, keyboard: false, backdrop: false, ignoreBackdropClick: true
        }, {
          class: 'modal-lg xlg', initialState
        }));

      this.modalRef.content.event
        .subscribe(arg => {
          if (arg) {
            this.getValueWithAsync().then(() =>            
                this.isDataAvailable = true
            );
          }
        });
    
  }

  viewboard(){
    this.type_view = true;
    this.p = 0;
  }
  viewlist(){
    this.type_view = false;
    this.p = 0;
  }
  deletenguonphatsinh(datanguonp){
    let options = {
      prompt: 'Bạn có muốn xóa nguồn phát sinh [' + datanguonp.ten + '] này không?',
      title: "Thông báo",
      okText: `Đồng ý`,
      cancelText: `Hủy`,
    };

    this.confirmService.confirm(options).then((res: boolean) => {
      if (res) {
        this.phanxuongService.Del(datanguonp.ma_nguonphatsinh).subscribe({
          next: (_data) => {
            this.toastr.success("Xóa thành công", 'Thông báo', {
              timeOut: 3000,
              closeButton: true,
              positionClass: 'toast-bottom-right',
            });
            this.get_all();
          },
          error: (error) => {
            this.toastr.error(error);
          },
        });
        console.log('XÓA');
      }
    });
  }

}

function fuzzysearch(needle: string, haystack: string) {
  const haystackLC = haystack.toLowerCase();
  const needleLC = needle.toLowerCase();

  const hlen = haystack.length;
  const nlen = needleLC.length;

  if (nlen > hlen) {
    return false;
  }
  if (nlen === hlen) {
    return needleLC === haystackLC;
  }
  outer: for (let i = 0, j = 0; i < nlen; i++) {
    const nch = needleLC.charCodeAt(i);

    while (j < hlen) {
      if (haystackLC.charCodeAt(j++) === nch) {
        continue outer;
      }
    }
    return false;
  }
  return true;
}