import { HubService } from './hub.service';
import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatHubService {

  private connection: HubConnection;

  public newGroupMessageEvent$ = new Subject();

  constructor(
    private hubService: HubService
  ) {
    if (this.connection == null) {
      this.connection = this.hubService.StartConnection('v1/chat');

      // init handlers

      this.connection.onclose((event) => {
        // TODO: Handle close of socket
        console.log('Websocket closed connection from remote', event);
      });

      // Set Events
      this.connection.on('NewGroupMessageEvent', () => this.newGroupMessageEvent$.next());
    }
  }

  public FireGroupMessageEvent(groupId: string) {
    this.connection.invoke('SendGroupMessage', groupId);
  }
}
