import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BudgetPlanService {

  private endPoint = 'https://localhost:7154/MonthlyPlan/AddMonthlyPlans';

  constructor(private http: HttpClient) { }

  saveBudgetPlan(planData: any): Observable<any> {
    console.log(planData);
    return this.http.post<any>(this.endPoint, planData);
  }
}
