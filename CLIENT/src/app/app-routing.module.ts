import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from "@angular/common";
import { AuthorisedComponent } from './layouts/authorised/authorised.component';
import { GuestComponent } from './layouts/guest/guest.component';
import { AuthGuard } from './_helpers';

const guestModule = () => import('./layouts/guest/guest.module').then(x => x.GuestMoudle);
const authModule = () => import('./layouts/authorised/authorised.module').then(x => x.AuthorisedModule);

const routes: Routes = [
  { path: 'front', component: GuestComponent, loadChildren: guestModule },
  // { path :'' ,component:LoginComponent},
  { path: '', component: AuthorisedComponent, loadChildren: authModule, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
