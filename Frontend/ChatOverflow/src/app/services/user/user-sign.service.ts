import { IErrorResult } from './../../models/api/error-result';
import { IUserSignUp } from './../../models/api/user-sign-up';
import { ApiService } from './../api.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserSignService {

  constructor(
    private apiService: ApiService
  ) { }

  public SignUpCheck(data: IUserSignUp) {
    return this.apiService.MakePostRequest<IErrorResult>('v1/UserAuth/SignUp/Check', data);
  }

  public SignUp(data: IUserSignUp) {
    return this.apiService.MakePostRequest<IErrorResult>('v1/UserAuth/SignUp/Create', data);
  }
}
