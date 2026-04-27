import { CommonModule } from "@angular/common";
import { Component, input } from "@angular/core";

@Component({
  selector: 'app-heart-icon',
  imports: [CommonModule],
  template: `<i class="fa-heart heart-icon" [ngClass]="{'fa-regular': !status(), 'fa-solid liked': status()}"></i>`,
  styles: `
    .heart-icon {
      cursor: pointer;
      transition: transform 220ms cubic-bezier(0.34, 1.56, 0.64, 1), color 220ms ease-out;
      color: inherit;

      &:hover {
        transform: scale(1.18);
      }

      &.liked {
        color: #ec4366;
        animation: heart-pop 380ms cubic-bezier(0.34, 1.56, 0.64, 1);
      }
    }

    @keyframes heart-pop {
      0% { transform: scale(0.8); }
      50% { transform: scale(1.4); }
      80% { transform: scale(0.95); }
      100% { transform: scale(1); }
    }
  `
})
export class HeartIcon {
  status = input<boolean | undefined>(false);
}
