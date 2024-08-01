import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { MonthlyPlan } from '../models/MonthlyPlan';

@Injectable({
  providedIn: 'root'
})
export class DashboardButtonService {
  private currentPlanSubject = new BehaviorSubject<MonthlyPlan | null>(null);
  currentPlan$ = this.currentPlanSubject.asObservable();

  setCurrentPlan(plan: MonthlyPlan | null): void {
    this.currentPlanSubject.next(plan);
  }

  hasCurrentPlan(): Observable<boolean> {
    return this.currentPlan$.pipe(map(plan => !!plan));
  }
}
