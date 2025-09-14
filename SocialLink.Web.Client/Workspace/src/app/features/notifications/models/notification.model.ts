import { eNotificationType } from "../../../core/enumerators/notification-type.enum";

export class NotificationModel {
  id?: string;
  userId?: string;
  typeId?: eNotificationType;
  title?: string;
  message?: string;
  details?: string;
  isRead?: boolean;
  createdOn?: Date;
}
