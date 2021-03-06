import { ChatHubService } from './../../../../services/hub/chat-hub.service';
import { IChatMessage } from 'src/app/models/chat/chat-message';
import { UserService } from './../../../../services/user/user.service';
import { GroupChatService } from './../../../../services/chat/group-chat.service';
import { IGroupChat } from './../../../../models/chat/group-chat';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { exhaustMap, switchMap, map, last } from 'rxjs/operators';

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
    private chatHubService: ChatHubService
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

    // Listen to events
    this.chatHubService.newGroupMessageEvent$.subscribe(() => {
      var lastChatMessage = this.messages.length <= 0 ? null : this.messages[0];
      if (lastChatMessage == null) {
        this.groupChatService.GetMessages(this.chat.id).pipe(
          map(x => {
            if (x.status === 200) {
              this.messages = x.body;
              this.updateMessages();
            }
          })
        ).subscribe();
      } else {
        this.groupChatService.GetNewerMessages(this.chat.id, lastChatMessage.id).pipe(
          map(x => {
            if (x.status === 200) {
              for (const newChatMsg of x.body) {
                if (this.messages.find(y => y.id === newChatMsg.id) == null) {
                  this.messages.push(newChatMsg);
                }
              }
              
              this.updateMessages();
            }
          })
        ).subscribe();
      }
    });
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
