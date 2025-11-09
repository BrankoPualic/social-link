import { eGender } from "../../../core/enumerators/gender.enum";
import { Lookup } from "../../../core/models/lookup";

export class CurrentUserModel {
  id?: string;
  username?: string;
  fullName?: string;
  genderId?: eGender;
  isPrivate?: boolean;
  roles: Lookup[] = [];
}
