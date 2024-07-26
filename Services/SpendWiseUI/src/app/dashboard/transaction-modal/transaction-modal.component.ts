import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CurrentPlanService } from '../services/current-plan.service';
import { AccountService } from '../../auth/account.service';
import { TransactionService } from '../services/transaction-service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

import { CurrentPlanRefreshService } from '../services/current-plan-refresh-service';
@Component({
  selector: 'app-transaction-modal',
  templateUrl: './transaction-modal.component.html',
  styleUrls: ['./transaction-modal.component.scss']
})
export class TransactionModalComponent implements OnInit, OnDestroy {
  categories: string[] = [];
  selectedCategory: string = '';
  transactionValue: number | null = null;
  transactionName: string = '';
  userId: string | null = null;
  monthlyPlanId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    public dialogRef: MatDialogRef<TransactionModalComponent>,
    private currentPlanService: CurrentPlanService,
    private accountService: AccountService,
    private transactionService: TransactionService,
    private router: Router,
    private currentPlanRefreshService: CurrentPlanRefreshService // Inject CurrentPlanRefreshService
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
    });
    this.subscriptions.push(subscription);
  }

  loadCategories(userId: string): void {
    const subscription = this.currentPlanService.getCurrentPlan(userId).subscribe(
      data => {
        const currentPlan = data[0];
        if (currentPlan) {
          this.monthlyPlanId = currentPlan.monthlyPlan_id; 
          const categories = currentPlan.category.split(', ');
          this.categories = categories;
        }
      },
      error => {
        console.error('Error fetching categories', error);
      }
    );
    this.subscriptions.push(subscription);
  }

  onSave(): void {
    if (!this.selectedCategory || this.transactionValue === null || !this.transactionName) {
      console.error('Name, category, and value are required.');
      return;
    }

    const transactionData = {
      name: this.transactionName,
      monthlyPlan_id: this.monthlyPlanId,
      date: new Date().toISOString(),
      category: this.selectedCategory,
      amount: this.transactionValue
    };

    this.transactionService.saveTransaction(transactionData).subscribe(
      response => {
        console.log('Transaction saved successfully', response);
        this.dialogRef.close(true); // Close the modal and signal success

        // Notify CurrentPlanComponent to refresh
        this.currentPlanRefreshService.triggerRefresh();

        // Redirect to dashboard/current-plan
        this.router.navigate(['/dashboard/current-plan']);
      },
      error => {
        console.error('Error saving transaction', error);
      }
    );
  }

  onCancel(): void {
    this.dialogRef.close(); // Close the modal without saving
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
