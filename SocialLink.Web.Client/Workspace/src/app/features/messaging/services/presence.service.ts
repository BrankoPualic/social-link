import { Injectable, signal } from "@angular/core";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { ToastrService } from "ngx-toastr";
import { UAParser } from 'ua-parser-js';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  // TODO: Maybe move to some kind of Setting Service?
  private _hubUrl = 'https://localhost:7175/hubs';
  private _hubConnection?: HubConnection;

  private _onlineUsers = signal<string[]>([]);
  onlineUsersSignal = this._onlineUsers.asReadonly();

  constructor(private toastrService: ToastrService) { }

  createHubConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this._hubUrl + '/presence', {
        withCredentials: true,
        logger: LogLevel.Information
      })
      .withAutomaticReconnect()
      .build();

    this._hubConnection.start()
      .then(() => {
        const parser = new UAParser();
        this._hubConnection?.invoke('RegisterClientInfo', JSON.stringify(parser.getResult()))
          .catch(_ => console.error(_));
      })
      .catch(_ => console.error(_));

    this._hubConnection.onreconnecting(() => {
        this.toastrService.info('You are offline.');
        this._onlineUsers.set([]);
    });

    this._hubConnection.onreconnected(() => {
      this.toastrService.info('You are online.');
    });

    this._hubConnection.on('UserIsOnline', (userId) => {
      this._onlineUsers.update((current) => [...current, userId]);
    });

    this._hubConnection.on('FollowingOnlineList', (following) => {
      this._onlineUsers.set(following);
    });

    this._hubConnection.on('UserIsOffline', (userId) => {
      this._onlineUsers.update((current) => current.filter(_ => _ !== userId));
    })
  }

  stopHubConnection() {
    this._hubConnection?.stop().catch(_ => console.error(_));
  }
}
