import { Component, OnChanges, SimpleChanges, input } from "@angular/core";
import { GridOptions } from "../../../core/models/grid.model";
import { PagedResponse } from "../../../core/models/paged-response";

@Component({
  selector: 'app-grid',
  templateUrl: './grid.html',
  styleUrl: './grid.scss'
})
export class Grid implements OnChanges{
  options = input<GridOptions>();
  data!: any;
  isPagedResponse = false;

  ngOnChanges(changes: SimpleChanges): void {
    this.options()!.read()
      .then(result => {
        if (
          result &&
          typeof result === 'object' &&
          'items' in result &&
          'totalCount' in result
        ) {
          this.isPagedResponse = true;
        }

        this.data = result
      });
  }
}
