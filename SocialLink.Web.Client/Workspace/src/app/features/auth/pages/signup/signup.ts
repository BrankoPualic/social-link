import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { InputText } from '../../../../shared/components/forms/input-text';
import { Router, RouterLink } from '@angular/router';
import { BaseFormComponent } from '../../../../shared/base/base-form';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { SignupModel } from '../../models/signup.model';
import { AuthService } from '../../../../core/services/auth.service';
import { ErrorService } from '../../../../core/services/error.service';
import { finalize } from 'rxjs/internal/operators/finalize';
import { Dropdown } from '../../../../shared/components/forms/dropdown';
import { LookupService } from '../../../../core/services/lookup.service';
import { IFileUploadForm } from '../../../../shared/interfaces/file-upload-form.interface';

@Component({
  selector: 'app-signup',
  imports: [ReactiveFormsModule, InputText, Dropdown, RouterLink],
  templateUrl: './signup.html'
})
export class Signup extends BaseFormComponent<SignupModel> implements IFileUploadForm {
  file?: File;

  constructor(
    loaderService: PageLoaderService,
    fb: FormBuilder,
    public lookupService: LookupService,
    private authService: AuthService,
    private errorService: ErrorService,
    private router: Router
  ) {
    super(loaderService, fb);
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      [this.nameof(_ => _.firstName)]: [],
      [this.nameof(_ => _.lastName)]: [],
      [this.nameof(_ => _.username)]: [],
      [this.nameof(_ => _.email)]: [],
      [this.nameof(_ => _.password)]: [],
      [this.nameof(_ => _.repeatPassword)]: [],
      [this.nameof(_ => _.genderId)]: [],
      [this.nameof(_ => _.dateOfBirth)]: [],
      [this.nameof(_ => _.isPrivate)]: [false]
    });
  }

  override submit(): void {
    this.loading = true;
    this.errorService.clean();

    this.authService.signup(this.form.value, this.file)
      .pipe(
        finalize(() => this.loading = false)
      ).subscribe({
        next: () => this.router.navigateByUrl('/'),
        error: _ => this.errorService.add(_.error.errors)
      });
  }

  onFileChange(e: Event): void {
    const input = e.target as HTMLInputElement;
    if (!input.files?.length)
      return;

    this.file = input.files[0];
  }
}
