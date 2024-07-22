import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeletePlanService {
  private apiUrl = 'https://localhost:7154/BudgetPlan/DeletePlanById'; 

  constructor(private http: HttpClient) {}

  deletePlan(planId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${planId}`);
  }
}
