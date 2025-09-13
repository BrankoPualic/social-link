import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { filter, map } from 'rxjs/operators';
import { StorageService } from './storage.service';
import { FileUploadService } from './file-upload.service';
import { HttpResponse, HttpEventType } from '@angular/common/http';
import { LoginModel } from '../../features/auth/models/login.model';
import { Token } from '../../features/auth/models/token';
import { Router } from '@angular/router';
import { eSystemRole } from '../enumerators/system-role.enum';

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

  logout() {
    this.storageService.remove('token');
    this.router.navigateByUrl('/login');
  }

  getToken = () => this.storageService.get('token');

  getUserId() {
    const token = this.getToken();
    if (!token)
      return null;

    return this.decodeToken(token).UserId;
  }

  setToken(token: Token): void {
    if (!token || !token.content)
      return;

    this.storageService.set('token', token.content);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    return !!token && this.isTokenValid(token);
  };

  isTokenValid(token: string): boolean {
    if (!token)
      return false;

    try {
      const decodedToken = this.decodeToken(token);
      const expirationTime = decodedToken.exp * 1000;
      return expirationTime > Date.now();
    }
    catch (err) {
      console.error('Error decoding token:', err);
      return false;
    }
  }

  hasAccess(role: eSystemRole): boolean {
    const token = this.getToken();
    if (!token)
      return false;

    const roles: string[] = this.decodeToken(token).Roles.split(',');

    return !!roles.length && roles.includes(role.toString());
  }

  private decodeToken = (token: string) => token && JSON.parse(atob(token.split('.')[1]));
}
