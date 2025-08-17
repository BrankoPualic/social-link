import { PageLoaderService } from "../../core/services/page-loader.service";
import { Functions } from "../functions";
import { INameofOptions } from "../interfaces";

export abstract class BaseComponent {
  constructor(
    protected loaderService: PageLoaderService
  ) { }

  // Loader
  get loading() {
    return this.loaderService.state();
  }

  set loading(state: boolean) {
    state
      ? this.loaderService.show()
      : this.loaderService.hide();
  }
}

export abstract class BaseComponentGeneric<T extends object> extends BaseComponent {
  nameof = (exp: (obj: T) => any, options?: INameofOptions) => Functions.nameof<T>(exp, options);
}
