import { CommonModule } from "@angular/common";
import { Component, input } from "@angular/core";

@Component({
  selector: 'app-heart-icon',
  imports: [CommonModule],
  template: `<i class="fa-heart" [ngClass]="{'fa-regular': !status(), 'fa-solid': status()}" style="cursor: pointer" [ngStyle]="{'color': status() ? 'red' : 'default'}"></i>`
})
export class HeartIcon {
  status = input<boolean | undefined>(false);
}
