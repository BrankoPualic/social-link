import { eGender } from "../../../core/enumerators/gender.enum";

export class SignupModel {
  firstName?: string;
  lastName?: string;
  username?: string;
  email?: string;
  password?: string;
  repeatPassword?: string;
  genderId?: eGender;
  dateOfBirth?: Date | string;
  isPrivate?: boolean;
}
