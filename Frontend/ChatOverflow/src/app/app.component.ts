import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ChatOverflow';

  /**
   *
   */
  constructor() {
    var url = 'wss://localhost:44339/api/v1/Socket/api/Socket/ws';
    var webSocket = new WebSocket(url);

    webSocket.onmessage = (message) => {
      console.log('WebSocket MSG!', JSON.parse(message.data));
      webSocket.send(message.data);
    };
  }
}
