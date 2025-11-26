import { AsyncPipe, DatePipe } from '@angular/common';
import { AfterViewChecked, Component, ElementRef, OnDestroy, ViewContainerRef, effect, viewChild } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable, Subject, debounceTime, map, take } from 'rxjs';
import { FormatTextPipe } from '../../../../core/pipes/format-text.pipe';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';
import { SharedService } from '../../../../core/services/shared.service';
import { MessageInput } from '../../../../shared/components/message-input/message-input';
import { ConversationModel } from '../../models/conversation.model';
import { MessageModel } from '../../models/message.model';
import { MessageService } from '../../services/message.service';
import { PresenceService } from '../../services/presence.service';
import { VoiceRecorderButton } from '../../../../shared/components/voice-recorder-button/voice-recorder-button';
import { AudioPlayer } from '../../../../shared/components/audio-player';
import { Message } from '../../components/message.component';

@Component({
  selector: 'app-conversation',
  imports: [RouterLink, MessageInput, DatePipe, FormatTextPipe, AsyncPipe, VoiceRecorderButton, AudioPlayer, Message],
  templateUrl: './conversation.html',
  styleUrl: './conversation.scss'
})
export class Conversation implements OnDestroy, AfterViewChecked {
  messagesContainer = viewChild<ElementRef>('messagesContainer');
  conversationId?: string;
  conversation?: ConversationModel;
  currentUserId?: string;

  private _typingSubject = new Subject<string>();

  isAudioRecorded = false;
  audioBlob?: Blob;

  constructor(
    private apiService: ApiService,
    private authServuce: AuthService,
    public messageService: MessageService,
    public presenceService: PresenceService,
    public sharedService: SharedService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.route.paramMap.subscribe(params => {
      this.conversationId = params.get('id')!;
      this.loadConversation();
      this.messageService.getMessages(this.conversationId).subscribe({
        next: response => {
          setTimeout(() => {
            this.scrollToBottom();
          }, 0);
        }
      });
      this.messageService.createHubConnection(this.conversationId);
    });

    this.currentUserId = this.authServuce.getUserId();

    this._typingSubject.pipe(
      debounceTime(1500)
    ).subscribe(() => this.messageService.stopTyping(this.conversationId!));
  }

  ngAfterViewChecked(): void {
    setTimeout(() => {
      this.scrollToBottom();
    }, 0);
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  loadConversation(): void {
    this.apiService.get<ConversationModel>(`/Inbox/GetConversation/${this.conversationId}`).pipe(
      take(1)
    ).subscribe({
      next: response => this.conversation = response
    })
  }

  createMessage(message?: string): void {
    if (!message) return;

    const data: MessageModel = {
      chatGroupId: this.conversationId,
      userId: this.authServuce.getUserId(),
      content: message
    }

    this.messageService.createMessage(data);
  }

  fromCurrentUser = (userId?: string) => userId === this.currentUserId;

  isMultiline = (content?: string) => !!content && content.includes('\n');

  displayDate(message: MessageModel, index: number): boolean {
    if (index === 0)
      return true;

    const currentMessageDate = new Date(message.createdOn!).toDateString();
    const previousMessageDate = new Date(this.messageService.messagesSignal()?.items![index - 1]?.createdOn!).toDateString();

    return currentMessageDate !== previousMessageDate;
  }

  displayTime(message: MessageModel, index: number): boolean {
    const nextMessage = this.messageService.messagesSignal()?.items?.[index + 1];

    const currentMessageMinutes = new Date(message.createdOn!).getMinutes();
    const nextMessageMinutes = new Date(nextMessage?.createdOn!).getMinutes();

    return (currentMessageMinutes !== nextMessageMinutes) ||
      (message.userId !== nextMessage?.userId);
  }

  displayImage(message: MessageModel, index: number): boolean {
    return index === 0 || (this.messageService.messagesSignal()?.items?.[index + 1]?.userId !== message.userId);
  }

  isEdited(message: MessageModel): boolean {
    const createdOn = new Date(message.createdOn!);
    const lastChangedOn = new Date(message.lastChangedOn!);

    return (createdOn.getFullYear() != lastChangedOn.getFullYear()) ||
      (createdOn.getMonth() != lastChangedOn.getMonth()) ||
      (createdOn.getHours() != lastChangedOn.getHours()) ||
      (createdOn.getMinutes() != lastChangedOn.getMinutes()) ||
      (createdOn.getSeconds() != lastChangedOn.getSeconds());
  }

  startTyping(e: Event): void {
    if (!e) return;

    this._typingSubject.next(this.conversationId!);
    this.messageService.startTyping(this.conversationId!);
  }

  goToProfile = (userId?: string) => !!userId && this.router.navigateByUrl(`/profile/${userId}`);

  isSomeoneTyping(): Observable<string | null> {
    return this.authServuce.getCurrentUser().pipe(
      map(currentUser => {
        if (this.messageService.typingUserSignal() != currentUser?.username)
          return this.messageService.typingUserSignal();

        return null;
      })
    );
  }

  onAudioRecorded(blob: Blob) {
    this.audioBlob = blob;
    this.isAudioRecorded = true;
    setTimeout(() => {
      this.scrollToBottom();
    }, 0);
  }

  createAudioMessage() {
    if (!this.audioBlob) return;

    const file = new File([this.audioBlob], `${new Date().getTime().toString()}.webm`, { type: 'audio/webm' });

    this.isAudioRecorded = false;
    this.audioBlob = undefined;
    this.messageService.createAudioMessage(file, this.conversationId!);
  }

  cancelAudioMessage() {
    this.isAudioRecorded = false;
    this.audioBlob = undefined;
  }

  private scrollToBottom(): void {
    if (this.messagesContainer()) {
      const element = this.messagesContainer()!.nativeElement;
      element.scrollTop = element.scrollHeight;
    }
  }
}
