import { eGender } from "../../../core/enumerators/gender.enum";
import { BlobModel } from "../../../shared/models/blob.model";

export class UserModel {
  id?: string;
  username?: string;
  firstName?: string;
  lastName?: string;
  fullName?: string;
  genderId?: eGender;
  isPrivate?: boolean;
  biography?: string;
  dateOfBirth?: Date | string;
  profileImage?: BlobModel;
  posts?: number;
  followers?: number;
  following?: number;
}
