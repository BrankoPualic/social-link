import { Type } from "@angular/core";
import { eNotificationType } from "../../../core/enumerators/notification-type.enum";
import { FollowRequestComponent } from "./follow-request.component";
import { FollowAcceptedComponent } from "./follow-accepted.component";
import { StartFollowingComponent } from "./start-following.component";
import { DefaultComponent } from "./default.component";

export const NotificationComponentMap: Record<eNotificationType, Type<any>> = {
  "0": DefaultComponent,
  "10": FollowRequestComponent,
  "20": FollowAcceptedComponent,
  "30": StartFollowingComponent
};
