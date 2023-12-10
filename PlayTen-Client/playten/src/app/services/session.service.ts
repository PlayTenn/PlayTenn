import { Injectable } from "@angular/core";
import { Auth } from "../models/auth";
import { User } from "../models/user";
import jwt_decode from "jwt-decode";
import { Observable, Subject } from "rxjs";

@Injectable({
 providedIn: "root",
})
export class SessionService {
 private subject = new Subject();

 public setSession(authResult: Auth) {
  var jwtDecoded = jwt_decode(authResult.token) as any;
  const date = new Date(jwtDecoded['exp'] * 1000);
  setTimeout(() => {
   this.subject.next(null);
  }, date.getTime() - Date.now());
  localStorage.setItem('id_token', authResult.token);
  localStorage.setItem("expires_at", JSON.stringify(date));
  localStorage.setItem('name', authResult.user.name);
  localStorage.setItem('surname', authResult.user.surname);
  localStorage.setItem('email', authResult.user.email);
  localStorage.setItem('user_id', authResult.user.id);
  var userRole = jwtDecoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  localStorage.setItem('user_role', userRole);
  localStorage.setItem('avatar_url', authResult.user.profileImageUrl);
 }

 public setAvatarUrl(avatarUrl: string) {
  localStorage.setItem('avatar_url', avatarUrl);
 }

 public removeAvatarUrlFromSession() {
  localStorage.removeItem('avatar_url');
 }

 public getSession(): string | null {
  return localStorage.getItem('id_token');
 }

 public getUserFromSession(): User {
  var id = localStorage.getItem('user_id') ?? "";
  var name = localStorage.getItem('name') ?? "";
  var surname = localStorage.getItem('surname') ?? "";
  var email = localStorage.getItem('email') ?? "";
  var profileImageUrl = localStorage.getItem('avatar_url') ?? "";

  var user: User = {
   id: id,
   name: name,
   surname: surname,
   email: email,
   profileImageUrl: profileImageUrl
  };

  return user;
 }

 public isAdmin() {
  return localStorage.getItem('user_role') === 'Admin';
 }

 public removeSession() {
  localStorage.removeItem("id_token");
  localStorage.removeItem("user_id");
  localStorage.removeItem("expires_at");
  localStorage.removeItem("name");
  localStorage.removeItem("surname");
  localStorage.removeItem("email");
  localStorage.removeItem("user_role");
  localStorage.removeItem('avatar_url');
 }

 public showExpiredDialog(): Observable<any> {
  return this.subject.asObservable();
 }

 public prolongLeaseExpirationTime() {
  var oldStringDate = localStorage.getItem("expires_at");
  if (oldStringDate) {
   var oldDate = new Date(oldStringDate);
   var date = new Date(oldDate.getTime() + 120 * 60000);
   localStorage.setItem("expires_at", JSON.stringify(date));
   setTimeout(() => {
    this.subject.next(null);
   }, date.getTime() - Date.now());
  }
 }
}
