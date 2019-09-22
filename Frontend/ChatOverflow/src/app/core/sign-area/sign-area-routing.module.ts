import { SignUpComponent } from './sign-up/sign-up.component';
import { SignInPageComponent } from './sign-in-page/sign-in-page.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SignAreaComponent } from './sign-area.component';



const routes: Routes = [
  { path: '', component: SignAreaComponent, children: [
    { path: 'in', component: SignInPageComponent },
    { path: 'up', component: SignUpComponent },
    { path: '**', redirectTo: '/sign/in', pathMatch: 'full' },
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SignAreaRoutingModule { }
