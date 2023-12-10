import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
import { Place } from "../models/place";

@Injectable({
 providedIn: "root",
})
export class PlacesApiService {
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) { }

 public getAllPlaces(): Observable<Place[]> {
  return this.httpClient.get<Place[]>(`${this.apiUrl}api/Place/all`);
 }

 public deletePlaceById(placeId: number) {
  return this.httpClient.delete(`${this.apiUrl}/api/Place/delete/${placeId}`);
 }

 public createNewPlace(place: Place){
  return this.httpClient.post(`${this.apiUrl}api/Place/new`, place);
 }
}