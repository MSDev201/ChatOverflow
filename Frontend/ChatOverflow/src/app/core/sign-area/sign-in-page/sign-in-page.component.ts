import { UserSignService } from './../../../services/user/user-sign.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { IUserSignIn } from 'src/app/models/api/user-sign-in';
import { map, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-sign-in-page',
  templateUrl: './sign-in-page.component.html',
  styleUrls: ['./sign-in-page.component.scss']
})
export class SignInPageComponent implements OnInit {

  public signInForm = new FormGroup({
    nameId: new FormControl(''),
    password: new FormControl(''),
  });

  public showLoginFailed = false;

  constructor(
    private userSignService: UserSignService
  ) { }

  ngOnInit() {
  }

  login() {
    const signInData: IUserSignIn = {
      nameId: this.signInForm.value.nameId,
      password: this.signInForm.value.password,
      twoFactorCode: null,
    };
    this.userSignService.SignIn(signInData).pipe(
      map(x => x),
      catchError(error => {
        if (error.status === 400) { // Bad request
          console.error('Login failed...');
          this.showLoginFailed = true;
        } else if (error.status === 403) { // Forbidden (locked out)
          console.log('Forbidden');
        }
        return throwError(error);
      })
    ).subscribe();
  }

}
