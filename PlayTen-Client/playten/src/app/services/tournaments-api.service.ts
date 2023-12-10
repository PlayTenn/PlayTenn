
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
import { TrainingInfo, Training } from "../models/training";
import { Tournament, TournamentInfo } from "../models/tournament";
import { Match } from "../models/match";

@Injectable({
 providedIn: "root",
})
export class TournamentsApiService {
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) { }

 public getAllTournaments(): Observable<Tournament[]> {
  return this.httpClient.get<Tournament[]>(`${this.apiUrl}api/Tournament/all`);
 }

 public createNewTournament(tournament: TournamentInfo) {
  return this.httpClient.post(`${this.apiUrl}api/TournamentsUsers/newTournament`, tournament);
 }

 public deleteTournament(id: number) {
  return this.httpClient.delete(`${this.apiUrl}api/Tournament/${id}`);
 }

 public getTournamentById(id: number): Observable<Tournament> {
  return this.httpClient.get<Tournament>(`${this.apiUrl}api/Tournament/${id}/details`);
 }

 public generateDraw(id: number) {
  return this.httpClient.post(`${this.apiUrl}api/Match/${id}/generateDraw`, {});
 }

 public getAllMatches(id: number) {
  return this.httpClient.get(`${this.apiUrl}api/Match/getAllMatches/${id}`);
 }
 public updateMatch(match: Match) {
  return this.httpClient.put(`${this.apiUrl}api/Match/update`, match);
 }

 public editTournament(tournament: TournamentInfo) {
    return this.httpClient.put(`${this.apiUrl}api/TournamentsUsers/editedTournament`, tournament);
 }

 public startTournament(id: number) {
    return this.httpClient.post(`${this.apiUrl}api/TournamentsUsers/startTournament/${id}`, {});
 }

 public finishTournament(id: number) {
    return this.httpClient.post(`${this.apiUrl}api/TournamentsUsers/finishTournament/${id}`, {});
 }
}
