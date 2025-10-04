import { INameofOptions } from "./interfaces";

export class Functions {
  static nameof<T extends object>(
    exp: ((obj: T) => any) | (new (...params: any[]) => T),
    options?: INameofOptions,
  ): string {
    const fnStr = exp.toString();

    if (fnStr.substring(0, 6) == 'class ' && fnStr.substring(0, 8) != 'class =>') {
      return this.cleanseAssertionOperators(fnStr.substring('class '.length, fnStr.indexOf(' {')));
    }

    if (fnStr.indexOf('=>') !== -1) {
      let name = this.cleanseAssertionOperators(fnStr.substring(fnStr.indexOf('.') + 1));
      if (options?.lastPart) name = name.substring(name.lastIndexOf('.') + 1);
      return name;
    }

    throw new Error('ts-simple-nameof: Invalid function');
  }

  private static cleanseAssertionOperators(parsedName: string): string {
    return parsedName.replace(/[?!]/g, '');
  }

  // Date

  static formatRequestDates(data: any): void {
    // Ignore things that aren't objects.
    if (typeof data !== 'object')
      return data;

    for (let key in data) {
      if (data.hasOwnProperty(key)) {
        const value = data[key];

        if (value instanceof Date) {
          data[key] = value.toISOString();
        } else if (typeof value === 'object') {
          this.formatRequestDates(value);
        }
      }
    }
  }

  static formatDateForInput(date: string | Date | undefined): string | null {
    if (!date) return null;
    const d = new Date(date);

    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  static appendFormData(formData: FormData, data: any, parentKey = '') {
    for (const key in data) {
      if (data.hasOwnProperty(key)) {
        const value = data[key];
        const fullKey = parentKey ? `${parentKey}.${key}` : key;

        if (value instanceof Date) {
          formData.append(fullKey, value.toISOString());
        }
        else if (typeof value === 'object' && value != null) {
          this.appendFormData(formData, value, fullKey);
        }
        else {
          if (value !== null && value !== undefined) {
            formData.append(fullKey, value as any);
          }
        }
      }
    }
  }

  // String

  static formatString = (text?: string): string => !!text ? text.replace(/\n/g, '<br/>') : '';
}
