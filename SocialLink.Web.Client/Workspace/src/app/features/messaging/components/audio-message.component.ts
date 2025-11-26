import { Component, OnInit, signal } from "@angular/core";
import { AudioPlayer } from "../../../shared/components/audio-player";
import { BaseMessageBoxComponent } from "./_base-message-box-component";
import { ApiService } from "../../../core/services/api.service";
import { AuthService } from "../../../core/services/auth.service";
import { BlobModel } from "../../../core/models/blob.model";
import { take } from "rxjs";

@Component({
  selector: 'app-audio-message',
  imports: [AudioPlayer],
  template: `
    <app-audio-player [audioSrc]="audioSrc()" [isFromCurrentUser]="isFromCurrentUser(message()?.userId)" />
  `,
})
export class AudioMessage extends BaseMessageBoxComponent<{ blobId?: string }> implements OnInit {
  audioSrc = signal('');

  constructor(
    authService: AuthService,
    private apiService: ApiService) {
    super(authService);
  }

  override ngOnInit(): void {
    super.ngOnInit();

    this.apiService.get<BlobModel>(`/Blob/Get/${this.data?.blobId}`).pipe(
      take(1)
    ).subscribe(response => { this.audioSrc.set(response.url!) });
  }
}
