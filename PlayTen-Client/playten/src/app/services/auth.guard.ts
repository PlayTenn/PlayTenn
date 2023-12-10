import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { paths } from '../routes';
import { SessionService } from './session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private subject = new Subject<string>();
  constructor(private readonly session: SessionService,
    private readonly router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    var session = this.session.getSession();
    if (session) {
      return true;
    }
    this.router.navigateByUrl(paths.auth);
    this.subject.next("Please sign in first");
    return false;
  }

  public getMessage(): Observable<string> {
    return this.subject.asObservable();
  }
}
