import { Injectable, isDevMode } from '@angular/core';
import { Subject, of, throwError, Observable } from 'rxjs';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { UserAuthService } from './user/user-auth.service';
import { switchMap, mergeMap, exhaustMap, debounce, debounceTime, throttleTime, catchError } from 'rxjs/operators';

const MAIN_API_URL = isDevMode ? 'https://localhost:4420/api/' : 'https://chatoverflow.live/api/';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private renewTokenReqSubj = new Subject();
  private renewTokenReqSubjFinished = new Subject();

  constructor(
    private http: HttpClient,
    private userAuthService: UserAuthService
  ) {
    this.renewTokenReqSubj.pipe(
      exhaustMap(x => {
        if (this.userAuthService.TokenIsValide() != null && !this.userAuthService.TokenIsValide()) {
          const userId = this.userAuthService.GetUserIdFromToken();
          if (userId == null) {
            return of(null);
          }
          return this.MakePutRequest<{ value: string }>('v1/UserAuth/Token/Refresh/' + userId, { value: this.userAuthService.GetToken() });
        }
        return of(null);
      }),
      catchError(res => {
        if (res != null && res.status !== 0) {
          this.userAuthService.SignOut();
        }
        return throwError(res);
      })
    ).subscribe(res => {
      if (res != null && res.status === 200) {
        this.userAuthService.SetToken(res.body.value);
      }
      this.renewTokenReqSubjFinished.next();
    });
  }

  //#region Stuff to do

  public CheckAndRenewTokenIfNeeded() {
    return new Observable(obs => {
      this.renewTokenReqSubjFinished.subscribe(x => {
        obs.next();
        obs.complete();
      });
      this.renewTokenReqSubj.next();
    });
  }

  //#endregion

  //#region POST
  public MakePostRequest<TResponse>(path: string, body: any): Observable<HttpResponse<TResponse>> {
    const res = this.http.post<TResponse>(MAIN_API_URL + path, body, {
      observe: 'response'
    });
    return res;
  }

  public MakeSecurePostRequest<TResponse>(path: string, body: any): Observable<HttpResponse<TResponse>> {
    if (!this.userAuthService.IsLoggedIn()) {
      return null;
    }

    return this.CheckAndRenewTokenIfNeeded().pipe(
      mergeMap(ans => {
        return this.http.post<TResponse>(MAIN_API_URL + path, body, {
          headers: { Authorization: 'Bearer ' + this.userAuthService.GetToken() },
          observe: 'response'
        });
      })
    );
  }
  //#endregion

  //#region PUT
  public MakePutRequest<TResponse>(path: string, body: any): Observable<HttpResponse<TResponse>> {
    const res = this.http.put<TResponse>(MAIN_API_URL + path, body, {
      observe: 'response'
    });
    return res;
  }

  public MakeSecurePutRequest<TResponse>(path: string, body: any): Observable<HttpResponse<TResponse>> {
    if (!this.userAuthService.IsLoggedIn()) {
      return null;
    }

    return this.CheckAndRenewTokenIfNeeded().pipe(
      mergeMap(ans => {
        return this.http.put<TResponse>(MAIN_API_URL + path, body, {
          headers: { Authorization: 'Bearer ' + this.userAuthService.GetToken() },
          observe: 'response'
        });
      })
    );
  }
  //#endregion

  //#region GET
  public MakeGetRequest<TResponse>(path: string): Observable<HttpResponse<TResponse>> {
    return this.http.get<TResponse>(MAIN_API_URL + path, {
      observe: 'response'
    });
  }

  public MakeSecureGetRequest<TResponse>(path: string): Observable<HttpResponse<TResponse>> {
    if (!this.userAuthService.IsLoggedIn()) {
      return null;
    }

    return this.CheckAndRenewTokenIfNeeded().pipe(
      mergeMap(ans => {
        return this.http.get<TResponse>(MAIN_API_URL + path, {
          headers: { Authorization: 'Bearer ' + this.userAuthService.GetToken() },
          observe: 'response'
        });
      })
    );
  }
  //#endregion

  //#region DELETE
  public MakeDeleteRequest<TResponse>(path: string): Observable<HttpResponse<TResponse>> {
    return this.http.delete<TResponse>(MAIN_API_URL + path, {
      observe: 'response'
    });
  }

  public MakeSecureDeleteRequest<TResponse>(path: string): Observable<HttpResponse<TResponse>> {
    if (!this.userAuthService.IsLoggedIn()) {
      return null;
    }

    return this.CheckAndRenewTokenIfNeeded().pipe(
      mergeMap(ans => {
        return this.http.delete<TResponse>(MAIN_API_URL + path, {
          headers: { Authorization: 'Bearer ' + this.userAuthService.GetToken() },
          observe: 'response'
        });
      })
    );
  }
  //#endregion
}
