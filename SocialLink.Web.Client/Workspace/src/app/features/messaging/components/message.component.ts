import { Component, OnInit, ViewContainerRef, input, viewChild } from "@angular/core";
import { MessageModel } from "../models/message.model";
import { MessageBoxComponentMap } from "./_message-box-component.map";

@Component({
  selector: 'app-message',
  template: '<ng-template #container/>'
})
export class Message implements OnInit {
  message = input<MessageModel>();
  container = viewChild('container', { read: ViewContainerRef });

  ngOnInit(): void {
    if (!this.message()) return;

    const component = MessageBoxComponentMap[this.message()?.type || 0];
    if (component) {
      this.container()?.clear();
      const ref = this.container()?.createComponent(component);
      ref?.setInput('message', this.message());
    }
  }
}
