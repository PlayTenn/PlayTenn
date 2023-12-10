import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
import { User } from "../models/user";

@Injectable({
 providedIn: "root",
})
export class UserApiService {
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) {}

 public getUserById(userId: string): Observable<User> {
  return this.httpClient.get<User>(`${this.apiUrl}api/User/${userId}`);
 }

 public getAllUsers(): Observable<User[]> {
  return this.httpClient.get<User[]>(`${this.apiUrl}api/User/all`);
 }

 public deleteUserById(userId: string): Observable<any> {
  return this.httpClient.delete(`${this.apiUrl}api/User/delete/${userId}`);
 }
}