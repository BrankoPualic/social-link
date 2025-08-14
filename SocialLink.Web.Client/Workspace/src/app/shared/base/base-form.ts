import { FormBuilder, FormGroup } from "@angular/forms";
import { BaseComponentGeneric } from "./base";

/**
 * Import ReactiveFormsModule inside component imports if it's standalone component.
 */
export abstract class BaseFormComponent<T extends object> extends BaseComponentGeneric<T> {
  form: FormGroup;

  constructor(
    protected fb: FormBuilder
  ) {
    super();

    this.form = this.fb.group({});
  }

  abstract initializeForm(): void;
  abstract submit(): void;
}
