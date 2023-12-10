import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
import { Surface } from "../models/surface";

@Injectable({
 providedIn: "root",
})
export class SurfacesApiService {
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) { }

 public getAllSurfaces(): Observable<Surface[]> {
  return this.httpClient.get<Surface[]>(`${this.apiUrl}api/Surface/all`);
 }

 public getSurfaceById(surfaceId?: number): Observable<Surface> {
  return this.httpClient.get<Surface>(`${this.apiUrl}api/Surface/${surfaceId}`);
 }
}
