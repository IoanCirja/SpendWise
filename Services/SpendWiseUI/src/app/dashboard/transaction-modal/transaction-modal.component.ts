import {Component, OnDestroy, OnInit} from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CurrentPlanService } from '../services/current-plan.service';
import {AccountService} from "../../auth/account.service";
import {Subscription} from "rxjs"; // Adjust path if necessary

@Component({
  selector: 'app-transaction-modal',
  templateUrl: './transaction-modal.component.html',
  styleUrls: ['./transaction-modal.component.scss']
})
export class TransactionModalComponent implements OnInit, OnDestroy {
  categories: string[] = [];
  selectedCategory: string = '';
  transactionValue: number | null = null;
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    public dialogRef: MatDialogRef<TransactionModalComponent>,
    private currentPlanService: CurrentPlanService,
    private accountService: AccountService

  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
        this.loadCategories(this.userId);
      }
    })
    this.subscriptions.push(subscription);
  }

  loadCategories(userId:string): void {
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

  onSave(): void {
    if (!this.selectedCategory || this.transactionValue === null) {
      console.error('Category and value are required.');
      return;
    }

    const transactionData = {
      category: this.selectedCategory,
      value: this.transactionValue,
      user_id: this.userId // Make sure this is set
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

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }
}
