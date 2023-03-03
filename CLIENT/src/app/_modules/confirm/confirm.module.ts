import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ModalModule } from "ngx-bootstrap/modal";
import { ConfirmComponent } from "./confirm.component";

import { ConfirmService } from "./confirm.service";

@NgModule({
    imports: [ModalModule.forRoot()],
    declarations: [ConfirmComponent],
    exports: [ConfirmComponent],
    providers: [ConfirmService]
})

export class ConfirmModule {
}