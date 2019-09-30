import { IChatMessage } from 'src/app/models/chat/chat-message';
import { UserService } from './../../../../services/user/user.service';
import { GroupChatService } from './../../../../services/chat/group-chat.service';
import { IGroupChat } from './../../../../models/chat/group-chat';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { exhaustMap, switchMap, map } from 'rxjs/operators';
import { SocketService } from 'src/app/services/socket.service';

@Component({
  selector: 'app-chat-page',
  templateUrl: './chat-page.component.html',
  styleUrls: ['./chat-page.component.scss']
})
export class ChatPageComponent implements OnInit {

  public messages: IChatMessage[] = [];
  public chat: IGroupChat;
  public currentUser$;

  private currentId: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private groupChatService: GroupChatService,
    private userServie: UserService,
    private socketService: SocketService
  ) {
    this.activatedRoute.paramMap.pipe(
      switchMap(x => {
        this.currentId = x.get('id');
        return this.groupChatService.Get(this.currentId);
      }),
      map(x => {

        this.messages = [];
        this.chat = x;
        return x;
      }),
      switchMap((x) => {
        return this.groupChatService.GetMessages(x.id);
      }),
      map(messagesRes => {
        if(messagesRes.status === 200) {
          this.messages = messagesRes.body;
          this.updateMessages();
        }
      })
    ).subscribe();
  }

  ngOnInit() {
    this.currentUser$ = this.userServie.GetCurrentUserDetails();
  }

  public sendMessage(event: any) {
    event.preventDefault();

    const content = event.target.innerText;

    this.groupChatService.SendMessage(this.chat.id, {
      message: content
    }).subscribe(res => {
      if (res.status === 200) {
        this.messages.push(res.body);
        this.updateMessages();
      }
    });

    event.target.innerHTML = '';
  }

  private updateMessages() {
    this.messages = this.messages.sort((a, b) => b.createdAt.getTime() - a.createdAt.getTime());
  }

}
