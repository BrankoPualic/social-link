import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize, take } from 'rxjs';
import { PagedResponse } from '../../../../core/models/paged-response';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';
import { ErrorService } from '../../../../core/services/error.service';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { SharedService } from '../../../../core/services/shared.service';
import { BaseComponent } from '../../../../shared/base/base';
import { Navigation } from '../../../../shared/components/navigation/navigation';
import { Search } from '../../../../shared/components/search/search';
import { ValidationDirective } from '../../../../shared/directives/validation.directive';
import { UserLightModel } from '../../../user/models/user-light.model';
import { UserModel } from '../../../user/models/user.model';
import { ConversationCreateModel } from '../../models/conversation-create.model';
import { FileUploadService } from '../../../../core/services/file-upload.service';

@Component({
  selector: 'app-create-group',
  imports: [Navigation, Search, RouterLink, FormsModule, ValidationDirective],
  templateUrl: './create-group.html',
  styleUrl: './create-group.scss'
})
export class CreateGroup extends BaseComponent {
  file?: File;
  preview?: string;
  members: UserLightModel[] = [];
  searching = false;
  users?: PagedResponse<UserLightModel>;
  currentUserId?: string;
  name?: string;

  constructor(
    loaderService: PageLoaderService,
    public sharedService: SharedService,
    private apiService: ApiService,
    private authService: AuthService,
    private errorService: ErrorService,
    private fileUploadService: FileUploadService,
    private router: Router
  ) {
    super(loaderService);

    this.currentUserId = this.authService.getUserId();
    this.loadCreator();
  }

  loadCreator(): void {
    this.loading = true;
    this.apiService.get<UserModel>(`/User/Get/${this.currentUserId}`).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe({
      next: response => {
        const user = new UserLightModel();
        user.id = response.id;
        user.username = response.username;
        user.profileImage = response.profileImage?.url;

        this.members.push(user);
      }
    });
  }

  onSearch(value: string) {
    this.users = undefined;

    if (!value) {
      this.searching = false;
      return;
    }

    this.searching = true;
    this.apiService.post<PagedResponse<UserLightModel>>('/User/SearchContracts', { keyword: value, following: true }).pipe(
      take(1)
    ).subscribe({
      next: response => {
        this.users = response;
        this.users.items = response.items!.filter(_ => !this.members.some(m => m.id === _.id));
      }
    });
  }

  add(user: UserLightModel): void {
    console.log(this.members.findIndex(_ => _.id === user.id))
    if (this.members.findIndex(_ => _.id === user.id) >= 0)
      return;

    this.members.push(user);
    this.users!.items = this.users!.items!.filter(_ => _.id !== user.id);
  }
  remove = (userId: string): void => { this.members = this.members.filter(_ => _.id !== userId); }

  create(): void {
    this.loading = true;
    this.errorService.clean();

    this.fileUploadService.uploadMultipart<ConversationCreateModel, string>('/Inbox/CreateGroupConversation', [this.file!], {
      name: this.name,
      users: this.members.map(_ => _.id!)
    }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe({
      next: conversationId => this.router.navigateByUrl(`/inbox/${conversationId}`),
      error: _ => this.errorService.add(_.error.errors)
    });
  }

  onFileChange(e: Event): void {
    const input = e.target as HTMLInputElement;
    if (!input.files?.length)
      return;

    const reader = new FileReader();
    reader.onload = () => this.preview = reader.result as string;

    reader.readAsDataURL(input.files[0]);

    this.file = input.files[0];
  }
}
