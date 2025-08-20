import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Lookup } from "../models/lookup";

@Injectable({
  providedIn: 'root'
})
export class LookupService {
  private _apiUrl = 'https://localhost:7175';

  constructor(private http: HttpClient) { }

  get = (url: string, options?: object) => this.http.get<Lookup[]>(this._apiUrl + url, options);
}
