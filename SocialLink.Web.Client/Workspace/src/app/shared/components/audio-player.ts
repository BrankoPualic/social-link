import { AfterViewInit, CUSTOM_ELEMENTS_SCHEMA, Component, ElementRef, input, viewChild } from '@angular/core';

@Component({
  selector: 'app-audio-player',
  imports: [],
  template: `<div #container></div>`,
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AudioPlayer implements AfterViewInit {
  audioSrc = input('');
  myMessage = input(false);
  container = viewChild<ElementRef<HTMLDivElement>>('container');

  ngAfterViewInit() {
    const primaryColor = this.myMessage() ? '#fff' : '#ff677e';

    const template = `<wave-audio-path-player
      src="${this.audioSrc()}"
      wave-width="200"
      wave-height="40"
      color="${primaryColor}"
      wave-progress-color="${primaryColor}"
      wave-slider="${this.myMessage() ? primaryColor : '#373233'}"
      class="my-audio ${this.myMessage() ? 'is-mine' : ''}"></wave-audio-path-player>`;

    this.container()!.nativeElement.innerHTML = template;
  }
}
