import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseComponentGeneric } from '../../../../shared/base/base';
import { PostModel } from '../../models/post.model';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { finalize, forkJoin, take } from 'rxjs';
import { ApiService } from '../../../../core/services/api.service';
import { CommentModel } from '../../models/comment.model';
import { PagedResponse } from '../../../../core/models/paged-response';
import { Navigation } from '../../../../shared/components/navigation';
import { CommonModule } from '@angular/common';
import { Functions } from '../../../../shared/functions';
import { AuthService } from '../../../../core/services/auth.service';
import { HeartIcon } from '../../../../shared/components/heart-icon';
import { MessageBox } from '../../../../shared/components/message-box';

@Component({
  selector: 'app-post',
  imports: [Navigation, CommonModule, HeartIcon, MessageBox],
  templateUrl: './post.html',
  styleUrl: './post.scss'
})
export class Post extends BaseComponentGeneric<PostModel> {
  userId?: string;
  postId?: string;
  post?: PostModel;
  comments: CommentModel[] = [];

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

    forkJoin({
      post: this.apiService.get<PostModel>(`/posts/${this.postId}`),
      comments: this.apiService.post<PagedResponse<CommentModel>>('/posts/comments', { postId: this.postId })
    })
      .pipe(
        take(1),
        finalize(() => this.loading = false)
      )
      .subscribe(result => {
        this.post = result.post;
        this.comments = result.comments.items!;
      })
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
        next: commentId => this.comments.push({
          // TODO: REmove this part and load comments again
          id: commentId,
          userId: this.userId,
          postId: this.postId,
          message: e,
          createdOn: new Date()
        }),
        error: _ => console.error(_.error.errors)
      });
  }


  previousMedia = () => this.activeCarouselMedia--;
  nextMedia = () => this.activeCarouselMedia++;

  formatString = (value: string | undefined) => !!value && Functions.formatString(value);
}
