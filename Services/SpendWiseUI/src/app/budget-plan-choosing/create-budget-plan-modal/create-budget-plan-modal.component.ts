import {Component, OnDestroy} from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CreateBudgetPlanService } from '../services/create-budget-plan-service';
import {Subscription} from "rxjs";
import {AccountService} from "../../auth/account.service";

@Component({
  selector: 'app-create-budget-plan-modal',
  templateUrl: './create-budget-plan-modal.component.html',
  styleUrls: ['./create-budget-plan-modal.component.scss']
})
export class CreateBudgetPlanModalComponent implements OnDestroy{
  newPlan = {
    name: '',
    description: '',
    imagine: '', 
    categories: [{ name: '' }] 
  };
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    public dialogRef: MatDialogRef<CreateBudgetPlanModalComponent>,
    private createBudgetPlanService: CreateBudgetPlanService,
    private accountService: AccountService
  ) {
    this.loadCurrentUser();
  }

  addCategory(): void {
    this.newPlan.categories.push({ name: '' });
  }

  removeCategory(index: number): void {
    if (this.newPlan.categories.length > 1) {
      this.newPlan.categories.splice(index, 1);
    } else {
      console.warn('At least one category is required.');
    }
  }

  trackByIndex(index: number, item: any): number {
    return index;
  }

  onClose(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    if (!this.userId) {
      console.error('User ID not found. Unable to create budget plan.');
      return;
    }

    const planData = {
      name: this.newPlan.name,
      description: this.newPlan.description,
      noCategory: this.newPlan.categories.length,
      category: this.newPlan.categories.map(cat => cat.name).join(','),
      imagine: this.newPlan.imagine,
      user_id: this.userId
    };

    this.createBudgetPlanService.createBudgetPlan(planData).subscribe({
      next: () => {
        this.dialogRef.close(true); 
      },
      error: err => {
        console.error('Error creating budget plan:', err);
      }
    });
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
      }
    })
    this.subscriptions.push(subscription);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }
}
