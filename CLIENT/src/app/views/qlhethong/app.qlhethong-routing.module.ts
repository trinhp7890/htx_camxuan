import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';



import { TreeModule } from '@circlon/angular-tree-component';
import { QldonviComponent } from "./qlnhansu/donvi.component";
import { LoginComponent } from "../login/login.component";


const routes: Routes = [           
    { path: 'qlnguoidung',              component: QldonviComponent },  
    { path: 'logintelegram/:id',              component: LoginComponent },  
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        TreeModule
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}