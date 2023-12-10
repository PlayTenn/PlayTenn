import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, Observable, throwError } from "rxjs";
import { paths } from "../routes";
import { SessionService } from "../services/session.service";

@Injectable()
export class Http401Interceptor implements HttpInterceptor {
 constructor(private readonly router: Router,
  private readonly session: SessionService) { }


 intercept(
  request: HttpRequest<any>,
  next: HttpHandler
 ): Observable<HttpEvent<any>> {
  return next.handle(request).pipe(
   catchError((error: HttpErrorResponse) => {
    if (error.status === 401) {
     this.session.removeSession();
     this.router.navigateByUrl(paths.auth);
    } else {
     return throwError(error.message);
    }
    return next.handle(request);
   })
  );
 }
}
