import { BlobModel } from "../../../core/models/blob.model";
import { UserLightModel } from "../../user/models/user-light.model";

export class PostModel {
  id?: string;
  userId?: string;
  description?: string;
  allowComments?: boolean;
  likesCount?: number;
  commentsCount?: number;
  createdOn?: Date | string;
  user?: UserLightModel;
  media?: BlobModel[];
}
