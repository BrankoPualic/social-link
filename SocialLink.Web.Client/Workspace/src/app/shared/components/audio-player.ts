import { CUSTOM_ELEMENTS_SCHEMA, Component, ElementRef, OnChanges, SimpleChanges, input, viewChild } from '@angular/core';

@Component({
  selector: 'app-audio-player',
  imports: [],
  template: `<div #container></div>`,
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AudioPlayer implements OnChanges {
  audioBlob = input<Blob>();
  audioSrc = input('');
  isFromCurrentUser = input(false);
  container = viewChild<ElementRef<HTMLDivElement>>('container');

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes['audioSrc']?.currentValue && !changes['audioBlob']?.currentValue) return;

    const primaryColor = this.isFromCurrentUser() ? '#fff' : '#ff677e';

    const src = !!this.audioBlob() ? URL.createObjectURL(this.audioBlob()!) : this.audioSrc();

    const template = `<wave-audio-path-player
      src="${src}"
      wave-width="100"
      wave-height="40"
      color="${primaryColor}"
      wave-progress-color="${primaryColor}"
      wave-slider="${this.isFromCurrentUser() ? primaryColor : '#373233'}"
      class="my-audio ${this.isFromCurrentUser() ? 'is-mine' : ''}"></wave-audio-path-player>`;

    this.container()!.nativeElement.innerHTML = template;
  }
}
