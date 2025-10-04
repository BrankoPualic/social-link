import { Component, input, output } from '@angular/core';
import { Functions } from '../../../functions';
import { IConfirmable } from '../../../interfaces/confirmable.interface';

@Component({
  selector: 'app-confirm-delete-dialog',
  imports: [],
  templateUrl: './confirm-delete-dialog.html'
})
export class ConfirmDeleteDialog implements IConfirmable {
  title = input('Delete');
  message = input('Are you sure you want to delete this?');

  result = output<boolean>();

  confirm = () => this.result.emit(true);
  cancel = () => this.result.emit(false);

  formatString = (value: string) => Functions.formatString(value);
}
