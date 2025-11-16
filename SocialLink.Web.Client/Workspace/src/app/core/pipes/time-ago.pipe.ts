import { ChangeDetectorRef, OnDestroy, Pipe, PipeTransform } from '@angular/core';
import { Subscription, interval } from 'rxjs';

@Pipe({
  name: 'timeAgo',
  standalone: true,
  pure: false
})
export class TimeAgoPipe implements PipeTransform, OnDestroy {
  private _timer?: Subscription;

  constructor(private cd: ChangeDetectorRef) { }

  transform(value?: string | Date): string {
    if (!value) return '';

    const targetTime = new Date(value);
    const now = new Date();
    const diffInSeconds = Math.floor((now.getTime() - targetTime.getTime()) / 1000);
    const diffInMinutes = Math.floor(diffInSeconds / 60);
    const diffInHours = Math.floor(diffInMinutes / 60);
    const diffInDays = Math.floor(diffInHours / 24);
    const diffInMonths = Math.floor(diffInDays / 30);
    const diffInYears = Math.floor(diffInMonths / 12);

    // Setup timer to refresh pipe automatically
    this.setupTimer(diffInSeconds, diffInMinutes, diffInHours);

    if (diffInSeconds < 60) return `${diffInSeconds} second${diffInSeconds === 1 ? '' : 's'} ago`;
    if (diffInMinutes < 60) return `${diffInMinutes} minute${diffInMinutes === 1 ? '' : 's'} ago`;
    if (diffInHours < 24) return `${diffInHours} hour${diffInHours === 1 ? '' : 's'} ago`;
    if (diffInDays < 30) return `${diffInDays} day${diffInDays === 1 ? '' : 's'} ago`;
    if (diffInMonths < 12) return `${diffInMonths} month${diffInMonths === 1 ? '' : 's'} ago`;
    return `${diffInYears} year${diffInYears === 1 ? '' : 's'} ago`;
  }

  private setupTimer(seconds: number, minutes: number, hours: number) {
    // Clear previous timer
    if (this._timer) this._timer.unsubscribe();

    let refreshRate = 60000; // default: 1 minute
    if (seconds < 60) refreshRate = 1000;      // refresh every second
    else if (minutes < 60) refreshRate = 60000; // refresh every minute
    else if (hours < 24) refreshRate = 3600000; // refresh every hour

    this._timer = interval(refreshRate).subscribe(() => {
      this.cd.markForCheck(); // tell Angular to update view
    });
  }

  ngOnDestroy(): void {
    this._timer?.unsubscribe();
  }
}
