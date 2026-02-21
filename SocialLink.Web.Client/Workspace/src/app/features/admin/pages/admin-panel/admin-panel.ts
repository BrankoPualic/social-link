import { Component, OnInit } from "@angular/core";
import { ApiService } from "../../../../core/services/api.service";
import { BaseComponent } from "../../../../shared/base/base";
import { PageLoaderService } from "../../../../core/services/page-loader.service";
import { finalize, forkJoin, take } from "rxjs";
import { Navigation } from "../../../../shared/components/navigation/navigation";

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.html',
  imports: [Navigation]
})
export class AdminPanel extends BaseComponent implements OnInit {
  userCount = 0;
  postCount = 0;
  messageCount = 0;

  constructor(
    loaderService: PageLoaderService,
    private apiService: ApiService
  ) {
    super(loaderService);
  }

  ngOnInit(): void {
    this.loading = true;

    forkJoin({
      userCount: this.apiService.post<number>('/User/GetCount', {}),
      postCount: this.apiService.get<number>('/Post/GetCount'),
      messageCount: this.apiService.get<number>('/Message/GetCount')
    }).pipe(
      take(1),
      finalize(() => this.loading = false)
    ).subscribe(result => {
      this.userCount = result.userCount;
      this.postCount = result.postCount;
      this.messageCount = result.messageCount;
    });
  }
}
