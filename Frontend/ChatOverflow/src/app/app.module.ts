import { ChatHubService } from './services/hub/chat-hub.service';
import { HubService } from './services/hub/hub.service';
import { GroupChatService } from './services/chat/group-chat.service';
import { UserService } from './services/user/user.service';
import { ApiService } from './services/api.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './common/nav-bar/nav-bar.component';
import { SideNavComponent } from './core/chat-area/side-nav/side-nav.component';
import { ChatPageComponent } from './core/chat-area/pages/chat-page/chat-page.component';
import { ChatAreaModule } from './core/chat-area/chat-area.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { SignAreaModule } from './core/sign-area/sign-area.module';
import { UserAuthService } from './services/user/user-auth.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JsonDateInterceptor } from './special/http-interceptor';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    
  ],
  providers: [
    UserAuthService,
    ApiService,
    UserService,
    GroupChatService,
    HubService,
    ChatHubService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JsonDateInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
