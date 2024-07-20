import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BudgetPlanService } from '../services/budget-plan-modal.service';

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
export class BudgetPlanModalComponent {

  dataHolder: DialogData;

  constructor(
    public dialogRef: MatDialogRef<BudgetPlanModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private budgetPlanService: BudgetPlanService
  ) {
    this.dataHolder = data;
  }

  savePlan(): void {
    const plan_id = this.dataHolder.plan_id;
    const user_id = "04A7CE09-3B90-4940-835A-1EFEA93370B7";

    const totalAmount = this.dataHolder.categories.reduce((acc, category) => acc + category.value, 0);

    const planData = {
      user_id,
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
}

