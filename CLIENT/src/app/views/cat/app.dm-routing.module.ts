import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';

import { DantocComponent } from "./dantoc/dantoc.component";



import { TreeModule } from '@circlon/angular-tree-component';



const routes: Routes = [    
    { path: 'dan-toc',              component: DantocComponent },  
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        TreeModule
    ],
    exports: [RouterModule]
})

export class AppDmRoutingModule {}