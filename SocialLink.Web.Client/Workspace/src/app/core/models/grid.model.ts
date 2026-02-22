export class GridOptions {
  columns!: GridColumn[];
  read!: () => Promise<any[]>;
  scrollable?: boolean;
  height?: string;
}

export class GridColumn {
  title?: string;
  field?: string;
  template?: string;
  class?: string;
  style?: string;
  width?: number;
  titleStyle?: string;
  titleClass?: string;
}
