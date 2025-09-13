import { Component } from "@angular/core";
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from "../../core/services/auth.service";

@Component({
  selector: 'app-navigation',
  imports: [RouterLink, RouterLinkActive],
  template: `
    <div class="navigation">
      <div class="mb-4 logo" routerLink="">
        <img src="../../../../../assets/images/logo-pink.svg" alt="Pink logo" />
      </div>

      <ul>
        <li routerLink="" routerLinkActive="active" [routerLinkActiveOptions]="{ exact: true }"><i class="fa-regular fa-house"></i> Home</li>
        <li routerLinkActive="active"><i class="fa-solid fa-magnifying-glass"></i> Search</li>
        <li routerLinkActive="active"><i class="fa-regular fa-message"></i> Inbox</li>
        <li routerLinkActive="active"><i class="fa-regular fa-bell"></i> Notifications</li>

        <li [routerLink]="['/profile', userId]" routerLinkActive="active"><i class="fa-regular fa-user"></i> Profile</li>
        <li><i class="fa-solid fa-arrow-right-from-bracket"></i> Log out</li>
      </ul>
    </div>
  `,
  styles: `
    @import '../../../assets/styles/variables.scss';

    .navigation {
      width: 16.5rem;
      min-height: 100dvh;
      position: sticky;
      background-color: $white;
      box-shadow: $box-shadow;
      padding: 1.5rem 0;

      .logo {
        cursor: pointer;
        padding: 0 2rem;

        img {
          width: 12.5rem
        }
      }

      ul li {
        cursor: pointer;
        margin-bottom: 1rem;
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
}
