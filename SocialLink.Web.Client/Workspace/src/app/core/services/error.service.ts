import { Injectable, signal } from "@angular/core";
import { Error } from "../models/error";

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  errors = signal<Error[]>([]);

  add = (errors: Record<string, string[]>) => this.errors.set(this.convertToError(errors));

  clean = () => this.errors.set([]);

  convertToError = (err: Record<string, string[]>): Error[] => Object.entries(err).map(([key, errors]) => ({ key, errors }));
}
