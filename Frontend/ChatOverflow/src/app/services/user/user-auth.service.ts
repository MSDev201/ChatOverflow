import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {

  constructor(
    private router: Router
  ) { }

  public IsLoggedIn(): boolean {
    if (this.GetToken() != null) {
      return true;
    }
    return false;
  }

  public GetToken(): string {
    return localStorage.getItem('token');
  }

  public SetToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }

  public SignOut() {
    localStorage.removeItem('token');
    this.router.navigate(['/sign/in']);
  }

  public TokenIsValide() {
    const token = this.GetToken();
    if (token == null) {
      return null;
    } else {
      // Decode token
      const decoded = atob(token.split('.')[1]);
      if (decoded == null) {
        return null;
      }
      // To json
      const tokenObj = JSON.parse(decoded);
      if (tokenObj == null || tokenObj != null && tokenObj.exp == null) {
        return null;
      }
      // Expires one minute before actual
      const tokenExpiresAt = tokenObj.exp - 60;
      const currentUnixTime = Math.round(Date.now() / 1000);
      if (currentUnixTime > tokenExpiresAt) {
        return false;
      } else {
        return true;
      }
    }
  }

  public GetUserIdFromToken() {
    const token = this.GetToken();
    if (token == null) {
      return null;
    } else {
      // Decode token
      const decoded = atob(token.split('.')[1]);
      if (decoded == null) {
        return null;
      }
      // To json
      const tokenObj = JSON.parse(decoded);
      if (tokenObj == null || tokenObj != null && tokenObj.UserId == null) {
        return null;
      }
      return tokenObj.UserId;
    }
  }
}
