import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {BudgetPlans} from '../models/BudgetPlanGetPopular';

@Injectable({
  providedIn: 'root'
})
export class DisplayPopularPlanService {
  private endPoint = "https://localhost:7154/BudgetPlan/GetPopularFivePlans"

  constructor(private http: HttpClient){}

  getPopularBudgetPlans():Observable<BudgetPlans>{
    return this.http.get<BudgetPlans>(this.endPoint);
  }

}
