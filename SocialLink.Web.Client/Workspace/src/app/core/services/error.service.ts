import { Injectable, signal } from "@angular/core";
import { Error } from "../models/error";

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  errors = signal<Error[]>([]);

  add = (errors: { key: string, value: string }[]) => this.errors.set(this.convertToError(errors));

  clean = () => this.errors.set([]);

  convertToError = (err: { key: string; value: string }[]): Error[] => {
    if (!err) return [];

    // group errors by key
    const grouped = err.reduce((acc: Record<string, string[]>, curr) => {
      if (!acc[curr.key]) acc[curr.key] = [];
      acc[curr.key].push(curr.value);
      return acc;
    }, {});

    return Object.entries(grouped).map(([key, errors]) => ({ key, errors }));
  }

  // TODO: Create methods for teast message error or put it into base component
}
