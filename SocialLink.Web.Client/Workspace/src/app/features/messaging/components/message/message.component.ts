import { Component, OnInit, ViewContainerRef, input, viewChild } from "@angular/core";
import { MessageModel } from "../../models/message.model";
import { MessageBoxComponentMap } from "../_message-box-component.map";
import { AuthService } from "../../../../core/services/auth.service";
import { MessageService } from "../../services/message.service";
import { SharedService } from "../../../../core/services/shared.service";
import { RouterLink } from "@angular/router";
import { DatePipe } from "@angular/common";

@Component({
  selector: 'app-message',
  imports: [RouterLink, DatePipe],
  templateUrl: './message.component.html',
  styleUrl: './message.component.scss'
})
export class Message implements OnInit {
  message = input<MessageModel>();
  index = input<number>();
  container = viewChild('container', { read: ViewContainerRef });
  currentUserId?: string;

  constructor(
    private authSerice: AuthService,
    private messageService: MessageService,
    public sharedService: SharedService
  ) {
    this.currentUserId = this.authSerice.getUserId();
  }

  ngOnInit(): void {
    if (!this.message()) return;

    const component = MessageBoxComponentMap[this.message()?.type || 0];
    if (component) {
      this.container()?.clear();
      const ref = this.container()?.createComponent(component);
      ref?.setInput('message', this.message());
    }
  }

  fromCurrentUser = (userId?: string) => userId === this.currentUserId;

  displayImage(message: MessageModel, index: number): boolean {
    return index === 0 || (this.messageService.messagesSignal()?.items?.[index + 1]?.userId !== message.userId);
  }

  displayTime(message: MessageModel, index: number): boolean {
    const nextMessage = this.messageService.messagesSignal()?.items?.[index + 1];

    const currentMessageMinutes = new Date(message.createdOn!).getMinutes();
    const nextMessageMinutes = new Date(nextMessage?.createdOn!).getMinutes();

    return (currentMessageMinutes !== nextMessageMinutes) ||
      (message.userId !== nextMessage?.userId);
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
}
