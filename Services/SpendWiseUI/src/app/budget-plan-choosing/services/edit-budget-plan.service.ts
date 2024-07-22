import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EditPlanService {
  private apiUrl = 'https://localhost:7154/BudgetPlan/EditPlanByPlanId'; 

  constructor(private http: HttpClient) {}

  updatePlan(planId: string, planData: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${planId}`, planData);
  }
}
