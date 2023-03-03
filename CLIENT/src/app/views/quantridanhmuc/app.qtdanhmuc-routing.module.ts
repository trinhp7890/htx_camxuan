import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { QldonviComponent } from "./donvi/donvi.component";
import { NguonphatsinhComponent } from "./nguonphatsinh/nguonphatsinh.component";
import { UyquyengiaoviecComponent } from "./uyquyengiaoviec/uyquyengiaoviec.component";
import { NguoigiamsatComponent } from "./nguoigiamsat/nguoigiamsat.component";




const routes: Routes = [           
    { path: 'donvi',              component: QldonviComponent },
    { path: 'nguonphatsinh',              component: NguonphatsinhComponent },
    { path: 'uyquyengiaoviec',              component: UyquyengiaoviecComponent },
    { path: 'nguoigiamsat',              component: NguoigiamsatComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}
