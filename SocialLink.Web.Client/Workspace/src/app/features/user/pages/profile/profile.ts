import { Component } from '@angular/core';
import { Navigation } from '../../../../shared/components/navigation';
import { ApiService } from '../../../../core/services/api.service';
import { BaseComponentGeneric } from '../../../../shared/base/base';
import { UserModel } from '../../models/user.model';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { AuthService } from '../../../../core/services/auth.service';
import { finalize, forkJoin, of, take } from 'rxjs';
import { CommonModule } from '@angular/common';
import { eGender } from '../../../../core/enumerators/gender.enum';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [Navigation, CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile extends BaseComponentGeneric<UserModel> {
  userId?: string;
  user?: UserModel;
  currentUserId?: string;
  isCurrentUser = false;
  isFollowed = false;

  constructor(
    loaderService: PageLoaderService,
    private authService: AuthService,
    private apiService: ApiService,
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
      isFollowed: this.isCurrentUser
        ? of(false)
        : this.apiService.post<boolean>('/users/checkFollowStatus', {
          followerId: this.currentUserId,
          followingId: this.userId
        })
    }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe(result => {
      this.user = result.user;
      this.isFollowed = result.isFollowed;
    })
  }

  getProfileImage(): string {
    return this.user?.profileImage?.url || `./assets/images/${this.user?.genderId === eGender.Male ? `man.png` : `woman.png`}`;
  }
}
