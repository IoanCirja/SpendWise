import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BudgetPlans } from '../models/BudgetPlan';

@Injectable({
  providedIn: 'root'
})
export class DisplayPlanService {
  private endPoint = 'https://localhost:7154/BudgetPlan/GetPlans'

  constructor(private http: HttpClient) { }

  getBudgetPlans():Observable<BudgetPlans>{
    return this.http.get<BudgetPlans>(this.endPoint);
  }
}