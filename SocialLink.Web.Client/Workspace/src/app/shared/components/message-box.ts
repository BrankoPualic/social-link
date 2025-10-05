import { Component, ElementRef, output, viewChild } from "@angular/core";
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-message-box',
  imports: [FormsModule],
  template: `
  <div class="d-flex position-relative">
      <textarea #textarea
                class="form-control"
                [(ngModel)]="value"
                (input)="autoResize()"
                rows="1"
                style="resize:none; width:100%; margin-right: 3.5rem;">
      </textarea>
      <button class="btn btn-sm btn-primary px-3 position-absolute" style="right: 0; bottom: 0; padding: 0.5rem;" (click)="send()"><i class="fa-solid fa-arrow-right"></i></button>
  </div>
  `
})
export class MessageBox {
  textarea = viewChild<ElementRef<HTMLTextAreaElement>>('textarea');
  submit = output<string>();
  value = '';

  maxRows = 3;

  send = () => { this.submit.emit(this.value); this.value = ''; }

  autoResize(): void {
    const el = this.textarea()!.nativeElement;
    el.style.height = 'auto';
    const lineHeight = this.getLineHeight(el);
    const maxHeight = lineHeight * this.maxRows;

    const newHeight = el.scrollHeight;
    const finalHeight = Math.min(newHeight, maxHeight);

    el.style.height = finalHeight + 'px';

    el.style.overflowY = newHeight > maxHeight ? 'auto' : 'hidden';
  }

  private getLineHeight(el: HTMLElement): number {
    const lineHeight = window.getComputedStyle(el).lineHeight;
    return parseFloat(lineHeight || '20');
  }
}
