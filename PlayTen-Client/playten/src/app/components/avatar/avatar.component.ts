import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ImageApiService } from 'src/app/services/image-api.service';
import { SessionService } from 'src/app/services/session.service';

@Component({
  selector: 'app-avatar',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.less']
})
export class AvatarComponent {
  public fileName = '';
  @Output() isPhotoLoaded = new EventEmitter<boolean>(false);

  constructor(private readonly imageService: ImageApiService, private readonly sessionService: SessionService) {}

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      const userId = this.sessionService.getUserFromSession().id;
      const formData = new FormData();
      formData.append("file", file);

      this.imageService.uploadAvatar(userId, formData).subscribe((response:any) => {
        this.reloadProfile();
        this.sessionService.setAvatarUrl(response.avatarUrl)
        this.imageService.setIsAvatarChanged();
      });
    }
  }

  reloadProfile(): void {
    this.isPhotoLoaded.emit(true);
  }
}
