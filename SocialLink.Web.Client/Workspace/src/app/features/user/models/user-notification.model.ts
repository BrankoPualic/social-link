import { eNotificationType } from "../../../core/enumerators/notification-type.enum";

export class UserNotificationModel {
  id?: string;
  userId?: string;
  notificationTypeId?: eNotificationType;
  name?: string;
  isMuted?: boolean;
}
