import { Injectable, signal } from '@angular/core';
import { ApiService } from './api.service';
import { catchError, finalize, map, take, tap } from 'rxjs/operators';
import { FileUploadService } from './file-upload.service';
import { LoginModel } from '../../features/auth/models/login.model';
import { eSystemRole } from '../enumerators/system-role.enum';
import { Observable, of } from 'rxjs';
import { CurrentUserModel } from '../../features/auth/models/currentUser.model';
import { Lookup } from '../models/lookup';
import { SingleFlight } from '../utils/single-flight';
import { EventBusService } from './event-bus.service';
import { EventData } from '../models/event-data.model';
import { Constants } from '../../shared/constants';
import { PageLoaderService } from './page-loader.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _currentUser = signal<CurrentUserModel | null>(null);
  private _currentUserRequest$ = new SingleFlight<CurrentUserModel | null>();

  constructor(
    private apiService: ApiService,
    private fileUploadService: FileUploadService,
    private eventBusService: EventBusService,
    private loaderService: PageLoaderService
  ) { }

  login(data: LoginModel) {
    return this.apiService.post<void>('/Auth/Login', data).pipe(take(1));
  }

  signup(data: any, file?: File) {
    return this.fileUploadService.uploadMultipart<any, void>('/Auth/Signup', file ? [file] : [], data).pipe(take(1));
  }

  logout() {
    return this.apiService.post<void>('/Auth/Logout', {}).pipe(
      take(1),
      tap(() => this._currentUser.set(null))
    );
  }

  refreshToken() {
    return this.apiService.post<void>('/Auth/RefreshToken', {}).pipe(take(1));
  }

  getCurrentUser() {
    if (this._currentUser())
      return of(this._currentUser());

    this.loaderService.show();
    return this._currentUserRequest$.run(() =>
      this.apiService.get<CurrentUserModel>('/Auth/GetCurrentUser').pipe(
        tap(user => {
          this._currentUser.set(user);
          this.eventBusService.emit(new EventData(Constants.startHubConnections, null));
        }),
        finalize(() => this.loaderService.hide()),
        catchError(() => of(null))
      )
    );
  }

  getUserId = () => this._currentUser()?.id;

  isLoggedIn(): Observable<boolean> {
    if (this._currentUser()) {
      return of(true);
    }

    return this.getCurrentUser().pipe(
      map(user => !!user),
      catchError(() => of(false))
    );
  }

  hasAccess(role: eSystemRole): boolean {
    if (!this._currentUser())
      return false;

    const roles: Lookup[] = this._currentUser()!.roles;

    return !!roles.length && roles.some(_ => _.id == role);
  }
}
