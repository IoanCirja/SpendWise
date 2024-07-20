import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MonthlyPlan } from '../models/MonthlyPlan';

@Injectable({
  providedIn: 'root'
})
export class CurrentPlanService {

  private apiUrl = 'https://localhost:7154/MonthlyPlan/GetCurrentPlan';

  constructor(private http: HttpClient) { }

  getCurrentPlan(userId: string): Observable<MonthlyPlan[]> {
    const url = `${this.apiUrl}/${userId}`;
    return this.http.get<MonthlyPlan[]>(url);
  }
}
