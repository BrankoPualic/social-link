import { Injectable } from "@angular/core";
import { eGender } from "../enumerators/gender.enum";

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  getImage(url?: string): string;
  getImage(url?: string, genderId?: eGender): string;
  getImage(url?: string, genderId?: eGender): string {
    if (url)
      return url;

    const gender = genderId === eGender.Female ? 'woman' : 'man';
    return `./assets/images/${gender}.png`;
  }
}
