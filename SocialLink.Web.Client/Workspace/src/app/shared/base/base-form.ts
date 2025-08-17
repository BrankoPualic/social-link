import { FormBuilder, FormGroup } from "@angular/forms";
import { BaseComponentGeneric } from "./base";
import { PageLoaderService } from "../../core/services/page-loader.service";

/**
 * Import ReactiveFormsModule inside component imports if it's standalone component.
 */
export abstract class BaseFormComponent<T extends object> extends BaseComponentGeneric<T> {
  form: FormGroup;

  constructor(
    loaderService: PageLoaderService,
    protected fb: FormBuilder
  ) {
    super(loaderService);

    this.form = this.fb.group({});
  }

  abstract initializeForm(): void;
  abstract submit(): void;
}
