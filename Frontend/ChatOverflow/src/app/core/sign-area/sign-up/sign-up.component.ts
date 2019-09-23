import { UserSignService } from './../../../services/user/user-sign.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { debounceTime, map, switchMap, catchError } from 'rxjs/operators';
import { IUserSignUp } from 'src/app/models/api/user-sign-up';
import { throwError } from 'rxjs';


enum MsgBoxLocation {
  Username = 0,
  Email = 1,
  Password = 2,
  PasswordRepeat = 3
}

enum MsgBoxType {
  Info = 0,
  Warning = 1,
  Danger = 2
}

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

  public signUpForm = new FormGroup({
    username: new FormControl(''),
    password: new FormControl(''),
    email: new FormControl(''),
    passwordRepeat: new FormControl(''),
  });

  public msgBoxes: Array<{ location: MsgBoxLocation, msg: string, type: MsgBoxType }> = [];

  public signUpSuccess = false;

  constructor(
    private userSignService: UserSignService
  ) { }

  ngOnInit() {
    this.signUpForm.valueChanges
    .pipe(
      debounceTime(200),
      map(formValues => {
        const convertedValues: IUserSignUp = {
          userName: formValues.username,
          eMail: formValues.email,
          password: formValues.password,
        };
        return convertedValues;
      }),
      switchMap(values => {
        return this.userSignService.SignUpCheck(values);
      }),
      map(response => {
        return response.body;
      })
    )
    .subscribe((x: any) => {
      this.msgBoxes = [];
      if (this.signUpForm.value.password.length > 0 &&
        this.signUpForm.value.passwordRepeat.length > 1 &&
        this.signUpForm.value.password !== this.signUpForm.value.passwordRepeat) {
        this.msgBoxes.push({
          location: MsgBoxLocation.PasswordRepeat,
          type: MsgBoxType.Danger,
          msg: 'Die beiden Passwörter stimmen nicht überein.'
        });
      }
      if (x.errors != null) {
        for (const error of x.errors) {
          let addBlock: { location: MsgBoxLocation, msg: string, type: MsgBoxType };
          switch (error.code) {
            case 'InvalidEmail':
              addBlock = {
                location: MsgBoxLocation.Email,
                type: MsgBoxType.Warning,
                msg: 'Diese E-Mail scheint ungültig zu sein.'
              };
              break;
            case 'PasswordTooShort':
              addBlock = {
                location: MsgBoxLocation.Password,
                type: MsgBoxType.Warning,
                msg: 'Das Passwort ist zu kurz.'
              };
              break;
            case 'PasswordRequiresUniqueChars':
              addBlock = {
                location: MsgBoxLocation.Password,
                type: MsgBoxType.Warning,
                msg: 'Das Passwort benötigt mehr einzigartige Zeichen.'
              };
              break;
            case 'DuplicateUserName':
              addBlock = {
                location: MsgBoxLocation.Username,
                type: MsgBoxType.Danger,
                msg: 'Dieser Benutzername ist leider schon vergeben.'
              };
              break;
            case 'InvalidUserName':
              addBlock = {
                location: MsgBoxLocation.Username,
                type: MsgBoxType.Warning,
                msg: 'Dieser Benutzername enthält ungültige Zeichen.'
              };
              break;
            case 'DuplicateEmail':
              addBlock = {
                location: MsgBoxLocation.Email,
                type: MsgBoxType.Danger,
                msg: 'Diese E-Mail wird schon verwendet.'
              };
              break;

            default:
              break;
          }
          if (addBlock != null && this.msgBoxes.find(y => y.location === addBlock.location) == null) {
            if (addBlock.location === MsgBoxLocation.Username && this.signUpForm.value.username.length <= 0) {
              continue;
            }
            if (addBlock.location === MsgBoxLocation.Email && this.signUpForm.value.email.length <= 0) {
              continue;
            }
            if (addBlock.location === MsgBoxLocation.Password && this.signUpForm.value.password.length <= 0) {
              continue;
            }
            if (addBlock.location === MsgBoxLocation.PasswordRepeat && this.signUpForm.value.passwordRepeat.length <= 0) {
              continue;
            }
            this.msgBoxes.push(addBlock);
          }
        }
      }
    });
  }

  register() {
    const convertedValues: IUserSignUp = {
      userName: this.signUpForm.value.username,
      eMail: this.signUpForm.value.email,
      password: this.signUpForm.value.password,
    };
    const createObs = this.userSignService.SignUp(convertedValues);
    createObs.subscribe(result => {
      if (result.status === 201) { // Success
        this.userSignService.SignIn({
          nameId: convertedValues.userName,
          password: convertedValues.password
        }).pipe(
          map(x => x),
          catchError(error => {
            if (error.status === 400) { // Bad request
              console.error('Sign in after register failed!');
            } else if (error.status === 403) { // Forbidden (locked out)
              console.log('Forbidden');
            }
            return throwError(error);
          })
        ).subscribe();
        this.signUpForm.reset('');
      }
    });
  }

}
