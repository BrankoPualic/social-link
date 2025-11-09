import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable, catchError, throwError } from "rxjs";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private toastrService: ToastrService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    req = req.clone({
      withCredentials: true,
    });

    return next.handle(req).pipe(
      catchError(error => {

        if (error instanceof HttpErrorResponse) {
        console.log(error.error);
          if (error.status === 404) {
            this.router.navigateByUrl('/error/not-found');
          }
          else if (error.status === 500) {
            this.toastrService.error(error.error.errors.map((_: any) => `${_.value}\n`));
          }
        }

        return throwError(() => error);
      })
    )
  }
}

export const errorInterceptorProvider = [
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
];
