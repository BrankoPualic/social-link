import { Component } from '@angular/core';
import { BaseFormComponent } from '../../../../shared/base/base-form';
import { UserModel } from '../../models/user.model';
import { Navigation } from '../../../../shared/components/navigation';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Dropdown } from '../../../../shared/components/forms/dropdown';
import { InputText } from '../../../../shared/components/forms/input-text';
import { TextArea } from '../../../../shared/components/forms/text-area';
import { LookupService } from '../../../../core/services/lookup.service';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { ErrorService } from '../../../../core/services/error.service';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';
import { finalize, take } from 'rxjs';
import { Router } from '@angular/router';
import { InputDate } from '../../../../shared/components/forms/input-date';
import { Functions } from '../../../../shared/functions';

@Component({
  selector: 'app-edit-profile',
  imports: [Navigation, ReactiveFormsModule, InputText, Dropdown, TextArea, InputDate],
  templateUrl: './edit-profile.html',
  styleUrl: './edit-profile.scss'
})
export class EditProfile extends BaseFormComponent<UserModel>{
  userId?: string;
  constructor(
    loaderService: PageLoaderService,
    fb: FormBuilder,
    public lookupService: LookupService,
    private errorService: ErrorService,
    private apiService: ApiService,
    private authService: AuthService,
    private router: Router
  ) {
    super(loaderService, fb);

    this.userId = this.authService.getUserId();
    this.load();
  }

  load(): void {
    this.loading = true;
    this.apiService.get<UserModel>(`/users/profile/${this.userId}`)
      .pipe(
        take(1),
        finalize(() => this.loading = false)
    ).subscribe({
      next: user => {
        this.form.patchValue({
          ...user,
          [this.nameof(_ => _.dateOfBirth)]: Functions.formatDateForInput(user.dateOfBirth)
        })
      }
    })
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      [this.nameof(_ => _.id)]: [],
      [this.nameof(_ => _.firstName)]: [],
      [this.nameof(_ => _.lastName)]: [],
      [this.nameof(_ => _.username)]: [],
      [this.nameof(_ => _.genderId)]: [],
      [this.nameof(_ => _.dateOfBirth)]: [],
      [this.nameof(_ => _.biography)]: [],
      [this.nameof(_ => _.isPrivate)]: []
    })
  }

  override submit(): void {
    this.loading = true;
    this.errorService.clean();

    this.apiService.post('/users/update', this.form.value)
      .pipe(
        take(1),
        finalize(() => this.loading = false)
    ).subscribe({
      next: () => this.router.navigateByUrl(`/profile/${this.userId}`),
      error: _ => this.errorService.add(_.error.errors)
    })
  }

  cancel() {
    this.router.navigateByUrl(`/profile/${this.userId}`);
  }
}
