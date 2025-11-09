import { Injectable } from "@angular/core";
import { Subject, Subscription, filter, map } from "rxjs";
import { EventData } from "../models/event-data.model";

@Injectable({
  providedIn: 'root'
})
export class EventBusService {
  private _subject$ = new Subject<EventData>();

  emit(event: EventData) {
    this._subject$.next(event);
  }

  on(eventName: string, action: any): Subscription {
    return this._subject$.pipe(
      filter((e: EventData) => e.name === eventName),
      map((e: EventData) => e['value']))
      .subscribe(action);
  }
}
