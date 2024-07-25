import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HistoryPlan } from '../models/HistoryPlan';
import { MonthlyPlan } from '../models/MonthlyPlan';
@Injectable({
  providedIn: 'root'
})
export class HistoryService {

  private endPoint = 'https://localhost:7154/MonthlyPlan/GetHistoryPlans';
  private endPointForASinglePlan = 'https://localhost:7154/MonthlyPlan/GetPlanFromHistory';


  constructor(private http: HttpClient) { }

  getHistoryPlans(userId:string): Observable<HistoryPlan[]> {
    const url = `${this.endPoint}/${userId}`;
    return this.http.get<HistoryPlan[]>(url);
  }
  getPlanFromHistory(userId:string): Observable<MonthlyPlan> {
    const url = `${this.endPoint}/${userId}`;
    return this.http.get<MonthlyPlan>(url);
  }
}
