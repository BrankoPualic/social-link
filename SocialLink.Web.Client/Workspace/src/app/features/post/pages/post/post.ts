import { Component, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseComponentGeneric } from '../../../../shared/base/base';
import { PostModel } from '../../models/post.model';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { finalize, take } from 'rxjs';
import { ApiService } from '../../../../core/services/api.service';
import { Navigation } from '../../../../shared/components/navigation';
import { CommonModule } from '@angular/common';
import { Functions } from '../../../../shared/functions';
import { AuthService } from '../../../../core/services/auth.service';
import { HeartIcon } from '../../../../shared/components/heart-icon';
import { MessageBox } from '../../../../shared/components/message-box';
import { Comments } from '../../../comment/components/comments/comments';

@Component({
  selector: 'app-post',
  imports: [Navigation, CommonModule, HeartIcon, MessageBox, Comments],
  templateUrl: './post.html',
  styleUrl: './post.scss'
})
export class Post extends BaseComponentGeneric<PostModel> {
  userId?: string;
  postId?: string;
  post?: PostModel;
  reloadComments = signal<boolean>(false);

  activeCarouselMedia = 0;

  constructor(
    loaderService: PageLoaderService,
    private authService: AuthService,
    private apiService: ApiService,
    private route: ActivatedRoute
  ) {
    super(loaderService);

    this.route.paramMap.subscribe(params => {
      this.postId = params.get('id')!;
      this.load();
    });

    this.userId = this.authService.getUserId();
  }

  load(): void {
    this.loading = true;

    this.apiService.get<PostModel>(`/posts/${this.postId}`)
      .pipe(
        take(1),
        finalize(() => this.loading = false)
      )
      .subscribe({
        next: result => this.post = result
      });
  }

  likePost(): void {
    this.apiService.post('/posts/like', { postId: this.postId, userId: this.userId })
      .pipe(
        take(1)
      )
      .subscribe({
        next: () => { },
        error: _ => console.error(_.error.errors)
      });
  }

  createComment(e: string) {
    if (!e) return;

    this.apiService.post<string>('/posts/comments/create', {
      userId: this.userId,
      postId: this.postId,
      message: e
    })
      .pipe(
        take(1)
      )
      .subscribe({
        next: commentId => this.reloadComments.set(true),
        error: _ => console.error(_.error.errors)
      });
  }

  previousMedia = () => this.activeCarouselMedia--;
  nextMedia = () => this.activeCarouselMedia++;

  formatString = (value: string | undefined) => !!value && Functions.formatString(value);
}
