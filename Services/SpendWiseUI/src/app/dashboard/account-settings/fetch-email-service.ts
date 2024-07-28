// src/app/services/personal-information.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


import { UserData } from './user-model';@Injectable({
  providedIn: 'root'
})
export class fetchUserDataService {
  private endPoint = 'https://localhost:7154/Authentication/SaveAccountSettings';

  constructor(private http: HttpClient) { }

  getUserData(userId: string): Observable<UserData> {
    return this.http.get<UserData>(`${this.endPoint}/${userId}`);
  }
}
