import { HttpClient, HttpEvent, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Functions } from "../../shared/functions";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  // TODO: Move to some kind of settings service. Maybe even fetch at runtime to storage
  private _apiUrl = 'https://localhost:7175';
  constructor(private http: HttpClient) { }

  upload<T>(url: string, file: File, params?: any) {
    const formData = new FormData();
    formData.append('file', file);

    if (params) {
      Functions.formatRequestDates(params);
      Object.keys(params).forEach(key => formData.append(key, params[key]));
    }

    const req = new HttpRequest('POST', this._apiUrl + url, formData, {
      reportProgress: true,
      responseType: 'json'
    });

    return this.http.request<T>(req);
  }

  uploadMultipart<T, TResponse>(url: string, files: File[], model?: T): Observable<HttpEvent<TResponse>> {
    const formData = new FormData();

    files.forEach((file, index) => formData.append(`files[${index}]`, file));

    if (model) {
      Functions.appendFormData(formData, model);
    }

    const req = new HttpRequest('POST', this._apiUrl + url, formData, {
      reportProgress: true,
      responseType: 'json'
    });

    return this.http.request<TResponse>(req);
  }
}
