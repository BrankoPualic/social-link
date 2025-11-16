import { Pipe, PipeTransform } from "@angular/core";
import { DomSanitizer, SafeHtml } from "@angular/platform-browser";

@Pipe({
  name: 'formatText',
  standalone: true
})
export class FormatTextPipe implements PipeTransform {

  transform(value?: string): string {
    if (!value)
      return '';

    const escaped = this.escapeHtml(value);

    const withBreaks = escaped.replace(/\n/g, '<br/>');

    const formatted = this.applyFormatting(withBreaks);

    return formatted;
  }

  private escapeHtml(str: string): string {
    return str
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  }

  private applyFormatting(str: string): string {
    // Example: auto-link URLs (still safe because HTML was escaped first)
    return str.replace(
      /(https?:\/\/[^\s]+)/g,
      `<a href="$1" target="_blank" rel="noopener noreferrer">$1</a>`
    );
  }
}
