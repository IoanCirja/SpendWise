import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EditPlanService {
  private apiUrl = 'https://localhost:7154/BudgetPlan/EditPlanByPlanId'; 

  constructor(private http: HttpClient) {}

  updatePlan(planId: string, planData: any): Observable<string> {
    // Create headers if needed (e.g., Content-Type)
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    // Specify the response type as 'text'
    return this.http.post<string>(`${this.apiUrl}/${planId}`, planData, { headers, responseType: 'text' as 'json' });
  }
}
