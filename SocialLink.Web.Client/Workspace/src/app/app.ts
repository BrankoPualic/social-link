import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { PageLoaderComponent } from './shared/components/page-loader';
import { AuthService } from './core/services/auth.service';
import { Subscription } from 'rxjs';
import { EventBusService } from './core/services/event-bus.service';
import { Constants } from './shared/constants';
import { EventData } from './core/models/event-data.model';

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
    private eventBusService: EventBusService
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        if (!this.router.url.includes('/auth')) {
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
