import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BudgetPlanService } from '../services/budget-plan-modal.service';
import { AccountService } from '../../auth/account.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CurrentPlanService } from 'src/app/dashboard/services/current-plan.service';

export interface DialogData {
  plan_id: string;
  name: string;
  description: string;
  noCategory: number;
  categories: { name: string, value: number }[];
  image: string;
  created_by: string;
}

@Component({
  selector: 'app-budget-plan-modal',
  templateUrl: './budget-plan-modal.component.html',
  styleUrls: ['./budget-plan-modal.component.scss']
})
export class BudgetPlanModalComponent implements OnInit, OnDestroy {

  dataHolder: DialogData;
  userId: string | null = null;
  subscriptions: Subscription[] = [];
  currentPlanExists: boolean = false;
  errorMessage: string | null = null;

  constructor(
    public dialogRef: MatDialogRef<BudgetPlanModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private budgetPlanService: BudgetPlanService,
    private accountService: AccountService,
    private router: Router,
    private currentPlanService: CurrentPlanService
  ) {
    this.dataHolder = data;
  }

  ngOnInit(): void {
    this.loadCurrentUser();
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
        this.checkCurrentPlan();
      }
    });
    this.subscriptions.push(subscription);
  }

  checkCurrentPlan(): void {
    if (!this.userId) {
      console.error('User ID is required to check the current plan.');
      return;
    }

    const subscription = this.currentPlanService.getCurrentPlan(this.userId).subscribe(
      data => {
        this.currentPlanExists = !!data.length;
      },
      error => {
        console.error('Error checking current plan', error);
      }
    );
    this.subscriptions.push(subscription);
  }

  savePlan(): void {
    if (!this.userId) {
      this.dialogRef.close();
      this.router.navigate(['/auth/login']);
      return;
    }

    // Validate input
    const hasNegativeValues = this.dataHolder.categories.some(cat => cat.value <= 0);
    if (hasNegativeValues) {
      this.errorMessage = 'Only positive values are allowed';
      return;
    }
    const smallValues = this.dataHolder.categories.some(cat => cat.value <= 30);
    if (smallValues) {
      this.errorMessage = 'Each category needs to be funded with at least 30€';
      return;
    }

    this.errorMessage = null; // Clear any previous error message

    const plan_id = this.dataHolder.plan_id;
    const totalAmount = this.dataHolder.categories.reduce((acc, category) => acc + category.value, 0);
    const planData = {
      user_id: this.userId,
      plan_id,
      date: new Date().toISOString(),
      totalAmount,
      amountSpent: 0,
      priceByCategory: this.dataHolder.categories.map(cat => cat.value).join(','),
      spentOfCategory: this.dataHolder.categories.map(() => 0).join(',')
    };

    console.log('Saving plan data:', planData);

    this.budgetPlanService.saveBudgetPlan(planData).subscribe(
      (response) => {
        console.log('Save successful:', response);
        this.dialogRef.close();
        this.router.navigate(['/dashboard/current-plan']);
      },
      (error) => {
        console.error('Save failed:', error);
      }
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  goToCurrentPlan(): void {
    this.router.navigate(['/dashboard']);
    this.dialogRef.close();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
