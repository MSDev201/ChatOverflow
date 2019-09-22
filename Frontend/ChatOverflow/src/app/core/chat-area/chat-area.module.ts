import { ChatPageComponent } from './pages/chat-page/chat-page.component';
import { ChatAreaRoutingModule } from './chat-area-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatAreaComponent } from './chat-area.component';
import { NavBarComponent } from '../nav-bar/nav-bar.component';
import { SideNavComponent } from './side-nav/side-nav.component';



@NgModule({
  declarations: [
    ChatAreaComponent,
    NavBarComponent,
    ChatPageComponent,
    SideNavComponent
  ],
  imports: [
    CommonModule,
    ChatAreaRoutingModule,
  ],
  bootstrap: [ChatAreaComponent],
})
export class ChatAreaModule { }
