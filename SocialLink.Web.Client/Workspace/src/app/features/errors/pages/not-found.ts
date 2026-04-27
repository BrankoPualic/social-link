import { Location } from "@angular/common";
import { Component } from "@angular/core";

@Component({
  selector: 'app-not-found',
  template: `
    <div class="error-page">
      <div class="error-card anim-pop">
        <div class="error-code">404</div>
        <div class="error-title">Page not found</div>
        <div class="error-subtitle">The page you're looking for doesn't exist or has been moved.</div>
        <button class="btn btn-primary mt-4" (click)="goBack()">
          <i class="fa-solid fa-arrow-left me-2"></i>Go back
        </button>
      </div>
    </div>
  `,
  styles: `
    @import '../../../../assets/styles/variables.scss';

    .error-page {
      min-height: 100dvh;
      display: flex;
      align-items: center;
      justify-content: center;
      background: $gradient-aurora;
      padding: 2rem 1rem;
    }
    .error-card {
      background: rgba(255, 255, 255, 0.85);
      backdrop-filter: blur(18px);
      border: 1px solid rgba(255, 255, 255, 0.6);
      border-radius: $radius-xl;
      padding: 3rem 3rem 2.5rem;
      box-shadow: $shadow-lg;
      text-align: center;
      max-width: 480px;
      width: 100%;
    }
    .error-code {
      font-size: 5rem;
      font-weight: 800;
      letter-spacing: -0.04em;
      background: $gradient-primary;
      -webkit-background-clip: text;
      background-clip: text;
      -webkit-text-fill-color: transparent;
      line-height: 1;
    }
    .error-title {
      font-size: 1.4rem;
      font-weight: 700;
      color: $ink;
      margin-top: 0.5rem;
    }
    .error-subtitle {
      color: $muted;
      margin-top: 0.4rem;
    }
  `
})
export class NotFound {
  constructor(private location: Location) { }

  goBack = () => this.location.back();
}
