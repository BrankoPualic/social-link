import { Component } from '@angular/core';
import { Navigation } from '../../../../shared/components/navigation';
import { BaseFormComponent } from '../../../../shared/base/base-form';
import { PostCreateModel } from '../../models/post-create.model';
import { IFileUploadForm } from '../../../../shared/interfaces/file-upload-form.interface';
import { PageLoaderService } from '../../../../core/services/page-loader.service';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ErrorService } from '../../../../core/services/error.service';
import { FileUploadService } from '../../../../core/services/file-upload.service';
import { AuthService } from '../../../../core/services/auth.service';
import { finalize, take } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DialogService } from '../../../../core/services/dialog.service';
import { TextArea } from '../../../../shared/components/forms/text-area';
import { ValidationDirective } from '../../../../shared/directives/validation.directive';

@Component({
  selector: 'app-create-post',
  imports: [Navigation, ReactiveFormsModule, TextArea, ValidationDirective],
  templateUrl: './create-post.html',
  styleUrls: ['./create-post.scss']
})
export class CreatePost extends BaseFormComponent<PostCreateModel> implements IFileUploadForm {
  files: File[] = [];
  previews: string[] = [];

  constructor(
    loaderService: PageLoaderService,
    fb: FormBuilder,
    private authService: AuthService,
    private errorService: ErrorService,
    private fileUploadService: FileUploadService,
    private dialogService: DialogService,
    private toastr: ToastrService,
    private router: Router
  ) {
    super(loaderService, fb);
  }

  override initializeForm(): void {
    this.form = this.fb.group({
      [this.nameof(_ => _.description)]: [],
      [this.nameof(_ => _.allowComments)]: [true]
    });
  }

  override submit(): void {
    this.dialogService.confirm('Are you sure you want to create this post?').result
      .then(() => {
        this.loading = true;
        this.errorService.clean();

        const userId = this.authService.getUserId();
        const data: PostCreateModel = {
          ...this.form.value,
          userId: userId
        };

        this.fileUploadService.uploadMultipart('/posts/create', this.files, data)
          .pipe(
            take(1),
            finalize(() => this.loading = false)
          )
          .subscribe({
            next: (postId) => {
              this.toastr.success('You\'ve successfully created post.');
              this.router.navigateByUrl(`/profile/${userId}`);
            },
            error: _ => this.errorService.add(_.error.errors)
          });
      });
  }

  onFileChange(e: Event): void {
    const input = e.target as HTMLInputElement;
    if (!input.files?.length)
      return;

    for (const file of Array.from(input.files)) {
      const reader = new FileReader();
      reader.onload = () => this.previews.push(reader.result as string);

      reader.readAsDataURL(file);

      this.files.push(file);
    }
  }

  removeFile(index: number): void {
    this.files.splice(index, 1);
    this.previews.splice(index, 1);
  }
}
