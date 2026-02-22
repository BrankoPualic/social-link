import { TemplateRef } from "@angular/core";

export class GridOptions {
  columns!: GridColumn[];
  read!: () => Promise<any[]>;
  scrollable?: boolean;
  height?: string;
}

export class GridColumn<T = any> {
  title?: string;
  field?: keyof T;
  template?: TemplateRef<{ $implicit: T; column: GridColumn<T> }>;
  class?: string;
  style?: string;
  width?: number;
  titleStyle?: string;
  titleClass?: string;
}
