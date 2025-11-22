import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class AudioService {
  record() {
    return new Promise<{ start: () => void, stop: () => Promise<{ audioBlob: Blob, audioUrl: string, play: () => void }> }>(async resolve => {
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      const mediaRecorder = new MediaRecorder(stream, { audioBitsPerSecond: 32000 });
      const audioChunks: BlobPart[] = [];

      mediaRecorder.addEventListener('dataavailable', event => audioChunks.push(event.data));

      const start = () => mediaRecorder.start();

      const stop = () => {
        return new Promise<{ audioBlob: Blob, audioUrl: string, play: () => void }>(resolve => {
          mediaRecorder.addEventListener('stop', () => {
            const audioBlob = new Blob(audioChunks);
            const audioUrl = URL.createObjectURL(audioBlob);
            const audio = new Audio(audioUrl);
            const play = () => {
              audio.play();
            };

            resolve({ audioBlob, audioUrl, play });
          });

          mediaRecorder.stop();
        });
      };

      resolve({ start, stop });
    });
  }

  sleep = (time: number) => new Promise(resolve => setTimeout(resolve, time));
}
