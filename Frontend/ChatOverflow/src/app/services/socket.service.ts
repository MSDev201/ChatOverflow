import { ApiService } from './api.service';
import { Injectable, isDevMode } from '@angular/core';

const MAIN_SOCKET_URL = isDevMode ? 'wss://localhost:4420/api/v1/Socket/ws' : 'wss://chatoverflow.live/api/v1/Socket/ws';

@Injectable({
  providedIn: 'root'
})
export class SocketService {

  private connection: WebSocket;
  private token: string;

  constructor(
    private apiService: ApiService
  ) {
    this.apiService.MakeSecureGetRequest<{value: string}>('v1/Socket/Token').subscribe(res => {
      if (res.status === 200) {
        this.token = res.body.value;
        this.initConnection();
      }
    });
  }

  private initConnection() {
    // Connect to websocket
    console.log('Websocket ctor init');

    this.connection = new WebSocket(MAIN_SOCKET_URL);

    // ON OPEN
    this.connection.onopen = (event) => {
      // SEND SOCKET ACCESS KEY
      console.log('Websocket was opened');

      this.sendMessageObject({
        token: this.token
      });
    };

    // ON CLOSE
    this.connection.onclose = (event) => {
      // TODO: Handle close of socket
      console.log('Websocket closed connection from remote');
    };

    // ON MESSAGE
    this.connection.onmessage = this.handleMessage;

    // ON ERROR
    this.connection.onerror = (event) => {
      console.warn('Websocket error!', event);
    };
  }



  private handleMessage(event: MessageEvent) {
    console.log('Websocket Message received', event);
  }

  private sendMessageObject(data: any) {
    const dataString = JSON.stringify(data);
    this.sendMessage(dataString);
  }

  private sendMessage(data: string) {
    this.connection.send(data);
  }
}
