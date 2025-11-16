import { Component, ElementRef, input, output, viewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-message-input',
  imports: [FormsModule],
  templateUrl: './message-input.html',
  styleUrl: './message-input.scss'
})
export class MessageInput {
  placeholder = input('');
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

    const rowCount = this.getRowCount(el);
    el.style.borderRadius = rowCount > 2 ? "0.375rem" : "50px";
  }

  private getLineHeight(el: HTMLElement): number {
    const lineHeight = window.getComputedStyle(el).lineHeight;
    return parseFloat(lineHeight || '20');
  }

  private getRowCount(el: HTMLTextAreaElement): number {
    const style = window.getComputedStyle(el);
    const lineHeight = parseFloat(style.lineHeight);
    const rows = Math.round(el.scrollHeight / lineHeight);
    return rows;
  }
}
