import { Component, OnInit, Self, input } from "@angular/core";
import { RequiredFieldMark } from "./required-field-mark";
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from "@angular/forms";
import { ValidationDirective } from "../../directives/validation.directive";
import { Observable, take } from "rxjs";
import { Lookup } from "../../../core/models/lookup";

@Component({
  selector: 'app-dropdown',
  standalone: true,
  imports: [RequiredFieldMark, ReactiveFormsModule, ValidationDirective],
  template: `
    <div class="mb-3 d-flex flex-column" appValidation="{{ validation() }}">
      <div class="d-flex flex-md-row flex-column align-items-md-center">
        <label class="col-12 col-md-3">
          {{ label() }} @if (required()) {<app-required-field-mark />}
        </label>

        <div class="col-12 col-md-9">
          <select class="form-select" [formControl]="control">
            @for (item of data; track $index) {
              <option [value]="item.id">{{ item.description }}</option>
            }
          </select>
        </div>
      </div>
    </div>
  `
})
export class Dropdown implements ControlValueAccessor, OnInit {
  label = input('');
  required = input(false);
  validation = input('');
  loadMethod = input<Observable<Lookup[]>>();
  data: Lookup[] = [];

  constructor(
    @Self() public ngControl: NgControl
  ) {
    this.ngControl.valueAccessor = this;
  }

  ngOnInit(): void {
    if (this.loadMethod()) {
      this.loadMethod()?.pipe(take(1)).subscribe({
        next: _ => this.data = _
      });
    }
  }

  writeValue(obj: any): void { }
  registerOnChange(fn: any): void { }
  registerOnTouched(fn: any): void { }
  setDisabledState?(isDisabled: boolean): void { }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}
