import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { BaseFormComponent } from '../../../../shared/base/base-form';
import { LoginModel } from '../../models/login.model';
import { InputText } from '../../../../shared/components/forms/input-text';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { ErrorService } from '../../../../core/services/error.service';
import { finalize } from 'rxjs/internal/operators/finalize';
import { ValidationDirective } from '../../../../shared/directives/validation.directive';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, InputText, RouterLink, ValidationDirective],
  templateUrl: './login.html'
})
export class Login extends BaseFormComponent<LoginModel> {
  constructor(
    loaderService: PageLoaderService,
    fb: FormBuilder,
    private authService: AuthService,
    private errorService: ErrorService,
    private router: Router
  ) {
    super(loaderService, fb);
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      [this.nameof(_ => _.email)]: [''],
      [this.nameof(_ => _.password)]: ['']
    });
  }

  override submit(): void {
    this.loading = true;
    this.errorService.clean();

    this.authService.login(this.form.value)
      .pipe(
        finalize(() => this.loading = false)
      ).subscribe({
        next: () => this.router.navigateByUrl('/'),
        error: _ => this.errorService.add(_.error.errors)
      });
  }
}
