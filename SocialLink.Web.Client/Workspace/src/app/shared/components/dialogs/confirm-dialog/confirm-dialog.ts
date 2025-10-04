import { Component, input, output } from '@angular/core';
import { Functions } from '../../../functions';
import { IConfirmable } from '../../../interfaces/confirmable.interface';

@Component({
  selector: 'app-confirm-dialog',
  imports: [],
  templateUrl: './confirm-dialog.html'
})
export class ConfirmDialog implements IConfirmable {
  title = input('Confirm');
  message = input('Are you sure?');

  result = output<boolean>();

  confirm = () => this.result.emit(true);
  cancel = () => this.result.emit(false);

  formatString = (value: string) => Functions.formatString(value);
}
