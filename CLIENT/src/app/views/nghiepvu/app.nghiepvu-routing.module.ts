import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';



import { TreeModule } from '@circlon/angular-tree-component';
import { LoginComponent } from "../login/login.component";
import { NhapkhoComponent } from "./nhapkho/nhapkho.component";
import { NhapluongComponent } from "./nhapluong/nhapluong.component";


const routes: Routes = [            
    { path: 'logintelegram/:id',              component: LoginComponent },  
    { path: 'nhapkho',              component: NhapkhoComponent },
    { path: 'nhapluong',              component: NhapluongComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        TreeModule
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}