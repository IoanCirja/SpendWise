import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CreateBudgetPlanService } from '../services/create-budget-plan-service';
import { Subscription } from "rxjs";
import { AccountService } from "../../auth/account.service";
import { faTrash } from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-create-budget-plan-modal',
  templateUrl: './create-budget-plan-modal.component.html',
  styleUrls: ['./create-budget-plan-modal.component.scss']
})
export class CreateBudgetPlanModalComponent implements OnDestroy {
  newPlan = {
    name: '',
    description: '',
    imagine: '',
    categories: [{ name: '' }]
  };
  userId: string | null = null;
  subscriptions: Subscription[] = [];
  errorMessage: string | null = null;

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
    this.errorMessage = null; 

    if (this.isFormInvalid()) {
      this.errorMessage = 'All fields must be filled out and have at least 3 characters.';
      return;
    }

    if (!this.userId) {
      this.errorMessage = 'User ID not found. Unable to create budget plan.';
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
        this.errorMessage = 'Error creating budget plan: ' + err.message;
      }
    });
  }

  isFormInvalid(): boolean {
    if (!this.newPlan.name || this.newPlan.name.length < 3 ||
        !this.newPlan.description || this.newPlan.description.length < 3 ||
        !this.newPlan.imagine || this.newPlan.imagine.length < 3) {
      return true;
    }

    for (const category of this.newPlan.categories) {
      if (!category.name || category.name.length < 3) {
        return true;
      }
    }

    return false;
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
      }
    });
    this.subscriptions.push(subscription);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }

  protected readonly faTrash = faTrash;
}
