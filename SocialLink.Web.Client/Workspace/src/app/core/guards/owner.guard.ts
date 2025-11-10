import { inject } from "@angular/core";
import { CanActivateFn } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { catchError, map, of } from "rxjs";

export const ownerGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const userId = route.paramMap.get('id');

  return authService.isLoggedIn().pipe(
    map(_ => authService.getUserId() === userId),
    catchError(() => of(false))
  );
}
