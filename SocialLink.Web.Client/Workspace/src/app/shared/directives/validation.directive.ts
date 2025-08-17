import { Directive, ElementRef, Renderer2, effect, input } from "@angular/core";
import { ErrorService } from "../../core/services/error.service";
import { Error } from '../../core/models/error';

@Directive({
  selector: '[appValidation]'
})
export class ValidationDirective {
  appValidation = input<string>();

  constructor(
    private errorService: ErrorService,
    private el: ElementRef,
    private renderer: Renderer2
  ) {
    effect(() => {
      this.handleErrors(this.errorService.errors());
    })
  }

  handleErrors(errors: Error[]) {
    const existingErrors = this.el.nativeElement.querySelectorAll('.validation-feedback');
    existingErrors.forEach((el: HTMLDivElement) => {
        this.renderer.removeChild(this.el.nativeElement, el);
    });

    const formElements = this.el.nativeElement.querySelectorAll('input, select, textarea');
    formElements.forEach((el: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement) => {
      this.renderer.removeClass(el, 'is-invalid');
      this.renderer.removeClass(el, 'is-valid');
    });

    if (errors.length == 0)
      return;

    const error = errors.find(_ => _.key === this.appValidation());

    formElements.forEach((el: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement) => {
      let hasValue = !!el.value && el.value.trim().length > 0;

      const hasError = error && error.errors!.some(_ => _);

      if (hasValue && !hasError)
        this.renderer.addClass(el, 'is-valid');
      else if (hasError)
        this.renderer.addClass(el, 'is-invalid');
    });

    if (error) {
      error.errors?.forEach(_ => {
        const errorElement = this.renderer.createElement('div');
        this.renderer.addClass(errorElement, 'validation-feedback');
        this.renderer.addClass(errorElement, 'text-end');
        this.renderer.addClass(errorElement, 'pe-2');
        errorElement.innerText = _;

        this.renderer.appendChild(this.el.nativeElement, errorElement);
      });
    }
    else {
      formElements.forEach((el: HTMLElement) => {
        this.renderer.removeClass(el, 'is-invalid');
      });
    }
  }
}
