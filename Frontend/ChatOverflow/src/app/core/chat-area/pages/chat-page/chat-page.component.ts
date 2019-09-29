import { UserService } from './../../../../services/user/user.service';
import { GroupChatService } from './../../../../services/chat/group-chat.service';
import { IGroupChat } from './../../../../models/chat/group-chat';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { exhaustMap, switchMap, map } from 'rxjs/operators';

@Component({
  selector: 'app-chat-page',
  templateUrl: './chat-page.component.html',
  styleUrls: ['./chat-page.component.scss']
})
export class ChatPageComponent implements OnInit {

  public messages = [];
  public chat: IGroupChat;
  public currentUser$;

  private currentId: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private groupChatService: GroupChatService,
    private userServie: UserService
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
    }).subscribe();

    this.messages = this.messages.reverse();
    this.messages.push({
      id: 'kjsldkfsd',
      message: content,
      createdAt: new Date(Date.now())
    });
    this.messages = this.messages.reverse();

    event.target.innerHTML = '';
  }

}
