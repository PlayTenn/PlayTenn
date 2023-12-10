import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
import { TennisLevel } from "../models/tennisLevel";

@Injectable({
 providedIn: "root",
})
export class TennisLevelApiService {
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) { }

 public getAllTennisLevels(): Observable<TennisLevel[]> {
  return this.httpClient.get<TennisLevel[]>(`${this.apiUrl}api/TennisLevel/all`);
 }
}