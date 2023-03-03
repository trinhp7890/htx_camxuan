import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { AuthService } from '@app/_services';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
    var Islogin = localStorage.getItem('isLogged') ? localStorage.getItem('isLogged') : sessionStorage.getItem('isLogged') || '';
    // var Islogin= parseInt(localStorage.getItem('isLogged'));
    if (Islogin === '1') {
      return true;
    }

    this.router.navigate(['/front/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
