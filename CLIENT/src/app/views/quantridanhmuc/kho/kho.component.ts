import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { KhoService } from '@app/_services/danhmuc/kho.service';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { GlobalConstants } from '@app/_models/config';
import {Edit_KhoComponent  } from './edit_kho.component';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-kho',
  templateUrl: './kho.component.html',
  styleUrls: ['./kho.component.scss'],
  providers: [
  ]
})
export class KhoComponent implements OnInit {
  donvis: any[];
  sokho: "10";
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
  khos = [];
  serviceBase = `${environment.apiURL}`;
  type_view = false;  

  constructor(
    private khoService: KhoService,
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
      this.khoService.get_all()
        .subscribe(
          _data => {
            this.khos = _data;     
                this.totalItems = _data.length;
            this.p = 1;
            console.log(this.khos)
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
      const initialState = { title: GlobalConstants.THEMMOI + " kho", data: '0' };
      this.modalRef = this.modalService.show(
        Edit_KhoComponent,
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

  edit(kho) {    
      const initialState = { title: GlobalConstants.DIEUCHINH + " kho", data:kho };
      this.modalRef = this.modalService.show(
        Edit_KhoComponent,
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
  deletekho(datadel){
    console.log(datadel)
    let options = {
      prompt: 'Bạn có muốn xóa kho [' + datadel['ma_kho'] + '] này không?',
      title: "Thông báo",
      okText: `Đồng ý`,
      cancelText: `Hủy`,
    };

    this.confirmService.confirm(options).then((res: boolean) => {
      if (res) {
        let input = {
          "ma_kho": datadel.ma_kho
        };
        this.khoService.Del(input).subscribe({
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