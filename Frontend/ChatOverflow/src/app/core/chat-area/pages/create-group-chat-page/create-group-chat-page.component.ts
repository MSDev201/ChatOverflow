import { GroupChatService } from './../../../../services/chat/group-chat.service';
import { IUserDetails } from './../../../../models/user/user-details';
import { UserService } from './../../../../services/user/user.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, map, exhaustMap } from 'rxjs/operators';
import { ICreateGroupChat } from 'src/app/models/api/chat/create-group-chat';

@Component({
  selector: 'app-create-group-chat-page',
  templateUrl: './create-group-chat-page.component.html',
  styleUrls: ['./create-group-chat-page.component.scss']
})
export class CreateGroupChatPageComponent implements OnInit {

  public createGroupForm: FormGroup;
  public foundUsers$ = new Subject<IUserDetails[]>();
  public selectedUsers: IUserDetails[] = [];

  private userSearchChanged$ = new Subject<string>();

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private groupChatService: GroupChatService
  ) { }

  ngOnInit() {
    this.createGroupForm = this.formBuilder.group({
      name: '',
      password: null,
      access: this.formBuilder.group({
        usersInList: true,
        password: false,
        link: false,
      })
    });

    this.userSearchChanged$.pipe(
      debounceTime(150),
      exhaustMap(term => {
        term = term.trim();
        if (term.length <= 0) { 
          this.foundUsers$.next([]);
          return of([]);
        }
        return this.userService.GetUsersBySearchTerm(term);
      })
    ).subscribe(x => {
      this.selectedUsers.find
        this.foundUsers$.next(x.filter(y => this.selectedUsers.find(x => x.id === y.id) == null));
    });
  }

  public userSearch(term: string) {
    this.userSearchChanged$.next(term);
  }

  public selectUser(user: IUserDetails) {
    if (this.selectedUsers.find(x => x.id === user.id) != null) {
      return;
    }
    this.foundUsers$.next([]);
    this.selectedUsers = this.selectedUsers.reverse();
    this.selectedUsers.push(user);
    this.selectedUsers = this.selectedUsers.reverse();
  }

  public unselectUser(user: IUserDetails) {
    this.selectedUsers.splice(this.selectedUsers.findIndex(x => x.id === user.id), 1);
  }

  public createGroup() {
    const formValues = this.createGroupForm.value;
    const members: string[] = [];
    this.selectedUsers.forEach(x => members.push(x.id));
    const newGroup: ICreateGroupChat = {
      name: formValues.name,
      password: formValues.access.password ? formValues.password : null,
      createLink: formValues.access.link,
      members
    };

    this.groupChatService.Create(newGroup).subscribe(res => {
      console.log(res);
    });
  }

}
