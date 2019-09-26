import { ApiService } from './../api.service';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { IUserDetails } from 'src/app/models/user/user-details';
import { exhaustMap, map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private currentUserDetails: IUserDetails = null;
  private currentUserDetails$ = new Observable<IUserDetails>((obs) => {
    obs.next(this.currentUserDetails);
    obs.complete();
  });

  constructor(
    private apiService: ApiService
  ) {
  }

  public GetCurrentUserDetails() {
    return this.currentUserDetails$.pipe(
      exhaustMap(inputObj => {
        if(inputObj == null) {
          return this.apiService.MakeSecureGetRequest<IUserDetails>('v1/User/Details').pipe(
            map(res => {
              if (res.status === 200) {
                return res.body;
              }
            }),
            catchError(x => throwError(x))
          );
        } else {
          return of(inputObj);
        }
      })
    );
    
  }



  
}
