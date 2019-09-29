import { IGroupChat } from './../../models/chat/group-chat';
import { ICreateGroupChat } from './../../models/api/chat/create-group-chat';
import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';
import { Observable, of, throwError, Subject } from 'rxjs';
import { exhaustMap, map, catchError } from 'rxjs/operators';
import { IChatMessageInput } from 'src/app/models/api/chat/chat-message-input';
import { IChatMessage } from 'src/app/models/chat/chat-message';

@Injectable({
  providedIn: 'root'
})
export class GroupChatService {

  private groupChats: IGroupChat[];
  public groupChatsChanged$ = new Subject<IGroupChat[]>();

  constructor(
    private apiService: ApiService
  ) {
  }

  public Create(data: ICreateGroupChat) {
    return this.apiService.MakeSecurePostRequest<IGroupChat>('v1/GroupChat/Create', data).pipe(
      map(res => {
        if (res.status === 200) {
          this.addGroup(res.body);
        }
        return res;
      })
    );
  }

  public GetAll(reloadAll: boolean = false) {
    if (this.groupChats == null || reloadAll) {
      return this.apiService.MakeSecureGetRequest<IGroupChat[]>('v1/GroupChat/List/CurrentUser').pipe(
        map(res => {
          if (res.status === 200) {
            this.groupChats = res.body;
            this.groupChatsChanged$.next(this.groupChats);
            return res.body;
          }
        }),
        catchError(y => throwError(y))
      );  
    }
    return of(this.groupChats);
    
  }

  public Get(id: string) {
    const foundGroup = this.groupChats == null ? null : this.groupChats.find(x => x.id === id);
    if (foundGroup == null) {
      return this.apiService.MakeSecureGetRequest<IGroupChat>('v1/GroupChat/CurrentUser/' + encodeURI(id)).pipe(
        map(res => {
          if (res.status === 200) {
            if (this.groupChats == null) {
              this.groupChats = [];
            }
            this.groupChats.push(res.body);
            this.groupChatsChanged$.next(this.groupChats);
            return res.body;
          }
        })
      )
    }
    return of(foundGroup);
  }

  public SendMessage(groupId: string, data: IChatMessageInput) {
    return this.apiService.MakeSecurePostRequest<IChatMessage>('v1/GroupChat/Message/Create/' + encodeURI(groupId), data);
  }

  public GetMessages(groupId: string) {
    return this.apiService.MakeSecureGetRequest<IChatMessage[]>('v1/GroupChat/Messages/' + encodeURI(groupId)).pipe();
  }

  private addGroup(chat: IGroupChat) {
    this.groupChats.push(chat);
    this.groupChatsChanged$.next(this.groupChats);
  }

}
