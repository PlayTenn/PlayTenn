import { Component, OnInit } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { SessionExpiredComponent } from './components/session-expired/session-expired.component';
import { User } from './models/user';
import { paths } from './routes';
import { AuthGuard } from './services/auth.guard';
import { ImageApiService } from './services/image-api.service';
import { SessionService } from './services/session.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  public currentDate = new Date().getFullYear();
  public authPath: string = paths.auth;
  public homePath: string = paths.home;
  public profilePath: string = paths.profile;
  public placesPath: string = paths.places;
  public usersPath: string = paths.users;
  public calendarPath: string = paths.calendar;
  public trainingsPath: string = paths.trainings;
  public tournamentsPath: string = paths.tournaments;
  public hideHeader: boolean = false;
  public user!: User;

  constructor(private readonly session: SessionService,
    private readonly router: Router,
    private readonly snackBar: MatSnackBar,
    private readonly authGuard: AuthGuard,
    private readonly bottomSheet: MatBottomSheet,
    private readonly imageService: ImageApiService) { }

  ngOnInit(): void {
    this.authGuard.getMessage().subscribe(error => {
      this.snackBar.open(error, 'Close', {
        duration: 3000
      });
    });
    this.session.showExpiredDialog().subscribe(() => {
      this.hideHeader = true;
      const bottomSheetRef = this.bottomSheet.open(SessionExpiredComponent);
      bottomSheetRef.afterDismissed().subscribe(() => {
        this.hideHeader = false;
      });
    });
  }

  public isLoggedIn() {
    return !!this.session.getSession();
  }

  public isAdmin() {
    return this.session.isAdmin();
  }

  public logOut() {
    this.session.removeSession();
    this.router.navigateByUrl(this.authPath);
  }

  public goToProfile(isSettingTab: boolean) {
    this.router.navigate([`${paths.profile}`], { state: { isSettingTab: isSettingTab } })
  }

  public getUser(): User {
    const user = this.session.getUserFromSession();

    if (!user.profileImageUrl) {
      user.profileImageFilename = "../../../assets/default-avatar.png";
    }

    return user;
  }
}
