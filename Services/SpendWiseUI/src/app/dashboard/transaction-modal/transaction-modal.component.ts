import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CurrentPlanService } from '../services/current-plan.service'; // Adjust path if necessary

@Component({
  selector: 'app-transaction-modal',
  templateUrl: './transaction-modal.component.html',
  styleUrls: ['./transaction-modal.component.scss']
})
export class TransactionModalComponent implements OnInit {
  categories: string[] = [];
  selectedCategory: string = '';
  transactionValue: number | null = null;

  constructor(
    public dialogRef: MatDialogRef<TransactionModalComponent>,
    private currentPlanService: CurrentPlanService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    const userId = this.getUserIdFromLocalStorage();
    if (userId) {
      this.currentPlanService.getCurrentPlan(userId).subscribe(
        data => {
          const currentPlan = data[0];
          if (currentPlan) {
            const categories = currentPlan.category.split(', ');
            this.categories = categories;
          }
        },
        error => {
          console.error('Error fetching categories', error);
        }
      );
    }
  }

  getUserIdFromLocalStorage(): string | null {
    const userString = localStorage.getItem('currentUser');
    if (userString) {
      const user = JSON.parse(userString);
      return user.id;
    }
    return null;
  }

  onSave(): void {
    if (!this.selectedCategory || this.transactionValue === null) {
      console.error('Category and value are required.');
      return;
    }

    const transactionData = {
      category: this.selectedCategory,
      value: this.transactionValue,
      user_id: this.getUserIdFromLocalStorage() // Make sure this is set
    };

    // Call your service to save the transaction
    // this.transactionService.saveTransaction(transactionData).subscribe(
    //   response => {
    //     console.log('Transaction saved successfully', response);
    //     this.dialogRef.close(true); // Close the modal and signal success
    //   },
    //   error => {
    //     console.error('Error saving transaction', error);
    //   }
    // );
  }

  onCancel(): void {
    this.dialogRef.close(); // Close the modal without saving
  }
}
