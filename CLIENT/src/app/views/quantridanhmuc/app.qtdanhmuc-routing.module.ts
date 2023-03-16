import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { GiavattuComponent } from "./giavattu/giavattu.component";
import { PhanxuongComponent } from "./phanxuong/phanxuong.component";
import { TraicungcapComponent } from "./traicungcap/traicungcap.component";
import { VattuComponent } from "./vattu/vattu.component";




const routes: Routes = [           
    { path: 'phanxuong',              component: PhanxuongComponent },
    { path: 'giavattu',              component: GiavattuComponent },
    { path: 'vattu',              component: VattuComponent },
    { path: 'trai',              component: TraicungcapComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}
