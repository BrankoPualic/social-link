import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { BehaviorSubject, Observable, catchError, filter, switchMap, take, throwError } from "rxjs";
import { EventBusService } from "../services/event-bus.service";
import { EventData } from "../models/event-data.model";
import { Constants } from "../../shared/constants";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private _isRefreshing = false;
  private _refreshTokenSubject = new BehaviorSubject<any>(null);

  constructor(
    private router: Router,
    private authService: AuthService,
    private eventBusService: EventBusService,
    private toastrService: ToastrService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    req = req.clone({
      withCredentials: true,
    });

    return next.handle(req).pipe(
      catchError(error => {
        console.log(error.error);

        if (
          error instanceof HttpErrorResponse &&
          !req.url.includes('auth') &&
          error.status === 401
        ) {
          return this.handle401Error(req, next);
        }

        return throwError(() => error);
      })
    );
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this._isRefreshing) {
      this._isRefreshing = true;
      this._refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap(() => {
          this._isRefreshing = false;
          this._refreshTokenSubject.next('something');

          return next.handle(request);
        }),
        catchError(error => {
          this._isRefreshing = false;

          if (error.status == '403') {
            this.eventBusService.emit(new EventData(Constants.logoutEvent, null));
          }

          return throwError(() => error);
        })
      );
    }
    else {
      return this._refreshTokenSubject.pipe(
        filter((_: any) => _ != null),
        take(1),
        switchMap(() => next.handle(request))
      );
    }
  }
}

export const authInterceptorProvider = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
];
