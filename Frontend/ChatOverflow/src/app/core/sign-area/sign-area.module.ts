import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignAreaComponent } from './sign-area.component';
import { SignAreaRoutingModule } from './sign-area-routing.module';
import { NavBarComponent } from '../nav-bar/nav-bar.component';
import { SignInPageComponent } from './sign-in-page/sign-in-page.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    SignAreaComponent,
    NavBarComponent,
    SignInPageComponent,
    SignUpComponent
  ],
  imports: [
    SignAreaRoutingModule,
    CommonModule,
    ReactiveFormsModule
  ]
})
export class SignAreaModule { }
