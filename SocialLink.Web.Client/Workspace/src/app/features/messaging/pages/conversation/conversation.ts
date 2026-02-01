import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, ElementRef, OnDestroy, effect, viewChild } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable, Subject, debounceTime, finalize, map, take } from 'rxjs';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';
import { EventBusService } from '../../../../core/services/event-bus.service';
import { SharedService } from '../../../../core/services/shared.service';
import { AudioPlayer } from '../../../../shared/components/audio-player';
import { MessageInput } from '../../../../shared/components/message-input/message-input';
import { VoiceRecorderButton } from '../../../../shared/components/voice-recorder-button/voice-recorder-button';
import { Constants } from '../../../../shared/constants';
import { Message } from '../../components/message/message.component';
import { ConversationModel } from '../../models/conversation.model';
import { MessageModel } from '../../models/message.model';
import { MessageService } from '../../services/message.service';
import { PresenceService } from '../../services/presence.service';
import { MessageSearch } from '../../../../core/models/search/message-search';

@Component({
    selector: 'app-conversation',
    imports: [MessageInput, DatePipe, AsyncPipe, VoiceRecorderButton, AudioPlayer, Message, RouterLink],
    templateUrl: './conversation.html',
    styleUrl: './conversation.scss'
})
export class Conversation implements OnDestroy {
    messagesContainer = viewChild<ElementRef<HTMLDivElement>>('messagesContainer');
    conversationId?: string;
    conversation?: ConversationModel;
    currentUserId?: string;

    private _typingSubject = new Subject<string>();

    isAudioRecorded = false;
    audioBlob?: Blob;

    isSmallScreen = false;

    messagesLoading = false;

    constructor(
        private apiService: ApiService,
        private authServuce: AuthService,
        public messageService: MessageService,
        public presenceService: PresenceService,
        public sharedService: SharedService,
        private eventBusService: EventBusService,
        private route: ActivatedRoute,
        private router: Router
    ) {
        this.route.paramMap.subscribe(params => {
            this.conversationId = params.get('id')!;
            this.loadConversation();
            this.messageService.stopHubConnection();
            const searchOptions = new MessageSearch();
            searchOptions.chatGroupId = this.conversationId!;
            this.messageService.getMessages(searchOptions).subscribe({
                next: response => this.scrollToBottom()
            });
            this.messageService.createHubConnection(this.conversationId);
        });

        this.currentUserId = this.authServuce.getUserId();

        effect(() => {
            this.messageService.messagesSignal();
            if (this.messagesLoading)
                this.messagesLoading = false;
            else
                this.scrollToBottom();
        });

        effect(() => {
            this.messageService.typingUserSignal();
            this.scrollToBottom();
        });

        this._typingSubject.pipe(
            debounceTime(1500)
        ).subscribe(() => this.messageService.stopTyping(this.conversationId!));

        this.eventBusService.on(Constants.audioMessageLoaded, () => this.scrollToBottom());

        if (window.screen.width < 768)
            this.isSmallScreen = true;
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

    isMultiline = (content?: string) => !!content && content.includes('\n');

    displayDate(message: MessageModel, index: number): boolean {
        if (index === 0)
            return true;

        const currentMessageDate = new Date(message.createdOn!).toDateString();
        const previousMessageDate = new Date(this.messageService.messagesSignal()?.items![index - 1]?.createdOn!).toDateString();

        return currentMessageDate !== previousMessageDate;
    }

    startTyping(e: Event): void {
        if (!e) return;

        this._typingSubject.next(this.conversationId!);
        this.messageService.startTyping(this.conversationId!);
    }

    goToProfile = (userId?: string) => !!userId && this.router.navigateByUrl(`/profile/${userId}`);

    isSomeoneTyping(): Observable<string | null> {
        return this.authServuce.getCurrentUser().pipe(
            map(currentUser => this.messageService.typingUserSignal())
        );
    }

    onAudioRecorded(blob: Blob) {
        this.audioBlob = blob;
        this.isAudioRecorded = true;
        this.scrollToBottom();
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

  onScroll(): void {
        const container = this.messagesContainer()?.nativeElement;

        if (container && container.scrollTop === 0 && !this.messagesLoading) {
            this.messagesLoading = true;
            const searchOptions = new MessageSearch();
            searchOptions.chatGroupId = this.conversationId!;
            searchOptions.page = this.messageService.messagesSignal()?.currentPage
                ? this.messageService.messagesSignal()!.currentPage! + 1
                : searchOptions.page;

            if (searchOptions.page <= this.messageService.messagesSignal()?.pageCount!)
                this.messageService.getMessages(searchOptions).subscribe();
        }
    }

    private scrollToBottom(): void {
        setTimeout(() => {
            if (this.messagesContainer()) {
                const element = this.messagesContainer()!.nativeElement;
                element.scrollTop = element.scrollHeight;
            }
        }, 0);
    }
}
