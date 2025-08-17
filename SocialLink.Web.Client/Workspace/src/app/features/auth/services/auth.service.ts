import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { LoginModel } from '../models/login-model';
import { Token } from '../models/token';
import { map } from 'rxjs';
import { StorageService } from '../../../core/services/storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private apiService: ApiService,
    private storageService: StorageService
  ) { }

  login(data: LoginModel) {
    return this.apiService.post<Token>('/users/login', data)
      .pipe(
        map(_ => this.setToken(_))
      );
  }

  signup(data: any) {
    return this.apiService.post<Token>('/users/signup', data)
      .pipe(
        map(_ => this.setToken(_))
      );
  }

  getToken = () => this.storageService.get('token');

  setToken(token: Token): void {
    if (!token || !token.content)
      return;

    this.storageService.set('token', JSON.stringify(token));
  }

  private decodeToken = (token: Token) => token.content && JSON.parse(atob(token.content.split('.')[1]));
}
