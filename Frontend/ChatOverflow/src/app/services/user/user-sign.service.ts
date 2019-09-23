import { IErrorResult } from './../../models/api/error-result';
import { IUserSignUp } from './../../models/api/user-sign-up';
import { ApiService } from './../api.service';
import { Injectable } from '@angular/core';
import { IUserSignIn } from 'src/app/models/api/user-sign-in';
import { map } from 'rxjs/operators';
import { UserAuthService } from './user-auth.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserSignService {

  constructor(
    private apiService: ApiService,
    private userAuthService: UserAuthService,
    private router: Router
  ) { }

  public SignUpCheck(data: IUserSignUp) {
    return this.apiService.MakePostRequest<IErrorResult>('v1/UserAuth/SignUp/Check', data);
  }

  public SignUp(data: IUserSignUp) {
    return this.apiService.MakePostRequest<IErrorResult>('v1/UserAuth/SignUp/Create', data);
  }

  public SignIn(data: IUserSignIn) {
    return this.apiService.MakePostRequest<{ token: string }>('v1/UserAuth/SignIn', data).pipe(
      map(result => {
        if (result.status === 200) { // Success
          this.userAuthService.SetToken(result.body.token);
          this.router.navigate(['/chat']);
        }
        return result;
      })
    );
  }
}
