import { Component, OnInit } from '@angular/core';
import { Navigation } from '../../../../shared/components/navigation/navigation';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { ApiService } from '../../../../core/services/api.service';
import { BaseComponent } from '../../../../shared/base/base';
import { AuthService } from '../../../../core/services/auth.service';
import { PagedResponse } from '../../../../core/models/paged-response';
import { PostModel } from '../../../post/models/post.model';
import { finalize, take } from 'rxjs';
import { HeartIcon } from '../../../../shared/components/heart-icon';
import { FormatTextPipe } from '../../../../core/pipes/format-text.pipe';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BlobModel } from '../../../../core/models/blob.model';

@Component({
  selector: 'app-home',
  imports: [Navigation, HeartIcon, FormatTextPipe, CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home extends BaseComponent implements OnInit {
  currentUserId?: string;
  posts?: PagedResponse<PostModel>;

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService,
    private authService: AuthService
  ) {
    super(loaderService);

    this.currentUserId = this.authService.getUserId();
  }

  ngOnInit(): void {
    this.loading = true;
    this.apiService.post<PagedResponse<PostModel>>('/Post/GetList', { userId: this.currentUserId }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe({
      next: response => {
        this.posts = response;
        this.posts.items!.forEach((el: PostModel) => {
          el.activeCarouselMedia = 0;
        })
      }
    });
  }

  likePost(post: PostModel): void {
    this.apiService.post('/Post/UpdateLikeStatus', { postId: post.id, userId: this.currentUserId })
      .pipe(
        take(1)
      )
      .subscribe({
        next: () => { },
        error: _ => console.error(_.error.errors)
      });
  }

  previousMedia = (post: PostModel) => post.activeCarouselMedia--;
  nextMedia = (post: PostModel) => post.activeCarouselMedia++;
}
