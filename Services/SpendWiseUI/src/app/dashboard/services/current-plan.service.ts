import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MonthlyPlan } from '../models/MonthlyPlan';

@Injectable({
  providedIn: 'root'
})
export class CurrentPlanService {

  private apiUrlGetPlans = 'https://localhost:7154/MonthlyPlan/GetCurrentPlan';
  private apiUrlCancelPlan = 'https://localhost:7154/MonthlyPlan/CancelMonthlyPlans';

  constructor(private http: HttpClient) { }

  getCurrentPlan(userId: string): Observable<MonthlyPlan[]> {
    const url = `${this.apiUrlGetPlans}/${userId}`;
    return this.http.get<MonthlyPlan[]>(url);
  }

  cancelCurrentPlan(monthlyPlan_id: string): Observable<any> {
    console.log(`"${monthlyPlan_id}"`);
    return this.http.post<any>(this.apiUrlCancelPlan, `"${monthlyPlan_id}"`, {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
