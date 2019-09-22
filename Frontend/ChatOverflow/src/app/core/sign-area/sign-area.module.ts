import { CommonElementsModule } from './../../common/common-elements.module';
import { AppModule } from './../../app.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignAreaComponent } from './sign-area.component';
import { SignAreaRoutingModule } from './sign-area-routing.module';
import { NavBarComponent } from '../../common/nav-bar/nav-bar.component';
import { SignInPageComponent } from './sign-in-page/sign-in-page.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    SignAreaComponent,
    SignInPageComponent,
    SignUpComponent,
  ],
  imports: [
    SignAreaRoutingModule,
    CommonModule,
    ReactiveFormsModule,
    CommonElementsModule
  ]
})
export class SignAreaModule { }
