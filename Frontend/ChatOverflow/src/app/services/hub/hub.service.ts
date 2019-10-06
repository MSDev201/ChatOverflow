import { UserAuthService } from 'src/app/services/user/user-auth.service';
import { Injectable, isDevMode } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { ApiService } from '../api.service';

const MAIN_HUB_URL = isDevMode ? 'https://localhost:4420/hub/' : 'https://chatoverflow.live/hub/';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  private token: string;

  constructor(
    private apiService: ApiService,
    private userAuthService: UserAuthService
  ) {}

  private getRefreshToken() {
    return this.apiService.MakeSecureGetRequest<{value: string}>('v1/Socket/Token');
  }

  public StartConnection(path: string): HubConnection {
    console.log(this.userAuthService.GetToken());
    const con = new HubConnectionBuilder()
      .withUrl(MAIN_HUB_URL + path, {
        accessTokenFactory: () => this.userAuthService.GetToken()
      })
      .build();
    this.getRefreshToken().subscribe(res => {
      if (res.status === 200) {
        this.token = res.body.value;
        // Start the connection and authorize
        con.start().then(() => {
          con.invoke('StartAuth', this.token);
        }).catch(error => {
          console.log(error);
        });
      }
    });

    

    return con;
  }
}
