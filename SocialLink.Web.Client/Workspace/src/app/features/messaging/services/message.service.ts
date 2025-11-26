import { Injectable, signal } from "@angular/core";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { PagedResponse } from "../../../core/models/paged-response";
import { MessageModel } from "../models/message.model";
import { ApiService } from "../../../core/services/api.service";
import { Observable, finalize, take, tap } from "rxjs";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { AuthService } from "../../../core/services/auth.service";
import { FileUploadService } from "../../../core/services/file-upload.service";

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  // TODO: Maybe move to some kind of Setting Service?
  private _hubUrl = 'https://localhost:7175/hubs';
  private _hubConnection?: HubConnection;

  private _messages = signal<PagedResponse<MessageModel> | null>(null);
  messagesSignal = this._messages.asReadonly();

  private _typingUser = signal<string | null>(null);
  typingUserSignal = this._typingUser.asReadonly();

  constructor(
    private apiService: ApiService,
    private authService: AuthService,
    private loaderService: PageLoaderService,
    private fileUploadService: FileUploadService
  ) { }

  createHubConnection(chatGroupId: string) {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this._hubUrl + `/message?chatGroupId=${chatGroupId}`, {
        withCredentials: true
      })
      .withAutomaticReconnect()
      .build();

    this._hubConnection.start().catch(_ => console.error(_));

    this._hubConnection.onreconnected(() => this.getMessages(chatGroupId));

    this._hubConnection.on('NewMessage', (message: MessageModel) => {
      this._messages.update((current) => {
        return {
          ...current,
          items: [...current?.items!, message]
        }
      });

      this.apiService.post('/Message/ReadMessage', {
        lastMessageId: message.id,
        chatGroupId: message.chatGroupId,
        userId: this.authService.getUserId()
      }).pipe(take(1)).subscribe();
    });

    this._hubConnection.on('UserIsTyping', (username) => this._typingUser.set(username));
    this._hubConnection.on('UserStoppedTyping', () => this._typingUser.set(null));
  }

  stopHubConnection() {
    this._hubConnection?.stop().catch(_ => console.error(_));
  }

  startTyping(chatGroupId: string) {
    this._hubConnection?.invoke('StartedTyping', chatGroupId).catch(_ => console.error(_));
  }

  stopTyping(chatGroupId: string) {
    this._hubConnection?.invoke('StoppedTyping', chatGroupId).catch(_ => console.error(_));
  }

  getMessages(chatGroupId: string): Observable<PagedResponse<MessageModel>> {
    this.loaderService.show();
    return this.apiService.post<PagedResponse<MessageModel>>('/Message/Get', { pageSize: 30, chatGroupId: chatGroupId }).pipe(
      take(1),
      finalize(() => this.loaderService.hide()),
      tap((response) => this._messages.set(response))
    );
  };

  createMessage(data: MessageModel) {
    this.apiService.post<string>('/Message/Create', data).pipe(
      take(1)
    ).subscribe({
      next: messageId => { },
      error: _ => console.error(_)
    });
  }

  createAudioMessage(audio: File, chatGroupId: string) {
    this.fileUploadService.uploadMultipart('/Message/CreateAudioMessage', [audio], { chatGroupId: chatGroupId }).pipe(
      take(1)
    ).subscribe({
      next: messageId => { },
      error: _ => console.error(_)
    });
  }
}
