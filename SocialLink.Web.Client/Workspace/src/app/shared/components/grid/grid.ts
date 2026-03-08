import { CommonModule } from "@angular/common";
import { Component, OnInit, input } from "@angular/core";
import { GridOptions } from "../../../core/models/grid.model";
import { EventBusService } from "../../../core/services/event-bus.service";
import { Subscription } from "rxjs/internal/Subscription";
import { Constants } from "../../constants";
import { Pagination } from "../pagination/pagination";

@Component({
  selector: 'app-grid',
  templateUrl: './grid.html',
  styleUrl: './grid.scss',
  imports: [CommonModule, Pagination]
})
export class Grid implements OnInit {
  options = input<GridOptions>();
  data!: any;
  isPagedResponse = false;

  eventBusSub = new Subscription();

  constructor(
    private eventBusService: EventBusService
  ) { }

  ngOnInit(): void {
    this.load();

    this.eventBusSub.add(
      this.eventBusService.on(Constants.gridReadEvent, () => this.load())
    );
  }

  private load() {
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
