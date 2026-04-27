import { CommonModule } from "@angular/common";
import { Component, input } from "@angular/core";
import { PageLoaderService } from "../../core/services/page-loader.service";

@Component({
  selector: 'app-page-loader',
  imports: [CommonModule],
  template: `
    @if(loaderService.state()){
      <div class="backdrop">
        <div class="spinner-stack">
          <span class="ring ring-1"></span>
          <span class="ring ring-2"></span>
        </div>
      </div>
    }
    @if(containerLoader()){
      <div class="container-backdrop">
        <div class="spinner-stack">
          <span class="ring ring-1"></span>
          <span class="ring ring-2"></span>
        </div>
      </div>
    }
  `,
  styles: `
    .backdrop {
      position: fixed;
      top: 0;
      left: 0;
      z-index: 9998;
      background-color: rgba(31, 32, 48, 0.5);
      backdrop-filter: blur(6px);
      -webkit-backdrop-filter: blur(6px);
      width: 100dvw;
      height: 100dvh;
      display: flex;
      justify-content: center;
      align-items: center;
      animation: fade-in 200ms ease-out;
    }
    .container-backdrop {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(255, 255, 255, 0.6);
      backdrop-filter: blur(4px);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 50;
      animation: fade-in 200ms ease-out;
    }
    .spinner-stack {
      position: relative;
      width: 56px;
      height: 56px;
    }
    .ring {
      position: absolute;
      inset: 0;
      border-radius: 50%;
      border: 3px solid transparent;
      box-sizing: border-box;
    }
    .ring-1 {
      border-top-color: #ec4366;
      border-right-color: #ec4366;
      animation: rotation 0.9s cubic-bezier(0.65, 0, 0.35, 1) infinite;
    }
    .ring-2 {
      border-bottom-color: #7c5cff;
      border-left-color: #7c5cff;
      animation: rotation-reverse 1.4s cubic-bezier(0.65, 0, 0.35, 1) infinite;
      transform: scale(0.7);
      opacity: 0.7;
    }
    @keyframes rotation {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }
    @keyframes rotation-reverse {
      0% { transform: scale(0.7) rotate(0deg); }
      100% { transform: scale(0.7) rotate(-360deg); }
    }
    @keyframes fade-in {
      from { opacity: 0; }
      to { opacity: 1; }
    }
  `
})
export class PageLoaderComponent {
  containerLoader = input<boolean>(false);

  constructor(public loaderService: PageLoaderService) { }
}
