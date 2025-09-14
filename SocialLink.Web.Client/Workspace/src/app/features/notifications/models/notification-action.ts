import { eNotificationActionMethodType } from "../../../core/enumerators/notification-action-method-type.enum";

export class NotificationAction {
  Label?: string;
  Endpoint?: string;
  Method?: eNotificationActionMethodType;
  RequiresConfirmation?: boolean;
}
