import { FormBuilder, FormGroup } from "@angular/forms";
import { BaseComponentGeneric } from "./base";
import { PageLoaderService } from "../../core/services/page-loader.service";
import { Directive, OnInit } from "@angular/core";

/**
 * Import ReactiveFormsModule inside component imports if it's standalone component.
 */
@Directive()
export abstract class BaseFormComponent<T extends object> extends BaseComponentGeneric<T> implements OnInit {
  form: FormGroup;

  constructor(
    loaderService: PageLoaderService,
    protected fb: FormBuilder
  ) {
    super(loaderService);

    this.form = this.fb.group({});
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  abstract initializeForm(): void;
  abstract submit(): void;
}
