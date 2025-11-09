import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { PageLoaderComponent } from './shared/components/page-loader';
import { AuthService } from './core/services/auth.service';
import { Subscription } from 'rxjs';
import { EventBusService } from './core/services/event-bus.service';
import { Constants } from './shared/constants';
import { EventData } from './core/models/event-data.model';
import { ErrorService } from './core/services/error.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PageLoaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit, OnDestroy {
  eventBusSub?: Subscription;

  constructor(
    private router: Router,
    private authService: AuthService,
    private eventBusService: EventBusService,
    private errorService: ErrorService
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      this.errorService.clean();

      if (event instanceof NavigationEnd) {
        if (
          !this.router.url.includes('/auth') &&
          !this.router.url.includes('/error')
        ) {
          this.authService.getCurrentUser()
            .subscribe({
              error: _ => this.eventBusService.emit(new EventData(Constants.logoutEvent, null))
            });
        }
      }
    });

    this.eventBusSub = this.eventBusService.on(Constants.logoutEvent, () => {
      this.authService.logout()
        .subscribe({
          next: () => this.router.navigateByUrl('/auth/login'),
          error: error => console.error(error)
        })
    })
  }

  ngOnDestroy(): void {
    this.eventBusSub?.unsubscribe();
  }
}
