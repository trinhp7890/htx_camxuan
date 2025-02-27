import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';
import { GiavattuComponent } from "./giavattu/giavattu.component";
import { KhoComponent } from "./kho/kho.component";
import { LuongphanComponent } from "./luongphan/luongphan.component";
import { PhanxuongComponent } from "./phanxuong/phanxuong.component";
import { TuyenduongComponent } from "./tuyenduong/tuyenduoing.component";
import { TraicungcapComponent } from "./traicungcap/traicungcap.component";
import { VattuComponent } from "./vattu/vattu.component";




const routes: Routes = [           
    { path: 'phanxuong',              component: PhanxuongComponent },
    { path: 'duong',              component: TuyenduongComponent },
    { path: 'giavattu',              component: GiavattuComponent },
    { path: 'vattu',              component: VattuComponent },
    { path: 'trai',              component: TraicungcapComponent },
    { path: 'kho',              component: KhoComponent },
    { path: 'luongphan',              component: LuongphanComponent },
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}
