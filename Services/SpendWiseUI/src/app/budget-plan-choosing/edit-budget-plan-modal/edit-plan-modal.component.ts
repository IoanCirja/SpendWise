import { Component, Inject, EventEmitter, Output } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditPlanService } from '../services/edit-budget-plan.service';
import { DeletePlanService } from '../services/delete-budget-plan.service';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { DeleteConfirmationDialog } from '../delete-confirmation-dialog/delete-confirmation-dialog.component';

@Component({
  selector: 'app-edit-plan-modal',
  templateUrl: './edit-plan-modal.component.html',
  styleUrls: ['./edit-plan-modal.component.scss']
})
export class EditPlanModalComponent {
  faTrash = faTrash;

  plan: any = {
    name: '',
    description: '',
    image: '',
    categories: []
  };
  hasErrors: boolean = false;
  errorMessage: string | null = null;

  @Output() planChanged = new EventEmitter<void>();

  constructor(
    public dialog: MatDialog,
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

  validateForm(): boolean {
    if (!this.plan.name || this.plan.name.length < 3 ||
        !this.plan.description || this.plan.description.length < 5 ||
        !this.plan.image || this.plan.image.length < 5) {
      return false;
    }
    return true;
  }

  onClose(): void {
    this.dialogRef.close();
  }

  onSave(): void {
    this.errorMessage = null;

    if (!this.validateForm()) {
      this.errorMessage = 'All fields must be filled out and have at least the required number of characters.';
      return;
    }

    const updatedPlan = {
      name: this.plan.name,
      description: this.plan.description,
      noCategory: this.plan.categories.length,
      category: this.plan.categories.join(','),
      image: this.plan.image
    };

    this.editPlanService.updatePlan(this.data.plan.id, updatedPlan).subscribe({
      next: () => {
        this.planChanged.emit();
        this.dialogRef.close(true);
      },
      error: err => {
        this.errorMessage = 'Error updating plan: ' + err.message;
      }
    });
  }

  onDelete(): void {
    const confirmDialogRef = this.dialog.open(DeleteConfirmationDialog, {
      width: '617px',
      data: { message: 'Are you sure you want to delete this plan?' }
    });

    confirmDialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deletePlanService.deletePlan(this.data.plan.id).subscribe({
          next: () => {
            this.planChanged.emit();
            this.dialogRef.close(true);
          },
          error: err => {
            console.error('Error deleting plan:', err);
          }
        });
      }
    });
  }
}


