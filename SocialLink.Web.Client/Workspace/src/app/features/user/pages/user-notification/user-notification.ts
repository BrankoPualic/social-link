import { Component } from '@angular/core';
import { Navigation } from '../../../../shared/components/navigation/navigation';
import { FormArray, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { BaseFormComponent } from '../../../../shared/base/base-form';
import { UserNotificationModel } from '../../models/user-notification.model';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { ApiService } from '../../../../core/services/api.service';
import { Observable, finalize, take } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-notification',
  imports: [Navigation, ReactiveFormsModule, RouterLink],
  templateUrl: './user-notification.html',
  styleUrl: './user-notification.scss'
})
export class UserNotification extends BaseFormComponent<UserNotificationModel> {
  userId?: string;

  constructor(
    loaderService: PageLoaderService,
    fb: FormBuilder,
    private router: Router,
    private apiService: ApiService,
    private authService: AuthService,
    private toastrService: ToastrService
  ) {
    super(loaderService, fb);

    this.userId = this.authService.getUserId();
  }

  load(): Observable<UserNotificationModel[]> {
    this.loading = true;
    return this.apiService.get<UserNotificationModel[]>(`/UserNotification/GetUserPreferences/${this.userId}`).pipe(
      take(1),
      finalize(() => this.loading = false)
    );
  }

  get preferences(): FormArray {
    return this.form.get('preferences') as FormArray;
  }

  override initializeForm(): void {
    this.load().subscribe({
      next: response => {
        this.form = this.fb.group({
          preferences: this.fb.array(
            response.map(_ => this.fb.group({
              [this.nameof(_ => _.id)]: [_.id],
              [this.nameof(_ => _.userId)]: [_.userId],
              [this.nameof(_ => _.notificationTypeId)]: [_.notificationTypeId],
              [this.nameof(_ => _.name)]: [_.name],
              [this.nameof(_ => _.isMuted)]: [_.isMuted]
            }))
          )
        });
      }
    });
  }

  override submit(): void {
    const changedPreferences = this.form.value.preferences.filter((_: any, i: number) =>
      (this.form.get('preferences') as FormArray).at(i).dirty
    );

    this.loading = true;

    this.apiService.post('/UserNotification/SavePreferences', changedPreferences).pipe(
      take(1),
      finalize(() => this.loading = false)
    )
      .subscribe({
        next: () => {
          this.toastrService.success('Changes saved');
          this.router.navigateByUrl(`/profile/${this.userId}`);
        },
        error: _ => this.toastrService.error(_.error.errors)
      })
  }
}
