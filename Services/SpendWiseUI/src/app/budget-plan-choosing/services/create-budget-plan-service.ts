import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CreateBudgetPlanService {
  private apiUrl = 'https://localhost:7154/BudgetPlan/AddPlan'; 

  constructor(private http: HttpClient) {}

  createBudgetPlan(planData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, planData);
  }
}
