import { UserLightModel } from "../../user/models/user-light.model";

export class CommentModel {
  id?: string;
  userId?: string;
  postId?: string;
  parentId?: string;
  message?: string;
  likesCount?: number;
  repliesCount?: number;
  createdOn?: Date | string;
  user?: UserLightModel;
  isLiked?: boolean;
}
