import { Routes } from "@angular/router";

export const adminRoutes: Routes = [
  {
    path: "admin/dashboard",
    title: "Admin | Dashboard",
    loadComponent: () => import("./pages/admin-dashboard/admin-dashboard").then(_ => _.AdminDashboard)
  },
  {
    path: "admin/users",
    title: "Admin | Users",
    loadComponent: () => import("./pages/admin-users/admin-users").then(_ => _.AdminUsers)
  },
  {
    path: "admin/posts",
    title: "Admin | Posts",
    loadComponent: () => import("./pages/admin-posts/admin-posts").then(_ => _.AdminPosts)
  }
]
