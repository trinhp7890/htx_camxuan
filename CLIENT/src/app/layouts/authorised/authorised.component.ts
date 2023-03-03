import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@app/_services';

@Component({
  selector: 'app-authorised',
  templateUrl: './authorised.component.html',
  styleUrls: ['./authorised.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AuthorisedComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private route: Router
  ) { 
    
  }

  ngOnInit(): void {

  }

}
