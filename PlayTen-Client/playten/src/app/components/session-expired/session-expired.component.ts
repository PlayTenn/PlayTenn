import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { Router } from '@angular/router';
import { Subscription, timer } from 'rxjs';
import { paths } from 'src/app/routes';
import { SessionService } from 'src/app/services/session.service';

@Component({
  selector: 'app-session-expired',
  templateUrl: './session-expired.component.html',
  styleUrls: ['./session-expired.component.less']
})
export class SessionExpiredComponent implements OnInit, OnDestroy {
  public subscribeTimer: number = 60;
  private expirationSubscription = new Subscription();
  constructor(private readonly session: SessionService,
    private readonly bottomSheet: MatBottomSheet,
    private readonly router: Router) { }

  ngOnInit(): void {
    const expirationTime = 60;
    const source = timer(1000, 1000);
    this.expirationSubscription = source.subscribe(value => {
      this.subscribeTimer = expirationTime - value;
      if (this.subscribeTimer < 0) {
        this.onLogoutClick();
      }
    });
  }

  ngOnDestroy(): void {
    this.expirationSubscription.unsubscribe();
  }

  public onLogoutClick() {
    this.session.removeSession();
    this.bottomSheet.dismiss();
    this.router.navigateByUrl(paths.auth);
  }

  public prolongLeaseExpirationTime() {
    this.session.prolongLeaseExpirationTime();
    this.bottomSheet.dismiss();
  }
}
