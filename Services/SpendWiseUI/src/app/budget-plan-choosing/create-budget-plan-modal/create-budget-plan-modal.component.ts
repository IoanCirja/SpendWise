import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CreateBudgetPlanService } from '../services/create-budget-plan-service';

@Component({
  selector: 'app-create-budget-plan-modal',
  templateUrl: './create-budget-plan-modal.component.html',
  styleUrls: ['./create-budget-plan-modal.component.scss']
})
export class CreateBudgetPlanModalComponent {
  newPlan = {
    name: '',
    description: '',
    imagine: '', // Image URL input
    categories: [{ name: '' }] // Initialize with one empty category
  };

  constructor(
    public dialogRef: MatDialogRef<CreateBudgetPlanModalComponent>,
    private createBudgetPlanService: CreateBudgetPlanService
  ) {}

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
    // Create plain object
    const planData = {
      name: this.newPlan.name,
      description: this.newPlan.description,
      noCategory: this.newPlan.categories.length,
      category: this.newPlan.categories.map(cat => cat.name).join(','),
      imagine: this.newPlan.imagine,
      user_id: 'E28D2431-5530-4C67-9CCB-152E7317DCE4' // Replace with actual user ID
    };

    this.createBudgetPlanService.createBudgetPlan(planData).subscribe({
      next: () => {
        this.dialogRef.close(true); // Signal that the plan was saved successfully
      },
      error: err => {
        console.error('Error creating budget plan:', err);
      }
    });
  }
}
