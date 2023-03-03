import { CommonModule } from '@angular/common';
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from '@angular/forms';

import { GuestRoutingModule } from './guest-routing.module';
import { HelpComponent } from '@app/views/help/help.component';
import { LoginComponent } from '@app/views/login/login.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        GuestRoutingModule
    ],
    declarations: [
        LoginComponent,
        HelpComponent
    ]
})

export class GuestMoudle {}