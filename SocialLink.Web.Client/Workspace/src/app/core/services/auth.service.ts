import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { filter, map } from 'rxjs/operators';
import { StorageService } from './storage.service';
import { FileUploadService } from './file-upload.service';
import { HttpResponse, HttpEventType } from '@angular/common/http';
import { LoginModel } from '../../features/auth/models/login.model';
import { Token } from '../../features/auth/models/token';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private apiService: ApiService,
    private storageService: StorageService,
    private fileUploadService: FileUploadService,
    private router: Router
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

  signout() {
    this.storageService.remove('token');
    this.router.navigateByUrl('/login');
  }

  getToken = () => this.storageService.get('token');

  setToken(token: Token): void {
    if (!token || !token.content)
      return;

    this.storageService.set('token', JSON.stringify(token.content));
  }

  private decodeToken = (token: Token) => token.content && JSON.parse(atob(token.content.split('.')[1]));
}
