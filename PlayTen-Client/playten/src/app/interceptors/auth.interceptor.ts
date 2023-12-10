import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SessionService } from "../services/session.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
 constructor(private readonly session: SessionService) {}

 intercept(req: HttpRequest<any>,
  next: HttpHandler): Observable<HttpEvent<any>> {

  const idToken = this.session.getSession();

  if (idToken) {
   const cloned = req.clone({
    headers: req.headers.set("Authorization",
     "Bearer " + idToken)
   });

   return next.handle(cloned);
  }
  else {
   return next.handle(req);
  }
 }
}