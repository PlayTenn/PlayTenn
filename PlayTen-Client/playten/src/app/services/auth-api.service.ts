import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
import { Auth } from "../models/auth";
import { SignIn } from "../models/signIn";
import { SignUp } from "../models/signUp";

@Injectable({
 providedIn: "root",
})
export class AuthApiService {
 private readonly apiUrl = environment.apiUrl;
 constructor(private readonly httpClient: HttpClient) { }

 public signin(signInData: SignIn): Observable<Auth> {
  return this.httpClient.post<Auth>(`${this.apiUrl}api/Auth/signin`, signInData);
 }

 public signup(signUpData: SignUp): Observable<Auth> {
  return this.httpClient.post<Auth>(`${this.apiUrl}api/Auth/signup`, signUpData);
 }
}