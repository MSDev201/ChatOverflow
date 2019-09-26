import { UserService } from './../../services/user/user.service';
import { Component, OnInit } from '@angular/core';
import { UserAuthService } from 'src/app/services/user/user-auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  public isLoggedIn = false;

  public userDetails$;

  constructor(
    private userAuthService: UserAuthService,
    private userService: UserService
  ) {
    this.userDetails$ = this.userService.GetCurrentUserDetails();
  }

  ngOnInit() {
    this.isLoggedIn = this.userAuthService.IsLoggedIn();
  }

  signOut() {
    this.userAuthService.SignOut();
  }

}
