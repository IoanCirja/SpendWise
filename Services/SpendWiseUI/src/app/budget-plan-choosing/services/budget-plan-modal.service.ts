import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BudgetPlanService {

  private endPoint = 'https://localhost:7154/MonthlyPlan/AddMonthlyPlans';

  constructor(private http: HttpClient) { }

  saveBudgetPlan(planData: any): Observable<any> {
    console.log(planData);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(this.endPoint, planData, { headers, responseType: 'text' as 'json' });
  }
}
