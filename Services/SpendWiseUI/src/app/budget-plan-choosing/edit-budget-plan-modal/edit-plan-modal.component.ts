import { Component, Inject, EventEmitter, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditPlanService } from '../services/edit-budget-plan.service';
import { DeletePlanService } from '../services/delete-budget-plan.service';

@Component({
  selector: 'app-edit-plan-modal',
  templateUrl: './edit-plan-modal.component.html',
  styleUrls: ['./edit-plan-modal.component.scss']
})
export class EditPlanModalComponent {
  plan: any = {
    name: '',
    description: '',
    image: '',
    categories: []
  };

  @Output() planChanged = new EventEmitter<void>();

  constructor(
    public dialogRef: MatDialogRef<EditPlanModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { plan: any },
    private editPlanService: EditPlanService,
    private deletePlanService: DeletePlanService
  ) {
    this.plan = { ...data.plan };
  }

  addCategory(): void {
    this.plan.categories.push('');
  }

  removeCategory(index: number): void {
    if (this.plan.categories.length > 1) {
      this.plan.categories.splice(index, 1);
    } else {
      console.warn('At least one category is required.');
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    const updatedPlan = {
      name: this.plan.name,
      description: this.plan.description,
      noCategory: this.plan.categories.length,
      category: this.plan.categories.join(','),
      image: this.plan.image
    };
    this.editPlanService.updatePlan(this.data.plan.id, updatedPlan).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: err => {
        console.error('Error updating plan:', err);
      }
    });
  }

  onDelete(): void {
    this.deletePlanService.deletePlan(this.data.plan.id).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: err => {
        console.error('Error deleting plan:', err);
      }
    });
  }
}
