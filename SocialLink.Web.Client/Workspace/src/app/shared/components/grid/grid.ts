import { CommonModule } from "@angular/common";
import { Component, OnInit, input } from "@angular/core";
import { GridOptions } from "../../../core/models/grid.model";

@Component({
  selector: 'app-grid',
  templateUrl: './grid.html',
  styleUrl: './grid.scss',
  imports: [CommonModule]
})
export class Grid implements OnInit {
  options = input<GridOptions>();
  data!: any;
  isPagedResponse = false;

  ngOnInit(): void {
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

        this.data = result;
      });
  }
}
