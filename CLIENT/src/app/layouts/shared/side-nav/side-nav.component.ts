import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { SidenavService } from '@app/_services';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { BrowserModule } from '@angular/platform-browser'


@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SideNavComponent implements OnInit {

  menuState:string = 'out';

  menu_nghiepvus=[];
  menu_quytrinhs= [];
  hienquytrinh = false;
//  menu_quytrinh = new Menuquytrinh();

  isadmin :string='0';
  constructor(public sideNavService: SidenavService) { }

  ngOnInit(): void {
    this.get_danhsach_nguoidung();
    this.isadmin = localStorage.getItem('IsAdmin') ? localStorage.getItem('IsAdmin') : sessionStorage.getItem('IsAdmin') || '0';
  }
  ngAfterViewInit(){
      $(document).ready(function(){
        $('li[data-toggle="collapse"]>a').click( function(){
          $('li[data-toggle="collapse"]>a').not(this).removeClass('collapsed');
            $(this).toggleClass("collapsed");
            var target = $(this).parent().children('.sub-menu');
            $('ul.sub-menu').not(target).addClass("collapse");
            $(target).toggleClass("collapse");          
        });
      });
  }
  get_danhsach_nguoidung() {
    // var UserName_ = localStorage.getItem('UserName') ? localStorage.getItem('UserName') : sessionStorage.getItem('UserName') || '';
    // this.sideNavService.get_menuquytrinh(UserName_)
    //   .subscribe({
    //     next: (_data) => {
    //       this.menu_nghiepvus = _data;
    //       if(this.menu_nghiepvus.length > 0){
    //       this.hienquytrinh = true;
    //       var id_quytrinh = "";
    //       var id_quytrinh2 = "";
    //       var index = 0;
    //       this.menu_nghiepvus.forEach(obj => {            
    //         id_quytrinh = obj.id_quytrinh;   

    //         if(id_quytrinh != id_quytrinh2 ){
    //           if(index > 0)
    //           this.menu_quytrinhs.push(this.menu_quytrinh);
    //           this.menu_quytrinh = new Menuquytrinh();
    //           this.menu_quytrinh.ten_quytrinh = obj.ten_quytrinh;
    //           this.menu_quytrinh.menus = [];
              
    //         }            
    //         id_quytrinh2 = obj.id_quytrinh;
    //         this.menu_quytrinh.menus.push(obj);
    //         index ++;
    //       });
    //       this.menu_quytrinhs.push(this.menu_quytrinh);
    //       }
    //     },
    //     error: error => {
    //       console.log(error);
    //     }
    //   });
  }

  toggleMenu() {   
    this.menuState = this.menuState === 'out' ? 'in' : 'out';
  }
}
