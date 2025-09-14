import { eNotificationType } from "../../../../core/enumerators/notification-type.enum";
import { NotificationAction } from "../notification-action";

export class NotificationDetails {
  UserId?: string;
  NotificationTypeId?: eNotificationType;
  Title?: string;
  Message?: string;
  Actions?: NotificationAction[];
}
