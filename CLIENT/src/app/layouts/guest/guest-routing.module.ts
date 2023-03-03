import { NgModule } from "@angular/core";
import { RouterModule, Routes } from '@angular/router';

import { HelpComponent } from '@app/views/help/help.component';
import { LoginComponent } from '@app/views/login/login.component';

const routes: Routes = [
    {
        path: '',
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'help', component: HelpComponent }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GuestRoutingModule {}