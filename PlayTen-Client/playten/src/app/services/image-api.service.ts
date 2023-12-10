import { HttpClient } from "@angular/common/http";
import { EventEmitter, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";

@Injectable({
 providedIn: "root",
})
export class ImageApiService {
 public isAvatarChanged = new EventEmitter<boolean>(false);
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) { }

 public uploadAvatar(userId: string, avatar: FormData): Observable<string> {
  return this.httpClient.post<string>(`${this.apiUrl}api/Accounts/uploadPhoto/${userId}`, avatar);
 }

 public deleteAvatar(userId: string): Observable<void> {
  return this.httpClient.delete<void>(`${this.apiUrl}api/Accounts/deletePhoto/${userId}`);
 }

 public setIsAvatarChanged() {
  this.isAvatarChanged.emit(true);
 }
}
