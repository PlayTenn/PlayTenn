import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
import { TrainingInfo, Training } from "../models/training";

@Injectable({
 providedIn: "root",
})
export class TrainingApiService {
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) { }

 public getAllTrainings(): Observable<Training[]> {
  return this.httpClient.get<Training[]>(`${this.apiUrl}api/Trainings/all`);
 }

 public createNewTraining(training: TrainingInfo) {
  return this.httpClient.post(`${this.apiUrl}api/TrainingsUsers/newTraining`, training);
 }

 public deleteTraining(id: number) {
  return this.httpClient.delete(`${this.apiUrl}api/Trainings/${id}`);
 }

 public getTrainingById(id: number): Observable<Training> {
  return this.httpClient.get<Training>(`${this.apiUrl}api/Trainings/${id}/details`);
 }

 public subscribeOnTraining(id: number) {
  return this.httpClient.post(`${this.apiUrl}api/Trainings/${id}/participants`, {});
 }

 public unsubscribeFromTraining(id: number) {
  return this.httpClient.delete(`${this.apiUrl}api/Trainings/${id}/participants`);
 }
}