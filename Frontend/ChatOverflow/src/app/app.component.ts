import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ChatOverflow';

  constructor(private router: Router) {

    const isSignedIn = true;
    if (isSignedIn) {
      router.navigate(['/chat']);
    } else {
      router.navigate(['/sign']);
    }
    

  }
}
