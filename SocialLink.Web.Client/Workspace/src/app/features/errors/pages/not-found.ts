import { Location } from "@angular/common";
import { Component } from "@angular/core";

@Component({
  selector: 'app-not-found',
  template: `
    <div class="d-flex flex-column align-items-center pt-3">
      <h2>Page not found!</h2>
      <h3>404</h3>
      <div class="form-btn-group">
        <button class="btn btn-sm align-self-center btn-primary py-2 mt-2" (click)="goBack()">Go Back</button>
      </div>
    </div>
  `
})
export class NotFound {
  constructor(private location: Location) { }

  goBack = () => this.location.back();
}
