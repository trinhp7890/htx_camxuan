import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { PhanxuongComponent } from "./phanxuong/phanxuong.component";




const routes: Routes = [           
    { path: 'phanxuong',              component: PhanxuongComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}
