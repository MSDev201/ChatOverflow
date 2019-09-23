import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    NavBarComponent
  ],
  imports: [
    RouterModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule
  ],
  exports: [
    NavBarComponent
  ]
})
export class CommonElementsModule { }
