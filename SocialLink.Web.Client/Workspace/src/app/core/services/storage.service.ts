import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
  get = (key: string) => sessionStorage.getItem(key);

  set = (key: string, value: string) => sessionStorage.setItem(key, value);

  remove = (key: string) => sessionStorage.removeItem(key);
}
