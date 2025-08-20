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

  // JSON

  static toJson(data: any): string {
    try {
      return JSON.stringify(data, Functions.removeCircularReferences());
    } catch (error) {
      console.error('Error serializing object:', error);
      return '{}';
    }
  }

  private static removeCircularReferences() {
    const seen = new WeakSet();
    return (key: any, value: any) => {
      if (typeof value === 'object' && value !== null) {
        if (seen.has(value))
          return '[Circular]';
        seen.add(value);
      }
      return value;
    }
  }
}
