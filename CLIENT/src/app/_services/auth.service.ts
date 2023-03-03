import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { Account } from '@app/_models/account';
import { environment } from '@environments/environment';

const baseUrl = `${environment.apiURL}`;

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private accountSubject: BehaviorSubject<Account>;
  public account: Observable<Account>;
  constructor(
    private http: HttpClient,
    private router: Router
    ) { 
      this.accountSubject = new BehaviorSubject<Account>(null);
      this.account = this.accountSubject.asObservable();
  }

  public get accountValue(): Account {
    return this.accountSubject.value;
  }

  login(email: string, password: string): Observable<any> {
    const body = new HttpParams()
    .set('grant_type', 'password')  
    .set('username', email)
    .set('password', password);
    return this.http.post<any>(`${baseUrl}/token`, body, httpOptions);     
  }

  setToken(token) {
    return localStorage.setItem('token',token.access_token);
  }
    
  setLogin(token) {
    return localStorage.setItem('isLogged', token);
  }
  
  setACC(token){
    return localStorage.setItem('UserName', token.userName);
  }

  setIsAdmin(token){
    return localStorage.setItem('IsAdmin', token.Isadmin);
  }

  setId_donvi(token){
    return localStorage.setItem('Id_dv', token.Id_dv);
  }
  setMa_donvi(token){
    return localStorage.setItem('Ma_donvi', token.madonvi);
  }
  setMa_nhanvien(token){
    return localStorage.setItem('Ma_nhanvien', token.manhanvien);
  }
  setTen_donvi(token){
    return localStorage.setItem('Ten_donvi', token.Ten_dv);
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.setItem("isLogged","0");
    localStorage.setItem('UserName', "");
    localStorage.removeItem('IsAdmin');
    localStorage.setItem("Id_dv","0");
    localStorage.setItem("Ten_donvi","");
    localStorage.setItem('Ma_donvi', "");
    localStorage.setItem('Ma_nhanvien', "");
    //location.reload(true);
    // this.router.navigate(['/front/login']);
    this.router.navigate(['/front/login']);
  }

  refreshToken(): Observable<any> {
    return this.http.post<any>(
      `${baseUrl}/refresh-token`,
      {},
      { withCredentials: true }
    )
    .pipe(map((account) => {
      this.accountSubject.next(account);
      this.startRefreshTokenTimer();

      return account
    }));
  }

  private refreshTokenTimeout;
  private startRefreshTokenTimer() {
    const jwtToken = JSON.parse(atob(this.accountValue.JwtToken.split('.')[1]));

    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);

    this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
