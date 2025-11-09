import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Lookup } from "../models/lookup";
import { StorageService } from "./storage.service";
import { Observable, of, tap } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LookupService {
  private _apiUrl = 'https://localhost:7175/api';

  constructor(
    private http: HttpClient,
    private storageService: StorageService
  ) { }

  get(url: string, options?: object): Observable<Lookup[]> {
    const key = 'get' + url.split('/').join('_');
    const value = this.storageService.get(key);

    if (!!value)
      return of(JSON.parse(value));


    return this.http.get<Lookup[]>(this._apiUrl + url, options).pipe(
      tap(response => this.storageService.set(key, JSON.stringify(response)))
    );
  }
}
