import { AfterViewInit, Component, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { GridColumn, GridOptions } from "../../../../core/models/grid.model";
import { ApiService } from "../../../../core/services/api.service";
import { PageLoaderService } from "../../../../core/services/page-loader.service";
import { BaseComponentGeneric } from "../../../../shared/base/base";
import { Navigation } from "../../../../shared/components/navigation/navigation";
import { UserModel } from "../../../user/models/user.model";
import { finalize, lastValueFrom, take } from "rxjs";
import { Grid } from "../../../../shared/components/grid/grid";
import { PagedResponse } from "../../../../core/models/paged-response";
import { RouterLink } from "@angular/router";
import { DatePipe } from "@angular/common";
import { DialogService } from "../../../../core/services/dialog.service";

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.html',
  imports: [Navigation, Grid, RouterLink, DatePipe]
})
export class AdminUsers extends BaseComponentGeneric<UserModel> implements OnInit, AfterViewInit {
  gridOptions!: GridOptions;
  @ViewChild('userLinkCell', { read: TemplateRef }) userLinkCell!: TemplateRef<any>;
  @ViewChild('dobCell', { read: TemplateRef }) dobCell!: TemplateRef<any>;
  @ViewChild('createdOnCell', { read: TemplateRef }) createdOnCell!: TemplateRef<any>;
  @ViewChild('deleteCell', { read: TemplateRef }) deleteCell!: TemplateRef<any>;

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

  // TODO: This is working but its really bad implementation, find another way
  ngAfterViewInit() {
    // attach the template AFTER view init (when ViewChild exists)
    this.gridOptions.columns[0].template = this.userLinkCell;
    this.gridOptions.columns[4].template = this.dobCell;
    this.gridOptions.columns[5].template = this.createdOnCell;
    this.gridOptions.columns[6].template = this.deleteCell;

    // if you're using signals and the grid doesn't update,
    // you may need to replace the array reference:
    this.gridOptions = {
      ...this.gridOptions,
      columns: [...this.gridOptions.columns]
    };
  }

  gridInit(): void {
    this.gridOptions = {
      columns: [
        {
          title: 'User ID',
          field: this.nameof(_ => _.id),
          width: 310
        },
        {
          title: 'Username',
          field: this.nameof(_ => _.username),
          width: 100
        },
        {
          title: 'Full Name',
          field: this.nameof(_ => _.fullName),
          width: 200
        },
        {
          title: 'Email',
          field: this.nameof(_ => _.email),
          width: 250
        },
        {
          title: 'Date of Birth',
          field: this.nameof(_ => _.dateOfBirth),
          width: 200
        },
        {
          title: 'Created On',
          field: this.nameof(_ => _.createdOn),
          width: 200
        },
        {
          title: '',
          width: 150
        }
      ] as GridColumn[],
      scrollable: true,
      height: 'calc(100dvh - 200px)',
      read: async () => {
        this.loading = true;
        return lastValueFrom(this.apiService.post<PagedResponse<UserModel>>('/User/Search', {})
          .pipe(
            take(1),
            finalize(() => this.loading = false),
          ));
      }
    } as GridOptions;
  }

  changeActiveStatus(user: UserModel) {
    this.dialogService.confirm(`Are you sure you want to ${user.isActive ? 'deactivate' : 'activate'} user '${user.username}' ?`, `${user.isActive ? 'Deactivate' : 'Activate'} User`)
      .result
      .then(() => {
        this.loading = true;
        this.apiService.post<void>('/User/UpdateActiveStatus', {}, { params: { userId: user.id } })
          .pipe(
            take(1),
            finalize(() => this.loading = false)
        ).subscribe({
          next: () => this.gridOptions.read()
        })
      })
  }

  changeLockStatus(user: UserModel) {
    this.dialogService.confirmDelete(`Are you sure you want to lock user '${user.username}' ?`, 'Lock User')
      .result
      .then(() => {
        this.loading = true;
        this.apiService.post<void>('/User/UpdateLockStatus', {}, { params: { userId: user.id } })
          .pipe(
            take(1),
            finalize(() => this.loading = false)
          ).subscribe({
            next: () => this.gridOptions.read()
          })
      });
  }

  getStatusTitle = (user: UserModel) => user.isActive ? 'Deactivate' : 'Activate';
  getStatusClsas = (user: UserModel) => user.isActive ? 'fa-xmark' : 'fa-check';

  getLockTitle = (user: UserModel) => user.isLocked ? 'Unlock' : 'Lock';
  getLockClass = (user: UserModel) => user.isLocked ? 'fa-unlock' : 'fa-lock';
}
