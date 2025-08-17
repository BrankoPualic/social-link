import { Component, Self, input } from "@angular/core";
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from "@angular/forms";
import { RequiredFieldMark } from "./required-field-mark";
import { ValidationDirective } from "../../directives/validation.directive";

@Component({
  selector: 'app-input-text',
  standalone: true,
  imports: [RequiredFieldMark, ReactiveFormsModule, ValidationDirective],
  template: `
    <div class="mb-3 d-flex flex-column" appValidation="{{ validation() }}">
      <div class="d-flex flex-sm-row flex-column align-items-sm-center">
        <label class="col-12 col-sm-3">{{ label() }} @if(required()){<app-required-field-mark/>}</label>

        <div class="col-12 col-sm-9">
          <input
            type="{{ type() }}"
            class="form-control"
            [formControl]="control"
            placeholder="{{ placeholder() }}"
          />
        </div>
      </div>
    </div>
  `
})
export class InputText implements ControlValueAccessor {
  label = input('');
  type = input('text');
  required = input(false);
  placeholder = input('');
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
