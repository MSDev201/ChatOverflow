import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  { 
    path: 'chat',
    loadChildren: () => import('./core/chat-area/chat-area.module').then(mod => mod.ChatAreaModule)
  },
  { 
    path: 'sign',
    loadChildren: () => import('./core/sign-area/sign-area.module').then(mod => mod.SignAreaModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
