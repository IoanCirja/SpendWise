import { Component, OnDestroy, OnInit } from '@angular/core';
import { CurrentPlanService } from '../services/current-plan.service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { Router } from '@angular/router';
import { AccountService } from '../../auth/account.service';
import { Subscription } from 'rxjs';

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
    private accountService: AccountService
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser();
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

    const subscription = this.currentPlanService.getCurrentPlan(this.userId).subscribe(
      data => {
        this.currentPlan = data[0];
        if (this.currentPlan) {
          console.log('Current Plan:', this.currentPlan);
          this.extractCategoryDetails();
        }
      },
      error => {
        console.error('Error fetching current plan', error);
      }
    );
    this.subscriptions.push(subscription);
  }

  extractCategoryDetails(): void {
    if (this.currentPlan) {
      console.log('Category:', this.currentPlan.category);
      console.log('Price by Category:', this.currentPlan.priceByCategory);
      console.log('Spent of Category:', this.currentPlan.spentOfCategory);

      const categories = this.currentPlan.category.split(',').map(c => c.trim());
      const prices = this.currentPlan.priceByCategory.split(',').map(price => Number(price.trim()));
      const spends = this.currentPlan.spentOfCategory.split(',').map(spend => Number(spend.trim()));

      console.log('Categories:', categories);
      console.log('Prices:', prices);
      console.log('Spends:', spends);

      this.categoriesWithDetails = categories.map((category, index) => ({
        name: category,
        price: prices[index] || 0,
        spent: spends[index] || 0
      }));
    }
    console.log(this.categoriesWithDetails);
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

  cancelPlan(): void {
    if (this.currentPlan && this.currentPlan.monthlyPlan_id) {
      this.currentPlanService.cancelCurrentPlan(this.currentPlan.monthlyPlan_id).subscribe(
        response => {
          console.log('Plan canceled successfully', response);
          this.router.navigate([this.router.url]); 
        },
        error => {
          console.error('Error canceling the plan', error);
        }
      );
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
