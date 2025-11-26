import { Component, output } from "@angular/core";
import { AudioService } from "../../../core/services/audio.service";

@Component({
  selector: 'app-voice-recorder-button',
  templateUrl: './voice-recorder-button.html',
  styleUrl: './voice-recorder-button.scss'
})
export class VoiceRecorderButton {
  isRecording = false;
  maxTime = 150000; // 2.5 minutes in ms
  startTime = 0;
  progressOffset = 282; // full circle length

  private timer!: any;
  private recorder!: { start: () => void, stop: () => Promise<{ audioBlob: Blob, audioUrl: string, play: () => void }> };

  audioEmitter = output<Blob>();

  constructor(private audioService: AudioService) { }

  async startRecording() {
    if (this.isRecording) return;

    this.isRecording = true;
    this.startTime = Date.now();

    this.recorder = await this.audioService.record();
    this.recorder.start();

    this.timer = setInterval(() => {
      const elapsed = Date.now() - this.startTime;
      const progress = elapsed / this.maxTime;

      this.progressOffset = 282 - (282 * progress);

      if (elapsed >= this.maxTime)
        this.stopRecording();
    }, 100);
  }

  async stopRecording() {
    if (!this.isRecording) return;

    this.isRecording = false;
    clearInterval(this.timer);
    this.progressOffset = 282;

    const data = await this.recorder.stop();

    this.audioEmitter.emit(data.audioBlob);
  }
}
