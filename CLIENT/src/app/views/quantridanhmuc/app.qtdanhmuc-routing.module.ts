import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { GiavattuComponent } from "./giavattu/giavattu.component";
import { PhanxuongComponent } from "./phanxuong/phanxuong.component";
import { VattuComponent } from "./vattu/vattu.component";




const routes: Routes = [           
    { path: 'phanxuong',              component: PhanxuongComponent },
    { path: 'giavattu',              component: GiavattuComponent },
    { path: 'vattu',              component: VattuComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}
