import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmComponent } from './confirm.component';

@Injectable()
export class ConfirmService {
  modalRef: BsModalRef;

  constructor(private modalService: BsModalService) { }

  confirm(options: any): Promise<any> {
    return new Promise((resolve, reject) => {
      this.modalRef = this.modalService.show(ConfirmComponent, Object.assign({}, { animated: true, keyboard: true, backdrop: false, ignoreBackdropClick: true },
        {
          class: 'modal-confirm'
        }));

      this.modalRef.content.title = options.title;
      this.modalRef.content.prompt = options.prompt;
      this.modalRef.content.okText = options.okText;
      this.modalRef.content.cancelText = options.cancelText;

      this.modalRef.content.onClose.subscribe((results: boolean) => {
          resolve(results);
        });
    });
  }
}
