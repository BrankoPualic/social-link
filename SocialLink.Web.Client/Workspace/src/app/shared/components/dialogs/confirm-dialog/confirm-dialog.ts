import { Component, input, output } from '@angular/core';
import { FormatTextPipe } from '../../../../core/pipes/format-text.pipe';
import { IConfirmable } from '../../../interfaces/confirmable.interface';

@Component({
  selector: 'app-confirm-dialog',
  imports: [FormatTextPipe],
  templateUrl: './confirm-dialog.html'
})
export class ConfirmDialog implements IConfirmable {
  title = input('Confirm');
  message = input('Are you sure?');

  result = output<boolean>();

  confirm = () => this.result.emit(true);
  cancel = () => this.result.emit(false);
}
