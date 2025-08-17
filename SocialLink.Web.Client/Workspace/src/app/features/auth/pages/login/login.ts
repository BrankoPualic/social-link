import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseFormComponent } from '../../../../shared/base/base-form';
import { LoginModel } from '../../models/login-model';
import { InputText } from '../../../../shared/components/forms/input-text';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { ErrorService } from '../../../../core/services/error.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, InputText, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login extends BaseFormComponent<LoginModel> implements OnInit {
  constructor(
    loaderService: PageLoaderService,
    fb: FormBuilder,
    private authService: AuthService,
    private errorService: ErrorService,
    private router: Router
  ) {
    super(loaderService, fb);
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      email: [''],
      password: ['']
    })
  }

  override submit(): void {
    this.loading = true;

    this.authService.login(this.form.value)
      .pipe(
        finalize(() => this.loading = false)
      ).subscribe({
        next: () => { console.log('next'), this.router.navigateByUrl('/') },
        error: (_) => this.errorService.add(_.error.errors)
      });
  }
}
