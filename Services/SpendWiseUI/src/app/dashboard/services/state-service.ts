import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MonthlyPlan } from '../models/MonthlyPlan';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private selectedPlanSubject = new BehaviorSubject<MonthlyPlan | null>(null);
  selectedPlan$ = this.selectedPlanSubject.asObservable();

  setSelectedPlan(plan: MonthlyPlan | null): void {
    this.selectedPlanSubject.next(plan);
  }
}
