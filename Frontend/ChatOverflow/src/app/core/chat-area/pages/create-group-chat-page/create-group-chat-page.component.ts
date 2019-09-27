import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-create-group-chat-page',
  templateUrl: './create-group-chat-page.component.html',
  styleUrls: ['./create-group-chat-page.component.scss']
})
export class CreateGroupChatPageComponent implements OnInit {

  public createGroupForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder
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
    this.createGroupForm.valueChanges.subscribe(x => {
      console.log(x);
    })
  }

}
