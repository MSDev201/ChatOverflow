import { UserService } from './../../../../services/user/user.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { IUserDetails } from 'src/app/models/user/user-details';

@Component({
  selector: 'app-create-group-chat-page',
  templateUrl: './create-group-chat-page.component.html',
  styleUrls: ['./create-group-chat-page.component.scss']
})
export class CreateGroupChatPageComponent implements OnInit {

  public createGroupForm: FormGroup;
  public foundUsers$ = new Subject<IUserDetails[]>();

  private userSearchChanged$ = new Subject<string>();

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService
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
      debounceTime(210)
    ).subscribe(term => {
      term = term.trim();
      if (term.length <= 0) { 
        this.foundUsers$.next([]);
        return;
      }
      this.userService.GetUsersBySearchTerm(term).subscribe(x => {
        this.foundUsers$.next(x);
      });
    });


    this.createGroupForm.valueChanges.subscribe(x => {
      console.log(x);
    })
  }

  public userSearch(term: string) {
    this.userSearchChanged$.next(term);
  }

}
