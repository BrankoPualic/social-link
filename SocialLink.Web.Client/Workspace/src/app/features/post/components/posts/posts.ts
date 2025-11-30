import { Component, OnChanges, OnInit, SimpleChanges, input } from '@angular/core';
import { PostModel } from '../../models/post.model';
import { ApiService } from '../../../../core/services/api.service';
import { finalize, take } from 'rxjs';
import { PagedResponse } from '../../../../core/models/paged-response';
import { PageLoaderComponent } from '../../../../shared/components/page-loader';
import { RouterLink } from '@angular/router';
import { HeartIcon } from '../../../../shared/components/heart-icon';

@Component({
  selector: 'app-posts',
  imports: [PageLoaderComponent, RouterLink, HeartIcon],
  templateUrl: './posts.html',
  styleUrl: './posts.scss'
})
export class Posts implements OnInit {
  postsLoading = false;
  posts: PostModel[] = [];
  userId = input<string | undefined>(undefined);

  constructor(
    private apiService: ApiService
  ) { }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.postsLoading = true;

    this.apiService.post<PagedResponse<PostModel>>('/Post/GetList', { userId: this.userId() })
      .pipe(
        take(1),
        finalize(() => this.postsLoading = false)
      )
      .subscribe({
        next: result => this.posts = result.items || []
      });
  }
}
