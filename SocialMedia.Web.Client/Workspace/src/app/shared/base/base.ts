import { Functions } from "../functions";
import { INameofOptions } from "../interfaces";

export abstract class BaseComponent { }

export abstract class BaseComponentGeneric<T extends object> extends BaseComponent {
  nameof = (exp: (obj: T) => any, options?: INameofOptions) => Functions.nameof<T>(exp, options);
}
