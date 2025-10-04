import { Routes } from '@angular/router';
import { authRoutes } from './features/auth/auth.routes';
import { homeRoutes } from './features/home/home.routes';
import { errorRoutes } from './features/errors/error.routes';
import { userRoutes } from './features/user/user.routes';
import { notificationRoutes } from './features/notifications/notification.routes';
import { postRoutes } from './features/post/post.routes';

export const routes: Routes = [
  ...authRoutes,
  ...homeRoutes,
  ...errorRoutes,
  ...userRoutes,
  ...notificationRoutes,
  ...postRoutes,

  {
    path: "**",
    redirectTo: "",
    pathMatch: "full"
  }
];
