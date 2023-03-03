import { CommonModule } from '@angular/common';
import { NgModule } from "@angular/core";
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { NgxPaginationModule } from 'ngx-pagination';
import { TreeModule } from '@circlon/angular-tree-component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';

import { AppDmRoutingModule } from './app.dm-routing.module';

import { InsDantocComponent } from './dantoc/ins.dantoc.component';
import { DantocComponent } from './dantoc/dantoc.component';
import { UpdDantocComponent } from './dantoc/upd.dantoc.component';
import { KanbanModule } from '@syncfusion/ej2-angular-kanban';




import { NumbersDirectivesModule } from '@app/_services/Directive/number.module';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AppDmRoutingModule,
        FormsModule,
        Ng2SearchPipeModule,
        NgxPaginationModule,
        TreeModule,
        TabsModule,
        NumbersDirectivesModule,
        KanbanModule
    ], 
    declarations: [                
        DantocComponent, InsDantocComponent, UpdDantocComponent
        ]
})
export class AppDmModulue { }