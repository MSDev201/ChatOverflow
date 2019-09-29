import { CreateGroupChatPageComponent } from './pages/create-group-chat-page/create-group-chat-page.component';
import { ChatPageComponent } from './pages/chat-page/chat-page.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ChatAreaComponent } from './chat-area.component';


const routes: Routes = [
  { path: '', component: ChatAreaComponent, children: [
    { path: 'group/:id', component: ChatPageComponent },
    { path: 'create/group', component: CreateGroupChatPageComponent }
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChatAreaRoutingModule { }
