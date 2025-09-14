import { Component } from '@angular/core';
import { Navigation } from '../../../../shared/components/navigation';
import { ApiService } from '../../../../core/services/api.service';
import { BaseComponentGeneric } from '../../../../shared/base/base';
import { UserModel } from '../../models/user.model';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Observable, finalize, forkJoin, of, switchMap, take } from 'rxjs';
import { CommonModule } from '@angular/common';
import { eGender } from '../../../../core/enumerators/gender.enum';
import { ActivatedRoute } from '@angular/router';
import { eFollowStatus } from '../../../../core/enumerators/follow-status.enum';
import { IFileUploadForm } from '../../../../shared/interfaces/file-upload-form.interface';
import { FileUploadService } from '../../../../core/services/file-upload.service';

@Component({
  selector: 'app-profile',
  imports: [Navigation, CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile extends BaseComponentGeneric<UserModel> implements IFileUploadForm {
  userId?: string;
  user?: UserModel;
  currentUserId?: string;
  isCurrentUser = false;
  followStatus?: eFollowStatus;
  eFollowStatus = eFollowStatus;

  constructor(
    loaderService: PageLoaderService,
    private authService: AuthService,
    private apiService: ApiService,
    private fileUploadService: FileUploadService,
    private route: ActivatedRoute
  ) {
    super(loaderService);

    this.currentUserId = this.authService.getUserId();

    this.route.paramMap.subscribe(params => {
      this.userId = params.get('id')!;
      this.isCurrentUser = this.currentUserId == this.userId;
      this.load();
    });
  }

  load(): void {
    this.loading = true;

    forkJoin({
      user: this.apiService.get<UserModel>(`/users/profile/${this.userId}`),
      followStatus: this.isCurrentUser
        ? of(eFollowStatus.Unknown)
        : this.checkFollowStatus()
    }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe(result => {
      this.user = result.user;
      this.followStatus = result.followStatus;
    })
  }

  follow(): void {
    this.apiService.post('/users/follow', {
      followerId: this.currentUserId,
      followingId: this.userId
    }).pipe(
      switchMap(() => this.checkFollowStatus()),
      take(1)
    ).subscribe({
      next: status => this.followStatus = status
    })
  }

  unfollow(): void {
    this.apiService.post('/users/unfollow', {
      followerId: this.currentUserId,
      followingId: this.userId
    }).pipe(
      switchMap(() => this.checkFollowStatus()),
      take(1)
    ).subscribe({
      next: status => this.followStatus = status
    })
  }

  private checkFollowStatus(): Observable<eFollowStatus> {
    return this.apiService.post<eFollowStatus>('/users/checkFollowStatus', {
      followerId: this.currentUserId,
      followingId: this.userId
    });
  }

  getProfileImage(): string {
    return this.user?.profileImage?.url || `./assets/images/${this.user?.genderId === eGender.Male ? `man.png` : `woman.png`}`;
  }

  onFileChange(e: Event): void {
    const input = e.target as HTMLInputElement;
    if (!input.files?.length)
      return;

    this.loading = true;
    this.fileUploadService.upload('/users/uploadProfileImage', input.files[0], { userId: this.currentUserId })
      .pipe(
        take(1),
        finalize(() => this.loading = false)
      ).subscribe({
        next: () => this.load(),
        //TODO: Handle errors after implementing toast show error message
      });
  }
}
