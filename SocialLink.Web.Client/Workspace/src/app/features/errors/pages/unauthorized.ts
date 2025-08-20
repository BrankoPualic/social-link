import { Location } from "@angular/common";
import { Component } from "@angular/core";

@Component({
  selector: 'app-unauthorized',
  template: `
    <div class="d-flex flex-column align-items-center pt-3">
      <h2>Access denied!</h2>
      <h3>403</h3>
      <div class="form-btn-group">
        <button class="btn btn-sm align-self-center btn-primary py-2 mt-2" (click)="goBack()">Go Back</button>
      </div>
    </div>
  `
})
export class Unauthorized {
  constructor(private location: Location) { }

  goBack = () => this.location.back();
}
