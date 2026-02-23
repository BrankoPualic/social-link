import { AfterViewInit, Component, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { Grid } from "../../../../shared/components/grid/grid";
import { BaseComponentGeneric } from "../../../../shared/base/base";
import { PostModel } from "../../../post/models/post.model";
import { GridColumn, GridOptions } from "../../../../core/models/grid.model";
import { PageLoaderService } from "../../../../core/services/page-loader.service";
import { ApiService } from "../../../../core/services/api.service";
import { DialogService } from "../../../../core/services/dialog.service";
import { finalize, lastValueFrom, take } from "rxjs";
import { PagedResponse } from "../../../../core/models/paged-response";
import { Navigation } from "../../../../shared/components/navigation/navigation";
import { RouterLink } from "@angular/router";
import { FormatTextPipe } from "../../../../core/pipes/format-text.pipe";
import { DatePipe } from "@angular/common";

@Component({
  selector: 'app-admin-posts',
  templateUrl: './admin-posts.html',
  imports: [Navigation, Grid, RouterLink, FormatTextPipe, DatePipe]
})
export class AdminPosts extends BaseComponentGeneric<PostModel> implements OnInit, AfterViewInit {
  gridOptions!: GridOptions;
  @ViewChild('postLinkCell', { read: TemplateRef }) postLinkCell!: TemplateRef<any>;
  @ViewChild('userLinkCell', { read: TemplateRef }) userLinkCell!: TemplateRef<any>;
  @ViewChild('descriptionCell', { read: TemplateRef }) descriptionCell!: TemplateRef<any>;
  @ViewChild('createdOnCell', { read: TemplateRef }) createdOnCell!: TemplateRef<any>;

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService,
    private dialogService: DialogService
  ) {
    super(loaderService);
  }

  ngOnInit(): void {
    this.gridInit();
  }

  ngAfterViewInit() {
    this.gridOptions.columns[0].template = this.postLinkCell;
    this.gridOptions.columns[1].template = this.userLinkCell;
    this.gridOptions.columns[4].template = this.descriptionCell;
    this.gridOptions.columns[5].template = this.createdOnCell;

    this.gridOptions = {
      ...this.gridOptions,
      columns: [...this.gridOptions.columns]
    }
  }

  gridInit(): void {
    this.gridOptions = {
      columns: [
        {
          title: 'Post ID',
          field: this.nameof(_ => _.id),
          width: 310
        },
        {
          title: 'Username',
          field: this.nameof(_ => _.user?.username),
          width: 100
        },
        {
          title: 'Likes',
          field: this.nameof(_ => _.likesCount),
          width: 100,
          class: 'text-center',
          titleClass: 'text-center'
        },
        {
          title: 'Comments',
          field: this.nameof(_ => _.commentsCount),
          width: 100,
          class: 'text-center',
          titleClass: 'text-center'
        },
        {
          title: 'Description',
          field: this.nameof(_ => _.description),
          width: 300
        },
        {
          title: 'Created On',
          field: this.nameof(_ => _.createdOn),
          width: 200
        }
      ] as GridColumn[],
      scrollable: true,
      height: 'calc(100dvh - 200px)',
      read: async () => {
        this.loading = true;
        return lastValueFrom(this.apiService.post<PagedResponse<PostModel>>('/Post/GetList', {})
          .pipe(
            take(1),
            finalize(() => this.loading = false),
          ));
      }
    } as GridOptions;
  }
}
