import { Component, Self, input } from "@angular/core";
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from "@angular/forms";
import { ValidationDirective } from "../../directives/validation.directive";
import { RequiredFieldMark } from "./required-field-mark";

@Component({
  selector: 'app-input-date',
  standalone: true,
  imports: [RequiredFieldMark, ReactiveFormsModule, ValidationDirective],
  template: `
    <div class="mb-3 d-flex flex-column" appValidation="{{ validation() }}">
      <div class="d-flex flex-md-row flex-column align-items-md-center">
        <label class="col-12 col-md-3">
          {{ label() }} @if (required()) {<app-required-field-mark />}
        </label>

        <div class="col-12 col-md-9">
          <input type="date"
                 class="form-control"
                 [formControl]="control" />
        </div>
      </div>
    </div>
  `
})
export class InputDate implements ControlValueAccessor {
  label = input('');
  required = input(false);
  validation = input('');

  constructor(
    @Self() public ngControl: NgControl
  ) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void { }
  registerOnChange(fn: any): void { }
  registerOnTouched(fn: any): void { }
  setDisabledState?(isDisabled: boolean): void { }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}
