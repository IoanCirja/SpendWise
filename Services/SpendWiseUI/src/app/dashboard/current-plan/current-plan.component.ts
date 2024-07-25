import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';

import { CurrentPlanService } from '../services/current-plan.service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { AccountService } from '../../auth/account.service';
import { PlanStateService } from '../services/plan-state-service';
import { ConfirmCancelDialogComponent } from '../cancel-plan-confirmation-modal/cancel-plan-confirmation-modal.component';

@Component({
  selector: 'app-current-plan',
  templateUrl: './current-plan.component.html',
  styleUrls: ['./current-plan.component.scss']
})
export class CurrentPlanComponent implements OnInit, OnDestroy {
  currentPlan: MonthlyPlan | null = null;
  categoriesWithDetails: { name: string, price: number, spent: number }[] = [];
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    private currentPlanService: CurrentPlanService,
    private router: Router,
    private accountService: AccountService,
    private planStateService: PlanStateService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser();

    this.subscriptions.push(
      this.planStateService.currentPlan$.subscribe(plan => {
        this.currentPlan = plan;
        if (this.currentPlan) {
          this.extractCategoryDetails();
        }
      })
    );
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

  loadCurrentPlan(): void {
    if (!this.userId) {
      console.error('User ID is required to load the current plan.');
      return;
    }

    this.currentPlanService.getCurrentPlan(this.userId).subscribe(
      data => {
        this.currentPlan = data[0];
        if (this.currentPlan) {
          this.extractCategoryDetails();
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

  getCircleColor(percentage: number): string {
    if (percentage < 50) {
      return 'primary'; 
    } else if (percentage < 75) {
      return 'accent'; 
    } else {
      return 'warn'; 
    }
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
