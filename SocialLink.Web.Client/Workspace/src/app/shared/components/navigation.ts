import { Component } from "@angular/core";
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from "../../core/services/auth.service";

@Component({
  selector: 'app-navigation',
  imports: [RouterLink, RouterLinkActive],
  template: `
    <div class="d-flex align-items-center justify-content-between col-12 py-1 py-md-2 px-4 navigation">
      <div class="logo d-none d-md-block" routerLink="">
        <img src="../../../../../assets/images/logo-pink.svg" alt="Pink logo" />
      </div>

      <ul class="d-flex mb-0 ps-0 flex-grow-1 flex-md-grow-0 justify-content-between">
        <li class="m-2" routerLink="" routerLinkActive="active" [routerLinkActiveOptions]="{ exact: true }"><i class="fa-regular fa-house"></i><span class="d-md-inline d-none"> Home</span></li>
        <li class="m-2" routerLinkActive="active"><i class="fa-solid fa-magnifying-glass"></i><span class="d-md-inline d-none"> Search</span></li>
        <li class="m-2" routerLinkActive="active"><i class="fa-regular fa-message"></i><span class="d-md-inline d-none"> Inbox</span></li>
        <li class="m-2" [routerLink]="['/notifications']" routerLinkActive="active"><i class="fa-regular fa-bell"></i><span class="d-md-inline d-none"> Notifications</span></li>

        <li class="m-2" [routerLink]="['/profile', userId]" routerLinkActive="active"><i class="fa-regular fa-user"></i><span class="d-md-inline d-none"> Profile</span></li>
        <li class="m-2" (click)="logout()"><i class="fa-solid fa-arrow-right-from-bracket"></i><span class="d-md-inline d-none"> Log out</span></li>
      </ul>
    </div>
  `,
  styles: `
    @import '../../../assets/styles/variables.scss';

    .navigation {
      box-shadow: $box-shadow2;
      .logo {
        cursor: pointer;

        img {
          width: 12.5rem
        }
      }

      ul li {
        cursor: pointer;
        font-weight: 500;

        &.active {
          color: $primary;
        }
      }
    }
  `
})
export class Navigation {
  userId?: string;

  constructor(private authService: AuthService) {
    this.userId = this.authService.getUserId();
  }

  logout = () => this.authService.logout();
}
