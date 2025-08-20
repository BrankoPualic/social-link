import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { LoginModel } from '../models/login.model';
import { Token } from '../models/token';
import { filter, map } from 'rxjs/operators';
import { StorageService } from '../../../core/services/storage.service';
import { FileUploadService } from '../../../core/services/file-upload.service';
import { HttpResponse, HttpEventType } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private apiService: ApiService,
    private storageService: StorageService,
    private fileUploadService: FileUploadService
  ) { }

  login(data: LoginModel) {
    return this.apiService.post<Token>('/users/login', data)
      .pipe(
        map(_ => this.setToken(_))
      );
  }

  signup(data: any, file?: File) {
    return this.fileUploadService.uploadMultipart<any, Token>('/users/signup', file ? [file] : [], data)
      .pipe(
        filter((event): event is HttpResponse<Token> => event.type === HttpEventType.Response),
        map(_ => _.body as Token),
        map(_ => this.setToken(_))
      );
  }

  getToken = () => this.storageService.get('token');

  setToken(token: Token): void {
    if (!token || !token.content)
      return;

    this.storageService.set('token', JSON.stringify(token.content));
  }

  private decodeToken = (token: Token) => token.content && JSON.parse(atob(token.content.split('.')[1]));
}
