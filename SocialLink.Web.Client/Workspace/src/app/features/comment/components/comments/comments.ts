import { Component, OnChanges, SimpleChanges, input } from '@angular/core';
import { CommentModel } from '../../models/comment.model';
import { ApiService } from '../../../../core/services/api.service';
import { PagedResponse } from '../../../../core/models/paged-response';
import { finalize, take } from 'rxjs';
import { PageLoaderComponent } from '../../../../shared/components/page-loader';
import { HeartIcon } from '../../../../shared/components/heart-icon';
import { Functions } from '../../../../shared/functions';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-comments',
  imports: [PageLoaderComponent, HeartIcon, DatePipe, RouterLink],
  templateUrl: './comments.html',
  styleUrl: './comments.scss'
})
export class Comments implements OnChanges {
  postId = input<string | undefined>(undefined);
  commentsLoading = false;
  comments: CommentModel[] = [];

  constructor(
    private apiService: ApiService,
    private authService: AuthService
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['postId'] && changes['postId'].currentValue) {
      this.load();
    }
  }

  load(): void {
    this.commentsLoading = true;

    this.apiService.post<PagedResponse<CommentModel>>('/Comment/Get', { postId: this.postId() })
      .pipe(
        take(1),
        finalize(() => this.commentsLoading = false)
      )
      .subscribe({
        next: result => this.comments = result.items || []
      })
  }

  likeComment(commentId?: string): void {
    if (!commentId) return;
    this.apiService.post('/Comment/UpdateLikeStatus', { commentId: commentId, userId: this.authService.getUserId() })
      .pipe(
        take(1)
      )
      .subscribe({
        next: () => { },
        error: _ => console.error(_.error.errors)
      });
  }

  formatString = (value: string | undefined) => !!value && Functions.formatString(value);
}
