import { eGender } from "../../../core/enumerators/gender.enum";
import { BlobModel } from "../../../core/models/blob.model";

export class UserModel {
  id?: string;
  username?: string;
  firstName?: string;
  lastName?: string;
  fullName?: string;
  email?: string;
  genderId?: eGender;
  isPrivate?: boolean;
  biography?: string;
  dateOfBirth?: Date | string;
  createdOn?: Date | string;
  profileImage?: BlobModel;
  posts?: number;
  followers?: number;
  following?: number;
}
