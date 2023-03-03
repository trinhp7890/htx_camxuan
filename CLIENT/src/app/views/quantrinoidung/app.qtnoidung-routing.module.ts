import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';



import { TreeModule } from '@circlon/angular-tree-component';
import { QlthongbaoComponent } from "./thongbao/qlthongbao.component";



const routes: Routes = [        
    { path: 'thongbao',              component: QlthongbaoComponent },    
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        TreeModule
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}