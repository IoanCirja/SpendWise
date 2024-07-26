import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BudgetPlanModalComponent } from '../budget-plan-modal/budget-plan-modal.component';
import { EditPlanModalComponent } from '../edit-budget-plan-modal/edit-plan-modal.component';

@Component({
  selector: 'app-budget-plan-card',
  templateUrl: './budget-plan-card.component.html',
  styleUrls: ['./budget-plan-card.component.scss']
})
export class BudgetPlanCardComponent implements OnInit {
  @Input() plan_id!: string;
  @Input() name!: string;
  @Input() description!: string;
  @Input() noCategory!: number;
  @Input() category!: string;
  @Input() image!: string;
  @Input() creationDate!: string;
  @Input() created_by!: string;

  isAdmin: boolean = false;
  canEdit: boolean = false;

  constructor(public dialog: MatDialog) {}

  ngOnInit(): void {
    this.checkUserRoleAndOwnership();
  }

  checkUserRoleAndOwnership(): void {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      const user = JSON.parse(userJson);
      const userRole = user.role;
      const username = user.name; // Assuming the username is stored as 'name'
  
      // Check if the user is an admin
      this.isAdmin = userRole === 'admin';
      console.log('Created by:', this.created_by);
      console.log('Current user name:', username);
  
      // Check if the admin is the creator of the plan
      if (this.isAdmin && this.created_by === username) {
        this.canEdit = true;
      }
    } else {
      console.error('No user data found in local storage.');
    }
  }

  openDialog(): void {
    this.dialog.open(BudgetPlanModalComponent, {
      data: {
        plan_id: this.plan_id,
        name: this.name,
        description: this.description,
        noCategory: this.noCategory,
        categories: this.category.split(',').map(cat => ({ name: cat.trim(), value: 0 })),
        image: this.image,
        created_by: this.created_by,
        creationDate: this.creationDate // Pass creationDate here
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
          categories: this.category.split(','),
          creationDate: this.creationDate // Pass creationDate here
        }
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Handle result if necessary
      }
    });
  }
}
