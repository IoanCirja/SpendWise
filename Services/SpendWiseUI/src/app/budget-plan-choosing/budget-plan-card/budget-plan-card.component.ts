import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BudgetPlanModalComponent } from '../budget-plan-modal/budget-plan-modal.component';
import { EditPlanModalComponent } from '../edit-budget-plan-modal/edit-plan-modal.component';
@Component({
  selector: 'app-budget-plan-card',
  templateUrl: './budget-plan-card.component.html',
  styleUrls: ['./budget-plan-card.component.scss']
})
export class BudgetPlanCardComponent {
  @Input() plan_id!: string;
  @Input() name!: string;
  @Input() description!: string;
  @Input() noCategory!: number;
  @Input() category!: string;
  @Input() image!: string;
  @Input() created_by!: string;

  constructor(public dialog: MatDialog) {}

  openDialog(): void {
    this.dialog.open(BudgetPlanModalComponent, {
      data: {
        plan_id: this.plan_id,
        name: this.name,
        description: this.description,
        noCategory: this.noCategory,
        categories: this.category.split(',').map(cat => ({ name: cat.trim(), value: 0 })),
        image: this.image,
        created_by: this.created_by
      },
      disableClose: true,
      autoFocus: false,
      width: '60vw',
    });
  }

  editPlan(): void {
    const dialogRef = this.dialog.open(EditPlanModalComponent, {
      width: '500px',
      data: {
        plan: {
          id: this.plan_id, 
          name: this.name,
          description: this.description,
          image: this.image,
          categories: this.category.split(',') 
        }
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
      }
    });
  }
}
