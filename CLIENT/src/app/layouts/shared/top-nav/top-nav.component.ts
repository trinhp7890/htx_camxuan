import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/_services';
import { UserService } from '@app/_services/sys/user.service';
import { Account } from '@app/_models/account';
import { Router} from '@angular/router';
import { environment } from '@environments/environment';
import { RealtimeService } from '@app/_services/realtime.service';
@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent implements OnInit {

  serviceBase = `${environment.apiURL}`;
  account: string = '';
  hinhanh: '';
  tennguoidung: '';
  count_thongbao:string = '';
  danhsachthongbao: Object[] = [    
  ];
  chucdanh: '';
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private realtimeService: RealtimeService
  ) { }
  Ma_nhanvien = localStorage.getItem('Ma_nhanvien') ? localStorage.getItem('Ma_nhanvien') : sessionStorage.getItem('Ma_nhanvien') || '';

  ngOnInit(): void {
    
    this.account = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || ''; 
    this.getchitietnguoidung();
    this.realtimeService.connect_realtime(this.Ma_nhanvien);
    this.connect_realtime();
    this.get_danhsachthongbao();
  }

  logout() {
    this.authService.logout();
  }

  Change_pass() {
    this.router.navigate(['/change-pass']);
  }

  getchitietnguoidung() {
    this.userService.get_nguoidungchitiet(this.account)
      .subscribe(
        _data => {
          this.tennguoidung = _data[0].ten_nd;
          this.hinhanh = _data[0].hinhanh;
          this.chucdanh = _data[0].chucdanh;
        }
      );
  }

  // realtime
  connect_realtime(): void {
    try {
      this.realtimeService._hubConnection.on('Congviec_phatsinh', (message) => {
        this.get_danhsachthongbao();
      })
    } catch (e) {
      this.router.navigate(['/']);
    }
  }
  get_thongbao_realtime_byma(){
    this.realtimeService.get_thongbao_realtime_byma(this.Ma_nhanvien);
  }
   // lấy danh sách công việc chu tri
   get_danhsachthongbao() {
    this.realtimeService.get_thongbao_realtime_byma(this.Ma_nhanvien)
      .subscribe(
        _data => {
          this.danhsachthongbao= _data;
          if (this.danhsachthongbao.length > 9) {
            this.count_thongbao = '9+'
          }else{
            this.count_thongbao = this.danhsachthongbao.length.toString();
          }
        }
      );
  }
  viewthongbao(thongbao){    
    this.realtimeService.thongbao_realtime_up(thongbao.id_thongbao)
      .subscribe(
        _data => {
          this.get_danhsachthongbao();
          
        }
      );     
    
  }
}
