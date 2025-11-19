import { UserLightModel } from "../../user/models/user-light.model";

export class ConversationModel {
  id?: string;
  lastMessageOn?: Date | string;
  lastMessagePreview?: string;
  user?: UserLightModel;
  name?: string;
  isGroup?: boolean;
  groupImageUrl?: string;
}
