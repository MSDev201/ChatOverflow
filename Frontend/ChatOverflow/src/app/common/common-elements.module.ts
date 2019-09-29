import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Nl2brPipe } from './pipes/nl2br.pipe';



@NgModule({
  declarations: [
    NavBarComponent,
    Nl2brPipe
  ],
  imports: [
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    NgbModule
  ],
  exports: [
    NavBarComponent,
    Nl2brPipe
  ]
})
export class CommonElementsModule { }
