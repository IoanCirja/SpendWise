import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BudgetPlanService } from '../services/budget-plan-modal.service';
import {AccountService} from "../../auth/account.service";
import {Subscription} from "rxjs";

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

  constructor(
    public dialogRef: MatDialogRef<BudgetPlanModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private budgetPlanService: BudgetPlanService,
    private accountService: AccountService
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
      }
    })
    this.subscriptions.push(subscription);
  }

  savePlan(): void {
    if (!this.userId) {
      console.error('User ID not found. Unable to save budget plan.');
      return;
    }

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

    this.budgetPlanService.saveBudgetPlan(planData).subscribe(
      (response) => {
        console.log('Save successful:', response);
        this.dialogRef.close();
      },
      (error) => {
        console.error('Save failed:', error);
      }
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }
}
