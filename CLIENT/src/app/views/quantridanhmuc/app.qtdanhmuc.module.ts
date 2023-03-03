import { CommonModule } from '@angular/common';
import { NgModule } from "@angular/core";
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { NgxPaginationModule } from 'ngx-pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';

import { AppDmRoutingModule } from './app.qtdanhmuc-routing.module';
import { KanbanModule } from '@syncfusion/ej2-angular-kanban';
import { BrowserModule } from '@angular/platform-browser';
import { NumbersDirectivesModule } from '@app/_services/Directive/number.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { TreeModule } from '@circlon/angular-tree-component';
import { QuillModule } from 'ngx-quill';
import { HttpClientModule} from '@angular/common/http';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { Edit_nhansuComponent } from './donvi/edit_nhansu.component';
import { Giamsat_nhansuComponent } from './donvi/giamsat_nhansu.component';
import { QldonviComponent } from './donvi/donvi.component';

import { Edit_nguonphatsinhComponent } from './nguonphatsinh/edit_nguonphatsinh.component';
import { Giamsat_nguonphatsinhComponent } from './nguonphatsinh/giamsat_nguonphatsinh.component';
import { NguonphatsinhComponent } from './nguonphatsinh/nguonphatsinh.component';

import { UyquyengiaoviecComponent } from './uyquyengiaoviec/uyquyengiaoviec.component';
import { Edit_uyquyengiaoviecComponent } from './uyquyengiaoviec/edit_uyquyengiaoviec.component';

import { NguoigiamsatComponent } from './nguoigiamsat/nguoigiamsat.component';
import { Edit_nguoigiamsatComponent } from './nguoigiamsat/edit_nguoigiamsat.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AppDmRoutingModule,
        FormsModule,
        Ng2SearchPipeModule,
        NgxPaginationModule,
        TabsModule,
        NumbersDirectivesModule,
        KanbanModule,
        NgSelectModule,
        HttpClientModule,
        AngularEditorModule,
        TreeModule,
        
    QuillModule.forRoot(),
    QuillModule
    ], 
    declarations: [
        // donvi
        QldonviComponent, Edit_nhansuComponent, Giamsat_nhansuComponent,
        // nguon phat sinh
        Edit_nguonphatsinhComponent, Giamsat_nguonphatsinhComponent, NguonphatsinhComponent, 
        Edit_uyquyengiaoviecComponent, UyquyengiaoviecComponent,
        Edit_nguoigiamsatComponent, NguoigiamsatComponent
        ]
})
export class AppqtdanhmucModulue { }