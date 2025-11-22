import { Directive, OnInit, input } from "@angular/core";
import { MessageModel } from "../models/message.model";
import { AuthService } from "../../../core/services/auth.service";

@Directive()
export abstract class BaseMessageBoxComponent<T> implements OnInit{
  message = input<MessageModel>();
  data?: T;
  currentUserId?: string;

  constructor(private authService: AuthService) {
    this.currentUserId = this.authService.getUserId();
  }

  ngOnInit(): void{
    if (this.message()) {
      try {
        this.data = JSON.parse(this.message()?.content!);
      }
      catch { }
    }
  }

  protected isFromCurrentUser = (userId?: string) => userId === this.currentUserId;
}
