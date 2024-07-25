import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { MonthlyPlan } from '../models/MonthlyPlan';

@Injectable({
  providedIn: 'root'
})
export class PlanStateService {
  private currentPlanSubject = new BehaviorSubject<MonthlyPlan | null>(null);
  currentPlan$: Observable<MonthlyPlan | null> = this.currentPlanSubject.asObservable();

  // Update the current plan
  updateCurrentPlan(plan: MonthlyPlan | null): void {
    this.currentPlanSubject.next(plan);
  }
}
