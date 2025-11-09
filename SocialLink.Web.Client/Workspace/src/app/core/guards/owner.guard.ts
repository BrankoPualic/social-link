import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";

export const ownerGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const currentUserId = authService.getUserId();
  const userId = route.paramMap.get('id');
  if (currentUserId === userId)
    return true;

  router.navigateByUrl('/error/unauthorized');
  return false;
}
