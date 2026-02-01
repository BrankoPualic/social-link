import { Injectable, signal } from "@angular/core";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { PagedResponse } from "../../../core/models/paged-response";
import { MessageModel } from "../models/message.model";
import { ApiService } from "../../../core/services/api.service";
import { Observable, finalize, take, tap } from "rxjs";
import { PageLoaderService } from "../../../core/services/page-loader.service";
import { AuthService } from "../../../core/services/auth.service";
import { FileUploadService } from "../../../core/services/file-upload.service";
import { MessageSearch } from "../../../core/models/search/message-search";

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

    const searchOptions = new MessageSearch();
    searchOptions.chatGroupId = chatGroupId;
    this._hubConnection.onreconnected(() => this.getMessages(searchOptions));

    this._hubConnection.on('NewMessage', (message: MessageModel) => {
      if (this._messages()) {
        this._messages.update((current) => {
          return {
            ...current,
            items: [...current?.items!, message]
          }
        });
      }
      else {
        this._messages.update((current) => {
          return {
            ...new PagedResponse(),
            items: [message]
          }
        });
      }

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
    this._messages.set(null);
    this._hubConnection?.stop().catch(_ => console.error(_));
  }

  startTyping(chatGroupId: string) {
    this._hubConnection?.invoke('StartedTyping', chatGroupId).catch(_ => console.error(_));
  }

  stopTyping(chatGroupId: string) {
    this._hubConnection?.invoke('StoppedTyping', chatGroupId).catch(_ => console.error(_));
  }

  getMessages(searchOptions: MessageSearch): Observable<PagedResponse<MessageModel>> {
    this.loaderService.show();
    return this.apiService.post<PagedResponse<MessageModel>>('/Message/Get', searchOptions).pipe(
      take(1),
      finalize(() => this.loaderService.hide()),
      tap((response) => {
        this._messages.update((current) => {
          const cur = current?.items ?? [];
          const incoming = response.items ?? [];

          // Merge (newest first if you prepend) and dedupe by id
          const map = new Map<string, MessageModel>();

          // order matters: put incoming first if you want incoming to win
          for (const m of incoming) map.set(m.id!, m);
          for (const m of cur) map.set(m.id!, m);

          const items = Array.from(map.values());

          return {
            ...(current ?? response),
            ...response,                 // keep other paging fields fresh
            currentPage: searchOptions.page,
            items
          };
        });
      })
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
