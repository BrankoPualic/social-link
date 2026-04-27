import { Component } from "@angular/core";
import { BaseMessageBoxComponent } from "./_base-message-box-component";
import { FormatTextPipe } from "../../../core/pipes/format-text.pipe";

@Component({
  selector: 'app-default-message-box',
  imports: [FormatTextPipe],
  template: `
  <div class="message"
               [class.is-mine]="isFromCurrentUser(message()?.userId)"
               [class.is-others]="!isFromCurrentUser(message()?.userId)"
               [innerHtml]="message()?.content | formatText"></div>
  `,
  styles: `
  @import '../../../../assets/styles/variables.scss';
  .message {
        border-radius: 18px;
        padding: 0.55rem 0.9rem;
        line-height: 1.4;
        font-size: 0.94rem;
        word-wrap: break-word;

        &.is-mine {
          background: $gradient-primary;
          color: $white;
          border-bottom-right-radius: 6px;
          box-shadow: 0 4px 14px rgba(236, 67, 102, 0.22);
        }

        &.is-others {
          background-color: $smoke-white;
          color: $ink;
          border-bottom-left-radius: 6px;
          box-shadow: 0 1px 2px rgba(31, 32, 48, 0.04);
        }
    }
  `
})
export class DefaultComponent extends BaseMessageBoxComponent<string>{
  isMultiline = (content?: string) => !!content && content.includes('\n');
}
