import { Type } from "@angular/core";
import { eMessageType } from "../../../core/enumerators/message-type.enum";
import { AudioMessage } from "./audio-message.component";
import { DefaultComponent } from "./default.component";

export const MessageBoxComponentMap: Record<eMessageType, Type<any>> = {
  "0": DefaultComponent,
  "10": AudioMessage
};
