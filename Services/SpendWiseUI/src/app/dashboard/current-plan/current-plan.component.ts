import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';

import { DashboardButtonService } from '../services/dashboard-button-service';
import { CurrentPlanService } from '../services/current-plan.service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { AccountService } from '../../auth/account.service';
import { ConfirmCancelDialogComponent } from '../cancel-plan-confirmation-modal/cancel-plan-confirmation-modal.component';
import { CurrentPlanRefreshService } from '../services/current-plan-refresh-service';
import { TransactionModalComponent } from '../transaction-modal/transaction-modal.component';

@Component({
  selector: 'app-current-plan',
  templateUrl: './current-plan.component.html',
  styleUrls: ['./current-plan.component.scss']
})
export class CurrentPlanComponent implements OnInit, OnDestroy {
  currentPlan: MonthlyPlan | null = null;
  categoriesWithDetails: { name: string, price: number, spent: number }[] = [];
  averagePercentage: number = 0; // Added property
  totalSpent: number = 0; // Added property
  userId: string | null = null;
  subscriptions: Subscription[] = [];
  hasCurrentPlan$: Observable<boolean> | undefined;

  constructor(
    private dashboardButtonService: DashboardButtonService,
    private currentPlanService: CurrentPlanService,
    private router: Router,
    private accountService: AccountService,
    private dialog: MatDialog,
    private currentPlanRefreshService: CurrentPlanRefreshService
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
    this.hasCurrentPlan$ = this.dashboardButtonService.hasCurrentPlan();

    // Subscribe to refresh notifications
    const refreshSubscription = this.currentPlanRefreshService.refresh$.subscribe(() => {
      this.loadCurrentPlan(); // Reload current plan data
    });
    this.subscriptions.push(refreshSubscription);
  }

  goToDetails(): void {
    this.router.navigate(['dashboard/category-details']);
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
        this.loadCurrentPlan();
      }
    });
    this.subscriptions.push(subscription);
  }

  openTransactionModal(): void {
    this.dialog.open(TransactionModalComponent, {
      width: '300px',
      data: {}
    });
  }

  loadCurrentPlan(): void {
    if (!this.userId) {
      console.error('User ID is required to load the current plan.');
      return;
    }

    this.currentPlanService.getCurrentPlan(this.userId).subscribe(
      data => {
        this.currentPlan = data[0];
        this.dashboardButtonService.setCurrentPlan(this.currentPlan); // Update the shared service
        if (this.currentPlan) {
          this.extractCategoryDetails();
          this.calculateAveragePercentage(); // Calculate average percentage
          this.calculateTotalSpent(); // Calculate total spent
        }
      },
      error => {
        console.error('Error fetching current plan', error);
      }
    );
  }

  extractCategoryDetails(): void {
    if (this.currentPlan) {
      const categories = this.currentPlan.category.split(',').map(c => c.trim());
      const prices = this.currentPlan.priceByCategory.split(',').map(price => Number(price.trim()));
      const spends = this.currentPlan.spentOfCategory.split(',').map(spend => Number(spend.trim()));

      this.categoriesWithDetails = categories.map((category, index) => ({
        name: category,
        price: prices[index] || 0,
        spent: spends[index] || 0
      }));
    }
  }

  calculateAveragePercentage(): void {
    if (this.categoriesWithDetails.length === 0) {
      this.averagePercentage = 0;
      return;
    }

    const totalSpent = this.categoriesWithDetails.reduce((acc, category) => acc + category.spent, 0);
    const totalPrice = this.categoriesWithDetails.reduce((acc, category) => acc + category.price, 0);

    if (totalPrice === 0) {
      this.averagePercentage = 0;
    } else {
      this.averagePercentage = Math.round((totalSpent / totalPrice) * 100);
    }
  }

  calculateTotalSpent(): void {
    this.totalSpent = this.categoriesWithDetails.reduce((acc, category) => acc + category.spent, 0);
  }

  getRoundedPercentage(spent: number, price: number): number {
    return price === 0 ? 0 : Math.round((spent / price) * 100);
  }

  openCancelDialog(): void {
    const dialogRef = this.dialog.open(ConfirmCancelDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.cancelPlan();
      }
    });
  }

  cancelPlan(): void {
    if (this.currentPlan && this.currentPlan.monthlyPlan_id) {
      this.currentPlanService.cancelCurrentPlan(this.currentPlan.monthlyPlan_id).subscribe(
        response => {
          console.log('Plan canceled successfully', response);
          this.loadCurrentPlan(); // Refresh the current plan
        },
        error => {
          console.error('Error canceling the plan', error);
        }
      );
    }
  }

  openCategoryDetails(categoryName: string): void {
    this.router.navigate(['dashboard/category-details'], {
      queryParams: { category: categoryName }
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
