import { Component, input, output } from '@angular/core';
import { FormatTextPipe } from '../../../../core/pipes/format-text.pipe';
import { IConfirmable } from '../../../interfaces/confirmable.interface';

@Component({
  selector: 'app-confirm-delete-dialog',
  imports: [FormatTextPipe],
  templateUrl: './confirm-delete-dialog.html'
})
export class ConfirmDeleteDialog implements IConfirmable {
  title = input('Delete');
  message = input('Are you sure you want to delete this?');

  result = output<boolean>();

  confirm = () => this.result.emit(true);
  cancel = () => this.result.emit(false);
}
