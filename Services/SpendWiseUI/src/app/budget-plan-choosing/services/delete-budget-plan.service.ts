import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeletePlanService {
  private apiUrl = 'https://localhost:7154/BudgetPlan/DeletePlanById'; 

  constructor(private http: HttpClient) {}

  deletePlan(planId: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.delete<any>(`${this.apiUrl}/${planId}`, { headers, responseType: 'text' as 'json' });
  }
}

