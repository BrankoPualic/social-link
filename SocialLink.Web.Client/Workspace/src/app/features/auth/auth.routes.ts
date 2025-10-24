import { Routes } from "@angular/router";

export const authRoutes: Routes = [
  {
    path: "auth/login",
    title: "Login",
    loadComponent: () => import("./pages/login/login").then(_ => _.Login)
  },
  {
    path: "auth/signup",
    title: "Signup",
    loadComponent: () => import("./pages/signup/signup").then(_ => _.Signup)
  }
];
