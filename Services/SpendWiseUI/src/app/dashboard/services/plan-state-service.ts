import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { MonthlyPlan } from '../models/MonthlyPlan';


@Injectable({
  providedIn: 'root'
})
export class PlanStateService {
  private _monthlyPlanId: string | null = null;

  setMonthlyPlanId(id: string): void {
    this._monthlyPlanId = id;
  }

  getMonthlyPlanId(): string | null {
    return this._monthlyPlanId;
  }

  clearMonthlyPlanId(): void {
    this._monthlyPlanId = null;
  }
}

