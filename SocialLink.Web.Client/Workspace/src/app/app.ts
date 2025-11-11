import { Component, OnDestroy, OnInit, effect } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { PageLoaderComponent } from './shared/components/page-loader';
import { AuthService } from './core/services/auth.service';
import { Subscription } from 'rxjs';
import { EventBusService } from './core/services/event-bus.service';
import { Constants } from './shared/constants';
import { EventData } from './core/models/event-data.model';
import { ErrorService } from './core/services/error.service';
import { PresenceService } from './features/messaging/services/presence.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PageLoaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit, OnDestroy {
  eventBusSub = new Subscription();

  constructor(
    private router: Router,
    private authService: AuthService,
    private eventBusService: EventBusService,
    private errorService: ErrorService,
    private presenceService: PresenceService
  ) { }

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

    this.eventBusSub.add(
      this.eventBusService.on(Constants.logoutEvent, () => {
        this.presenceService.stopHubConnection();
        this.authService.logout()
          .subscribe({
            next: () => this.router.navigateByUrl('/auth/login'),
            error: error => console.error(error)
          })
      })
    );

    this.eventBusSub.add(
      this.eventBusService.on(Constants.startHubConnections, () => {
        this.presenceService.createHubConnection();
      })
    );
  }

  ngOnDestroy(): void {
    this.presenceService.stopHubConnection();
    this.eventBusSub?.unsubscribe();
  }
}
