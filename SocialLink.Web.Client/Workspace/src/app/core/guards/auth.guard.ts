import { inject } from "@angular/core";
import { CanActivateFn } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { map, catchError, of } from "rxjs";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);

  return authService.isLoggedIn().pipe(
    map(isLoggedIn => {
      if (isLoggedIn)
        return true;

      return false;
    }),
    catchError(() => {
      return of(false);
    })
  );
}
