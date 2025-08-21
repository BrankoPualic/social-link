import { Routes } from '@angular/router';
import { authRoutes } from './features/auth/auth.routes';
import { homeRoutes } from './features/home/home.routes';
import { errorRoutes } from './features/errors/error.routes';

export const routes: Routes = [
  ...authRoutes,
  ...homeRoutes,
  ...errorRoutes,

  {
    path: "**",
    redirectTo: "",
    pathMatch: "full"
  }
];
