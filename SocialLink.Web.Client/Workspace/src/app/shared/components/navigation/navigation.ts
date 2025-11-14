import { Component } from "@angular/core";
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from "../../../core/services/auth.service";
import { EventBusService } from "../../../core/services/event-bus.service";
import { EventData } from "../../../core/models/event-data.model";
import { Constants } from "../../constants";

@Component({
  selector: 'app-navigation',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navigation.html',
  styleUrl: './navigation.scss'
})
export class Navigation {
  userId?: string;

  constructor(
    private authService: AuthService,
    private eventBusService: EventBusService
  ) {
    this.userId = this.authService.getUserId();
  }

  logout = (): void => this.eventBusService.emit(new EventData(Constants.logoutEvent, null));
}
