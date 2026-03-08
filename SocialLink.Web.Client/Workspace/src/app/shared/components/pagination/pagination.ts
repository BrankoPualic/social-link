import { Component, OnChanges, SimpleChanges, input } from "@angular/core";
import { PagedResponse } from "../../../core/models/paged-response";
import { EventBusService } from "../../../core/services/event-bus.service";
import { EventData } from "../../../core/models/event-data.model";
import { Constants } from "../../constants";

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.html',
  styleUrl: './pagination.scss'
})
export class Pagination implements OnChanges {
  options = input<PagedResponse<any[]>>();
  pages: any[] = [];

  constructor(
    private eventBusService: EventBusService
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.options()?.pageCount)
      this.pages = Array.from({ length: this.options()?.pageCount! }, (_, i) => i + 1);
  }

  setPage(page?: number) {
    if (page !== undefined && (this.options()?.pageCount! < page || page < 1)) return;
    this.eventBusService.emit(new EventData(Constants.paginationChangePageEvent, page));
  }
}
