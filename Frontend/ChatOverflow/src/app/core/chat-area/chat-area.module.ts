import { CommonElementsModule } from './../../common/common-elements.module';
import { ChatPageComponent } from './pages/chat-page/chat-page.component';
import { ChatAreaRoutingModule } from './chat-area-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatAreaComponent } from './chat-area.component';
import { NavBarComponent } from '../../common/nav-bar/nav-bar.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { CreateGroupChatPageComponent } from './pages/create-group-chat-page/create-group-chat-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';



@NgModule({
  declarations: [
    ChatAreaComponent,
    ChatPageComponent,
    SideNavComponent,
    CreateGroupChatPageComponent
  ],
  imports: [
    CommonModule,
    ChatAreaRoutingModule,
    ReactiveFormsModule,
    CommonElementsModule,
    NgbModule
  ],
})
export class ChatAreaModule { }
