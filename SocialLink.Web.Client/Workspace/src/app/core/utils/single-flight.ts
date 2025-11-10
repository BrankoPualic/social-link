import { Observable, finalize, shareReplay } from "rxjs";

export class SingleFlight<T> {
  private _inFlight$?: Observable<T>;

  run(factory: () => Observable<T>): Observable<T> {
    if (this._inFlight$)
      return this._inFlight$;

    this._inFlight$ = factory().pipe(
      finalize(() => this._inFlight$ = undefined),
      shareReplay(1)
    );

    return this._inFlight$;
  }
}
