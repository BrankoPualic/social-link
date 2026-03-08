import { Component, input, output } from "@angular/core";
import { FormatTextPipe } from "../../../../core/pipes/format-text.pipe";
import { IConfirmable } from "../../../../shared/interfaces/confirmable.interface";

@Component({
  selector: 'app-description-dialog',
  imports: [FormatTextPipe],
  templateUrl: './description-dialog.html'
})
export class DescriptionDialog implements IConfirmable {
  description = input('');
  result = output<boolean>();

  cancel = () => this.result.emit(false);
}
