import { Component, OnInit } from '@angular/core';
import { SidenavService } from '@app/_services';

@Component({
  selector: 'app-side-nav-toggler',
  templateUrl: './side-nav-toggler.component.html',
  styleUrls: ['./side-nav-toggler.component.scss']
})
export class SideNavTogglerComponent implements OnInit {

  constructor(public sideNavService: SidenavService) { }

  ngOnInit(): void {
  }

}
