import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { GiavattuComponent } from "./giavattu/giavattu.component";
import { PhanxuongComponent } from "./phanxuong/phanxuong.component";




const routes: Routes = [           
    { path: 'phanxuong',              component: PhanxuongComponent },
    { path: 'giavattu',              component: GiavattuComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}
