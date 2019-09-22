import { ChatPageComponent } from './pages/chat-page/chat-page.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ChatAreaComponent } from './chat-area.component';


const routes: Routes = [
  { path: '', component: ChatAreaComponent, children: [
    { path: '', component: ChatPageComponent }
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatAreaRoutingModule { }
