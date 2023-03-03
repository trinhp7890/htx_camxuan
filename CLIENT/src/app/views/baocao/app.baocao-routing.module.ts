import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';



import { TreeModule } from '@circlon/angular-tree-component';
import { Bc_congviecphatsinhComponent } from "./congviecphatsinh/bc_congviecphatsinh.component";



const routes: Routes = [        
    { path: 'CVphatsinh',              component: Bc_congviecphatsinhComponent },    
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        TreeModule
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}