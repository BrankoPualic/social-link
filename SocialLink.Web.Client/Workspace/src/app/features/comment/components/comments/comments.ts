import { DatePipe } from '@angular/common';
import { Component, OnChanges, SimpleChanges, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { finalize, take } from 'rxjs';
import { PagedResponse } from '../../../../core/models/paged-response';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';
import { SharedService } from '../../../../core/services/shared.service';
import { HeartIcon } from '../../../../shared/components/heart-icon';
import { PageLoaderComponent } from '../../../../shared/components/page-loader';
import { CommentModel } from '../../models/comment.model';
import { FormatTextPipe } from '../../../../core/pipes/format-text.pipe';

@Component({
  selector: 'app-comments',
  imports: [PageLoaderComponent, HeartIcon, DatePipe, RouterLink, FormatTextPipe],
  templateUrl: './comments.html',
  styleUrl: './comments.scss'
})
export class Comments implements OnChanges {
  postId = input<string | undefined>(undefined);
  commentsLoading = false;
  comments: CommentModel[] = [];

  constructor(
    private apiService: ApiService,
    private authService: AuthService,
    public sharedService: SharedService
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
}
