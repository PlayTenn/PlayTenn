import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { SessionService } from 'src/app/services/session.service';
import { UserApiService } from 'src/app/services/user-api.service';
import { ThemePalette } from '@angular/material/core';
import { Router } from '@angular/router';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ImageApiService } from 'src/app/services/image-api.service';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.less']
})
export class ProfileComponent implements OnInit {
  public spinnerMode: ProgressSpinnerMode = "indeterminate";
  public showSpinner: boolean = false;
  public links = ['Профіль', 'Налаштування'];
  public activeLink = this.links[0];
  public background: ThemePalette = 'primary';
  public user!: User;
  public avatar!: string;

  constructor(private readonly session: SessionService,
    private readonly userApi: UserApiService,
    private readonly router: Router,
    private readonly imageService: ImageApiService) {
    var isSettingTab = this.router.getCurrentNavigation()?.extras?.state?.['isSettingTab'];
    if (isSettingTab) {
      this.activeLink = this.links[1];
    }
  }

  ngOnInit(): void {
    this.showSpinner = true;
    var user = this.session.getUserFromSession();
    if (user.id) {
      this.userApi.getUserById(user.id).subscribe({
        next: (user: User) => {
          this.user = user;
          if (user.profileImageUrl && user.profileImageFilename) {
            this.avatar = user.profileImageUrl;
          } else {
            this.avatar = "";
          }
        },
        complete: () => {
          this.showSpinner = false;
        }
      });
    }
  }

  public reloadProfile() {
    this.ngOnInit();
  }

  public deletePhoto() {
    var user = this.session.getUserFromSession();
    if (user.id) {
      this.imageService.deleteAvatar(user.id).subscribe({
        next: () => {
          this.session.removeAvatarUrlFromSession();
          this.ngOnInit();
        }
      });
    }
  }

}
