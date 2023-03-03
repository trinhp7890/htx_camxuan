import { CommonModule } from '@angular/common';
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from '@angular/forms';
import { AuthorisedRoutingModule } from './authorised-routing.module';
import { KanbanModule } from '@syncfusion/ej2-angular-kanban';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { DashboardComponent } from '@app/views/dashboard/dashboard.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AuthorisedRoutingModule,
        KanbanModule,
        TabsModule
    ],
    declarations: [
        DashboardComponent,        
    ]
})

export class AuthorisedModule {}