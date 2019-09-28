import { IChatPreview, ChatPreviewType } from './../../../models/chat/chat-preview';
import { GroupChatService } from './../../../services/chat/group-chat.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {

  public chatPreviews: IChatPreview[] = [];

  constructor(
    private groupChatService: GroupChatService
  ) {}

  ngOnInit() {
    this.groupChatService.GetAll().subscribe();
    this.groupChatService.groupChatsChanged$.subscribe(z => {
      this.chatPreviews = [];
      z.forEach(chat => {
        if (this.chatPreviews.find(x => x.id === chat.id) == null) {
          this.chatPreviews.push({
            id: chat.id,
            type: ChatPreviewType.GroupChat,
            name: chat.name,
            lastMessage: '',
            newMessagesCount: 0,
          });
        }
      });
    });
  }

}
