import { Routes } from "@angular/router";

export const adminRoutes: Routes = [
  {
    path: "admin/panel",
    title: "Admin | Panel",
    loadComponent: () => import("./pages/admin-panel/admin-panel").then(_ => _.AdminPanel)
  }
]
