import { Component, OnInit, Injectable, ViewChild } from '@angular/core';
import { DonviService } from '@app/_services/danhmuc/donvi.service';
import { ToastrService } from 'ngx-toastr';
import { FormGroup } from '@angular/forms';
import { TreeNode, TreeModel } from '@circlon/angular-tree-component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmService } from '@app/_modules/confirm/confirm.service';
import { GlobalConstants } from '@app/_models/config';
import {Edit_nhansuComponent  } from './edit_nhansu.component';
import { Chart } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { environment } from '@environments/environment';
import { Giamsat_nhansuComponent } from './giamsat_nhansu.component';
import { DashboardService } from '@app/_services/dashboard/dashboard.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-donvi',
  templateUrl: './donvi.component.html',
  styleUrls: ['./donvi.component.scss'],
  providers: [
  ]
})
export class QldonviComponent implements OnInit {
  Ma_donvi = localStorage.getItem('Ma_donvi') ? localStorage.getItem('Ma_donvi') : sessionStorage.getItem('Ma_donvi') || '';
  
  donvis: any[];
  sonhansu: "10";
  totalItems = 0;
  countChild = 0;
  term : string = '';
  p: number = 1;
  TreeNode: [];
  node: [];
  items: any;
  options = {
  };
  donvi = [];
  modalRef: BsModalRef;
  id_donvi: any ;
  isDataAvailable: boolean = false;
  nhansus = [];
  serviceBase = `${environment.apiURL}`;

  // giam sat don vi 
  canvas: any;
  ctx: any;
  canvas1: any;
  ctx1: any;
  canvas2: any;
  ctx2: any;
  canvas3: any;
  ctx3: any;
  canvas4: any;
  ctx4: any;
  ds_list_moi: any = [];
  ds_list_choxl: any = [];
  list_quahan: any = [];
  list_duocgiao: any = [];
  data_box: any = [];
  dang_xuli: number = 0;
  moi_tiepnhan: number = 0;
  xuli_dunghan: number = 0;
  xuli_trehan: number = 0;
  tongso: number = 0;
  quahan: number = 0;
  dangthuchien : number = 0;
  chuathuchien: number = 0;
  hoanthanh: number = 0;
  constructor(
    private donviService: DonviService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private confirmService: ConfirmService,
    private dashboardService: DashboardService
  ) { }

  ngOnInit(): void {
    this.id_donvi = this.Ma_donvi;
    this.getValueWithAsync().then(() =>
      this.isDataAvailable = true);

      // giam sat don vị
      this.canvas = document.getElementById('tientrinh_hoanthanh');
    this.ctx = this.canvas.getContext('2d');
    this.canvas1 = document.getElementById('tiendo_nhanvien');
    this.ctx1 = this.canvas1.getContext('2d');
    this.canvas2 = document.getElementById('tiendo_thuchien');
    this.ctx2 = this.canvas2.getContext('2d');
  }

  async getValueWithAsync() {
    this.items = await this.get_all();    
    this.node = this.items;
  }

  get_all() {
    return new Promise<any>((resolve) => {
      this.donviService.get_tree("HUE000000")
        .subscribe(
          _data => {
            resolve(_data);         
          }
        );
    })
  }
  get_box(): void {
    this.dashboardService.get_box_donvi(this.id_donvi,this.countChild)
      .subscribe(
        _data => {
          this.data_box = _data;
          this.tongso = _data[0].tongso;
          this.quahan = _data[0].quahan;
          this.chuathuchien = _data[0].chuahoanthanh;
          this.dangthuchien = _data[0].dangthuchien;
          this.hoanthanh = _data[0].hoanthanh
        }
      )
  }
  get_chitiet_donvi(): void {
    console.log(this.id_donvi+"|"+this.countChild)
    this.dashboardService.get_chitiet_donvi(this.id_donvi,this.countChild)
      .subscribe(
        _data => {
          this.list_quahan = _data.table;
          this.list_duocgiao = _data.table1;
        }
      )
  }
  onInitialized(tree) {
    setTimeout(() => {
      tree.treeModel.expandAll();
    });
  }

  onEvent(node: TreeNode): void {
   this.id_donvi = node.data.id;
   this.countChild = node.data.children.length;
   this.donviService.get_nhansubydv(this.id_donvi)
   .subscribe(
     _data => {
      this.nhansus = _data;   
      this.totalItems = _data.length;
      this.p = 1;
      this.get_box();
      this.get_chitiet_donvi()
     }
   );
  }
  onSelect(value){
    console.log(value);
  }

  filterFn(value: string, treeModel: TreeModel) {
    treeModel.filterNodes((node: TreeNode) => fuzzysearch(value, node.data.ten_dv));
  }

  get_info(id) {
    this.donvi = [];
    this.donviService.get(id)
      .subscribe(
        _data => {
          this.donvi.push(_data);
        }
      );
  }

  add() {
    // if (this.id_donvi == 0) {
    //   this.toastr.warning(GlobalConstants.warn_missing_donvi, "",
    //     {
    //       timeOut: 3000,
    //       closeButton: true,
    //       positionClass: 'toast-bottom-right'
    //     });
    //   return;
    // }
    // else {
    //   const initialState = { title: GlobalConstants.THEMMOI, data: this.id_donvi };
    //   this.modalRef = this.modalService.show(
    //     InsDonviComponent,
    //     Object.assign({}, {
    //       animated: true, keyboard: false, backdrop: false, ignoreBackdropClick: true
    //     }, {
    //       class: 'modal-md', initialState
    //     }));

    //   this.modalRef.content.event
    //     .subscribe(arg => {
    //       if (arg) {
    //         this.getValueWithAsync().then(() =>
    //             this.isDataAvailable = true);
    //       }
    //     });
    // }
  }

  edit(nhansu) {    
    console.log("Edit");
      const initialState = { title: GlobalConstants.DIEUCHINH + " nhân sự", data:nhansu };
      this.modalRef = this.modalService.show(
        Edit_nhansuComponent,
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
            this.donviService.get_nhansubydv(this.id_donvi)
              .subscribe(
                _data => {
                  this.nhansus = _data;
                  this.totalItems = _data.length;
                  this.p = 1;
                  console.log(_data);
                }
              );
          }
        });
    
  }

  giamsat(nhansu) {    
  
    console.log("Giám sát");
    const initialState = { title: "Chi tiết công việc " + nhansu.ten_nd, data:nhansu };
    this.modalRef = this.modalService.show(
      Giamsat_nhansuComponent,
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
          this.donviService.get_nhansubydv(this.id_donvi)
            .subscribe(
              _data => {
                this.nhansus = _data;
                this.totalItems = _data.length;
                this.p = 1;
                console.log(_data);
              }
            );
        }
      });
  
}

  // delete() {
  //   var UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
  //   if (this.donvi.length == 0) {
  //     this.toastr.warning(GlobalConstants.warn_missing_donvi, "",
  //       {
  //         timeOut: 3000,
  //         closeButton: true,
  //         positionClass: 'toast-bottom-right'
  //       });
  //     return;
  //   }
  //   else {
  //     let ans = confirm("Bạn có muốn xóa đơn vị " + this.donvi[0][0].ten_donvi + " không?");
  //     if (ans) {
  //       this.donviService.delete(this.donvi[0][0].id, UserName)
  //         .subscribe({
  //           next: (_data) => {
  //             this.toastr.success(GlobalConstants.success_delete, "",
  //               {
  //                 timeOut: 3000,
  //                 closeButton: true,
  //                 positionClass: 'toast-bottom-right'
  //               });
  //               this.getValueWithAsync().then(() =>
  //               this.isDataAvailable = true);
  //           },
  //           error: error => {
  //             this.toastr.error(error)
  //           }
  //         });
  //     };
  //   }
  // }

  delete() {
    let options = {
      prompt: `Bạn có muốn xóa đối tượng ${this.donvi[0][0].ten_donvi} không?`,
      title: GlobalConstants.title_hethong,
      okText: `Đồng ý`,
      cancelText: `Hủy`
    };

    // this.confirmService.confirm(options).then((res: boolean) => {
    //   if (res) {
    //     var UserName = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
    //     this.donviService.delete(this.donvi[0][0].id, UserName) //Sẽ sửa lại thành user sau
    //       .subscribe({
    //         next: (_data) => {
    //           this.toastr.success(GlobalConstants.success_delete, "",
    //             {
    //               timeOut: 3000,
    //               closeButton: true,
    //               positionClass: 'toast-bottom-right'
    //             });
    //           //this.get_all();
    //           this.getValueWithAsync().then(() =>
    //              this.isDataAvailable = true);
    //         },
    //         error: error => {
    //           this.toastr.error(error)
    //         }
    //       });
    //   }
    // });
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