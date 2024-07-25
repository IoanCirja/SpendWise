import {Component, OnDestroy, OnInit} from '@angular/core';
import { CurrentPlanService } from '../services/current-plan.service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { Router } from '@angular/router';
import {AccountService} from "../../auth/account.service";
import {Subscription} from "rxjs";

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
    })
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
      const categories = this.currentPlan.category.split(', ');
      const prices = this.currentPlan.priceByCategory.split(', ').map(Number);
      const spends = this.currentPlan.spentOfCategory.split(', ').map(Number);

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
      return 'primary'; // Blue
    } else if (percentage < 75) {
      return 'accent'; // Orange
    } else {
      return 'warn'; // Red
    }
  }

  cancelPlan(): void {
    if (this.currentPlan && this.currentPlan.monthlyPlan_id) {
      this.currentPlanService.cancelCurrentPlan(this.currentPlan.monthlyPlan_id).subscribe(
        response => {
          console.log('Plan canceled successfully', response);
          // Navigate to the same route to refresh the component
          this.router.navigate([this.router.url]); // This should reload the component
        },
        error => {
          console.error('Error canceling the plan', error);
        }
      );
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }
}
