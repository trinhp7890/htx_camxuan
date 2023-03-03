import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '@app/_services';
import { first } from 'rxjs/operators';
@Component({
  selector: 'section[app-login]',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  loading = false;
  submitted = false;
  id_telegram = '';
  login_tele = false;
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.id_telegram = params.get('id')
    })
    if(this.id_telegram != null){
      this.login_tele = true;
    }
    console.log(this.id_telegram);

    this.form = this.formBuilder.group({
      email: ['',[Validators.required]],
      password: ['', Validators.required]
    });
  }

  get f() { return this.form.controls; }
  ngAfterViewInit(){
    $(document).ready(function () {
      var randomnum = Math.floor((Math.random() * 50000) + 1);
      // $("#hiddencapcha").val(randomnum);
      // $("#btcapcha").html(randomnum);
      $('.button-login-inside').click(function(){
        //CheckForm();
      });
    });
    function RefreshCapcha() {
        // var randomnum = Math.floor((Math.random() * 50000) + 1);
        // $("#hiddencapcha").val(randomnum);
        // $("#btcapcha").html(randomnum);
    }
    function CheckForm() {      
      if ($("#codecapcha").val() != $("#hiddencapcha").val()) { alert("Mã bảo vệ không đúng!"); $("#codecapcha").focus(); return false; }
      
  }
  }
  onSubmit(): void {
    var result: any;
    this.submitted = true;

    if (this.form.invalid){
      return;
    }

    this.loading = true;
    this.authService.login(this.f.email.value, this.f.password.value)
      .pipe(first())
      .subscribe(
        (res) => {
          result = res;
        },
        (err) => {
          this.loading = false;
          this.toastr.error(err, 'Thông báo:');
        },
        () => {
          this.authService.setToken(result);
          this.authService.setLogin(1);
          this.authService.setACC(result);
          this.authService.setIsAdmin(result);
          this.authService.setId_donvi(result);
          this.authService.setTen_donvi(result);
          this.authService.setMa_donvi(result);
          this.authService.setMa_nhanvien(result);
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || 'dashboard';
          this.router.navigateByUrl(returnUrl);
      });
  }
  Login_tele(){
    console.log("Login telegram");
    console.log("Username : " + this.f.email.value);
    console.log("Pass: " + this.f.password.value);
    console.log("Id Telegram: " + this.id_telegram);
  }

  
}
