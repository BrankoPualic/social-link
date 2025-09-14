import { Component, Self, input } from "@angular/core";
import { RequiredFieldMark } from "./required-field-mark";
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from "@angular/forms";
import { ValidationDirective } from "../../directives/validation.directive";

@Component({
  selector: 'app-text-area',
  standalone: true,
  imports: [RequiredFieldMark, ReactiveFormsModule, ValidationDirective],
  template: `
    <div class="mb-3 d-flex flex-column" appValidation="{{ validation() }}">
      <div class="d-flex flex-md-row flex-column">
        <label class="col-12 col-md-3">
          {{ label() }} @if (required()) {<app-required-field-mark />}
        </label>

        <div class="col-12 col-md-9">
          <textarea class="form-control"
                    [formControl]="control"
                    placeholder="{{ placeholder() }}"
                    [maxlength]="maxLength()"
          ></textarea>
          @if (maxLength())
          {
          <div class="text-black-50 small float-end">{{ control.value?.length || 0 }}/{{ maxLength() }}</div>
          }
        </div>
      </div>
    </div>
  `
})
export class TextArea implements ControlValueAccessor {
  label = input('');
  required = input(false);
  placeholder = input('');
  validation = input('');
  maxLength = input(0);

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
