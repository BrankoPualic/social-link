import { eMessageType } from "../../../core/enumerators/message-type.enum";
import { UserLightModel } from "../../user/models/user-light.model";

export class MessageModel {
  id?: string;
  chatGroupId?: string;
  userId?: string;
  type?: eMessageType;
  content?: string;
  createdOn?: Date | string;
  lastChangedOn?: Date | string;
  user?: UserLightModel;
}
