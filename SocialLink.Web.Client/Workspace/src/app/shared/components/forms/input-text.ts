import { Component, Self, input } from "@angular/core";
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from "@angular/forms";
import { RequiredFieldMark } from "./required-field-mark";

@Component({
  selector: 'app-input-text',
  standalone: true,
  imports: [RequiredFieldMark, ReactiveFormsModule],
  template: `
    <div class="mb-3 d-flex flex-sm-row flex-column">
      <label class="form-label col-12 col-sm-3">{{ label() }} <app-required-field-mark/></label>

      <div class="col-12 col-sm-9">
        <input
          type="{{ type() }}"
          [class.is-invalid]="control.touched && control.invalid"
          class="form-control"
          [formControl]="control"
          placeholder="{{ placeholder() }}"
        />
        <div class="invalid-feedback text-end pe-2">
          @if (control.errors?.['required']) {
            {{ label() }} is required.
          }
          @if (control.errors?.['minlength']) {
            {{ label() }} must be at least {{ control.errors?.['minlength'].requiredLength }} characters.
          }
          @if (control.errors?.['maxlength']) {
            {{ label() }} must be at most {{ control.errors?.['maxlength'].requiredLength }} characters.
          }
          @if (control.errors?.['notmatching']) {
            {{ label() }} do not match.
          }
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
