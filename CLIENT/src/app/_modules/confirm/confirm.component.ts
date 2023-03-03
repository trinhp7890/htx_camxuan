import { Component, Input, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.scss']
})
export class ConfirmComponent implements OnInit {
  @Input() title: string = '';
  @Input() prompt: string = '';
  @Input() okText: string = '';
  @Input() cancelText: string = '';
  
  public onClose: Subject<boolean>;
  
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    this.onClose = new Subject<boolean>();
  }

  confirm(): void {
      this.onClose.next(true);
      this.bsModalRef.hide();
  }

  decline(): void {
      this.onClose.next(false);
      this.bsModalRef.hide();
  }
}
