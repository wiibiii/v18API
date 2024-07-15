import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, map, of, take } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { SharedService } from '../shared/shared.service';
import { User } from '../shared/models/acount/user';
import { Register } from '../shared/models/acount/register';
import { environment } from '../../environments/environment';
import { RegisterWithExternal } from '../shared/models/acount/registerWithExternal';
import { ConfirmEmail } from '../shared/models/acount/confirmEmail';
import { Login } from '../shared/models/acount/login';
import { LoginWithExternal } from '../shared/models/acount/loginWithExternal';
import { ResetPassword } from '../shared/models/acount/resetPassword';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();
  refreshTokenTimeout: any;
  timeoutId: any;

  constructor(
    private _http: HttpClient,
    private router: Router,
    private sharedService: SharedService
  ) {}

  register(model: Register) {
    return this._http.post(`${environment.appUrl}account/register`, model);
  }

  refreshToken = async () => {
    this._http
      .post<User>(
        `${environment.appUrl}account/refresh-token`,
        {},
        { withCredentials: true }
      )
      .subscribe({
        next: (user: User) => {
          if (user) {
            this.setUser(user);
          }
        },
        error: (error) => {
          this.sharedService.showNotification(false, 'Error', error.error);
          this.logout();
        },
      });
  };

  registerWithThirdParty(model: RegisterWithExternal) {
    return this._http
      .post<User>(
        `${environment.appUrl}account/register-with-third-party`,
        model
      )
      .pipe(
        map((user: User) => {
          if (user) {
            this.setUser(user);
          }
        })
      );
  }

  confirmEmail(model: ConfirmEmail) {
    return this._http.put(`${environment.appUrl}account/confirm-email`, model);
  }

  resendEmailConfirmationLink(email: string) {
    return this._http.post(
      `${environment.appUrl}account/resend-email-confirmation-link/${email}`,
      {}
    );
  }

  forgotUsernameOrPassword(email: string) {
    return this._http.post(
      `${environment.appUrl}account/forgot-username-or-password/${email}`,
      {}
    );
  }

  resetPassword(model: ResetPassword) {
    return this._http.put(`${environment.appUrl}account/reset-password`, model);
  }

  refreshUser(jwt: string | null) {
    if (jwt === null) {
      this.userSource.next(null);
      return of(undefined);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', 'Bearer ' + jwt);

    return this._http
      .get<User>(`${environment.appUrl}account/refresh-page`, {
        headers,
        withCredentials: true,
      })
      .pipe(
        map((user: User) => {
          if (user) {
            this.setUser(user);
          }
        })
      );
  }

  login(model: Login) {
    console.log(`${environment.appUrl}account/login`);
    return this._http
      .post<User>(`${environment.appUrl}account/login`, model, {
        withCredentials: true,
      })
      .pipe(
        map((user: User) => {
          if (user) {
            this.setUser(user);
            //return user;
          }
          //return null;
        })
      );
  }

  loginWithThirdParty(model: LoginWithExternal) {
    return this._http
      .post<User>(
        `${environment.appUrl}account/login-with-third-party`,
        model,
        { withCredentials: true }
      )
      .pipe(
        map((user: User) => {
          if (user) {
            this.setUser(user);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem(environment.userKey);
    this.sharedService.displayExpiringSessionModal = false;
    clearTimeout(this.timeoutId);
    this.userSource.next(null);
    this.router.navigateByUrl('/');
    this.stopRefreshTokenTimer();
  }

  getJwt() {
    if (typeof window !== 'undefined' && window.localStorage) {
      const key = localStorage.getItem(environment.userKey);
      if (key) {
        const user: User = JSON.parse(key);
        return user.jwt;
      } else {
        return null;
      }
    }
    return null;
  }

  checkUserIdleTimeout() {
    this.user$.pipe(take(1)).subscribe({
      next: (user: User | null) => {
        //the user is logged in
        if (user) {
          //if not currently displaying expiring session modal
          if (!this.sharedService.displayExpiringSessionModal) {
            this.timeoutId = setTimeout(() => {
              this.sharedService.displayExpiringSessionModal = true;
              this.sharedService.openExpirySessionCountdown();
              // in 10 minutes of user inactivity
            }, 10 * 1000);
          }
        }
      },
    });
  }

  private setUser(user: User) {
    this.stopRefreshTokenTimer();
    this.startRefreshTokenTimer(user.jwt);
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userSource.next(user);

    this.sharedService.displayExpiringSessionModal = false;
    this.checkUserIdleTimeout();

    // this.user$.subscribe({
    //   next: response =>
    //     console.log(response)
    // });
  }

  private startRefreshTokenTimer(jwt: string) {
    const decodedToken: any = jwtDecode(jwt);

    //expires in seconds
    const expires = new Date(decodedToken.exp * 1000);
    //30 seconds before the expirry
    const timeout = expires.getTime() - Date.now() - 30 * 1000;
    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
