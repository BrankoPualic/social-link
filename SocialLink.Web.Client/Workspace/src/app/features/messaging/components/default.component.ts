import { Component } from "@angular/core";
import { BaseMessageBoxComponent } from "./_base-message-box-component";
import { FormatTextPipe } from "../../../core/pipes/format-text.pipe";

@Component({
  selector: 'app-default-message-box',
  imports: [FormatTextPipe],
  template: `
  <div class="message px-2 py-1"
               [class.is-mine]="isFromCurrentUser(message()?.userId)"
               [class.is-others]="!isFromCurrentUser(message()?.userId)"
               [class.rounded-3]="isMultiline(message()?.content)"
               [class.rounded-4]="!isMultiline(message()?.content)"
               [innerHtml]="message()?.content | formatText"></div>
  `,
  styles: `
  @import '../../../../assets/styles/variables.scss';
  .message {
        &.is-mine {
          background-color: $primary;
          color: $white;
        }

        &.is-others {
          background-color: $smoke-white;
        }
    }
  `
})
export class DefaultComponent extends BaseMessageBoxComponent<string>{
  isMultiline = (content?: string) => !!content && content.includes('\n');
}
