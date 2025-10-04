import { ApplicationRef, ComponentRef, EnvironmentInjector, Injectable, Type, createComponent } from '@angular/core';
import { ConfirmDialog } from '../../shared/components/dialogs/confirm-dialog/confirm-dialog';
import { IConfirmable } from '../../shared/interfaces/confirmable.interface';
import { ConfirmDeleteDialog } from '../../shared/components/dialogs/confirm-delete-dialog/confirm-delete-dialog';

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  constructor(
    private appRef: ApplicationRef,
    private injector: EnvironmentInjector
  ) { }

  confirmDelete = (message?: string, title?: string) => this.open(ConfirmDeleteDialog, message, title);
  confirm = (message?: string, title?: string) => this.open(ConfirmDialog, message, title);

  // private

  private open<T extends IConfirmable>(component: Type<T>, message?: string, title?: string): { result: Promise<boolean> } {
    const componentRef: ComponentRef<T> = createComponent(component, {
      environmentInjector: this.injector
    });

    if (message)
      componentRef.setInput('message', message);

    if (title)
      componentRef.setInput('title', title);

    this.appRef.attachView(componentRef.hostView);
    document.body.appendChild(componentRef.location.nativeElement);

    const result = new Promise<boolean>(resolve => {
      const sub = componentRef.instance.result.subscribe(value => {
        if (value)
          resolve(value);

        sub.unsubscribe();
        this.appRef.detachView(componentRef.hostView);
        componentRef.destroy();
      })
    });

    return { result };
  }
}
