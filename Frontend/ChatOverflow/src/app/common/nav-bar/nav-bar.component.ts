import { Component, OnInit } from '@angular/core';
import { UserAuthService } from 'src/app/services/user/user-auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  public isLoggedIn = false;

  constructor(
    private userAuthService: UserAuthService
  ) { }

  ngOnInit() {
    this.isLoggedIn = this.userAuthService.IsLoggedIn();
  }

  signOut() {
    this.userAuthService.SignOut();
  }

}
