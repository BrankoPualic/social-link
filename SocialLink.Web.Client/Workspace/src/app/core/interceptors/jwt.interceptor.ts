import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core/primitives/di";
import { AuthService } from "../services/auth.service";

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const token = authService.getToken();

  if (authService.isLoggedIn()) {
    req = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }

  return next(req); // TODO: Add .pipe(delay(500)); if you want to simulate production and loading
};
