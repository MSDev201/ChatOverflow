import { ICreateGroupChat } from './../../models/api/chat/create-group-chat';
import { Injectable } from '@angular/core';
import { ApiService } from '../api.service';
import { IGroupChat } from 'src/app/models/chat/group-chat';

@Injectable({
  providedIn: 'root'
})
export class GroupChatService {

  constructor(
    private apiService: ApiService
  ) {
  }

  public Create(data: ICreateGroupChat) {
    return this.apiService.MakeSecurePostRequest<IGroupChat>('v1/GroupChat/Create', data);
  }

}
