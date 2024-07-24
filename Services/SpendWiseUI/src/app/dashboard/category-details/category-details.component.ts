import {Component, OnDestroy, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { CurrentPlanService } from '../services/current-plan.service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import {AccountService} from "../../auth/account.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit, OnDestroy {

  categoriesWithDetails: { name: string, price: number, spent: number }[] = [];
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private currentPlanService: CurrentPlanService,
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
        const currentPlan = data[0];
        if (currentPlan) {
          this.extractCategoryDetails(currentPlan);
        }
      },
      error => {
        console.error('Error fetching current plan', error);
      }
    );
  }

  extractCategoryDetails(plan: MonthlyPlan): void {
    const categories = plan.category.split(', ');
    const prices = plan.priceByCategory.split(', ').map(Number);
    const spends = plan.spentOfCategory.split(', ').map(Number);

    this.categoriesWithDetails = categories.map((category, index) => ({
      name: category,
      price: prices[index] || 0,
      spent: spends[index] || 0
    }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }
}
