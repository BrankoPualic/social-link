import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private _apiUrl = 'https://localhost:7175';

  constructor(private http: HttpClient) { }

  get = <T>(url: string, options?: object) => this.http.get<T>(this._apiUrl + url, options);

  post = <T>(url: string, data: any, options?: object) => this.http.post<T>(this._apiUrl + url, data, options);
}
