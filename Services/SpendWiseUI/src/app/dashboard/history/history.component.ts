import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Subscription } from 'rxjs';
import { HistoryService } from '../services/history.service';
import { AccountService } from '../../auth/account.service';
import { HistoryPlan } from '../models/HistoryPlan';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { firstValueFrom } from 'rxjs';
import { StateService } from '../services/state-service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit, OnDestroy {
  historyPlans: HistoryPlan[] = [];
  selectedPlan: MonthlyPlan | null = null;
  categoriesWithDetails: { name: string, price: number, spent: number }[] = [];
  isDetailedView = false;
  userId: string | null = null;
  subscriptions: Subscription[] = [];
  averagePercentage: number = 0;

  constructor(
    private historyService: HistoryService,
    private accountService: AccountService,
    private router: Router,
    private stateService: StateService
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();

    // Handle navigation state to determine if detailed view should be shown
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        const navigationState = event.restoredState;
        if (navigationState && navigationState.returnTo === 'detailed-view' && navigationState.planId) {
          this.isDetailedView = true;
          this.loadPlanDetails(navigationState.planId);
        } else {
          this.isDetailedView = false;
        }
      }
    });
  }

  async loadCurrentUser(): Promise<void> {
    try {
      const subscription = this.accountService.currentUser$.subscribe(currentUser => {
        if (currentUser) {
          this.userId = currentUser.id;
          if (this.userId) {
            this.loadHistoryPlans(this.userId);
          } else {
            console.error('User ID is null or undefined');
          }
        }
      });
      this.subscriptions.push(subscription);
    } catch (error) {
      console.error('Error loading current user:', error);
    }
  }

  async loadHistoryPlans(userId: string): Promise<void> {
    try {
      this.historyPlans = await firstValueFrom(this.historyService.getHistoryPlans(userId));
    } catch (error) {
      console.error('Error fetching history plans', error);
    }
  }

  async onPlanSelect(plan: HistoryPlan): Promise<void> {
    this.isDetailedView = true;
    if (plan.monthlyPlan_id) {
      await this.loadPlanDetails(plan.monthlyPlan_id);
      this.stateService.setSelectedPlan(this.selectedPlan);
    } else {
      console.error('Selected plan does not have a valid monthlyPlan_id');
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

  getRoundedPercentage(spent: number, price: number): number {
    if (price === 0) {
      return 0;
    }
    return Math.round((spent / price) * 100);
  }

  async loadPlanDetails(monthlyPlanId: string): Promise<void> {
    if (!monthlyPlanId) {
      console.error('Provided monthlyPlanId is invalid or undefined.');
      return;
    }

    try {
      const plan = await firstValueFrom(this.historyService.getPlanFromHistory(monthlyPlanId));
      if (plan) {
        this.selectedPlan = plan[0];
        this.extractCategoryDetails();
      } else {
        console.error('Plan returned from service is undefined');
      }
    } catch (error) {
      console.error('Error fetching plan details', error);
    }
  }

  extractCategoryDetails(): void {
    if (this.selectedPlan) {
      const categories = this.selectedPlan.category.split(',').map(c => c.trim());
      const prices = this.selectedPlan.priceByCategory.split(',').map(price => parseFloat(price.trim()));
      const spends = this.selectedPlan.spentOfCategory.split(',').map(spend => parseFloat(spend.trim()));
  
      const validPrices = prices.map(price => isNaN(price) ? 0 : price);
      const validSpends = spends.map(spend => isNaN(spend) ? 0 : spend);
  
      const maxLength = Math.max(categories.length, validPrices.length, validSpends.length);
  
      this.categoriesWithDetails = categories.map((category, index) => ({
        name: category,
        price: validPrices[index] !== undefined ? validPrices[index] : 0,
        spent: validSpends[index] !== undefined ? validSpends[index] : 0
      }));
  
      this.calculateAveragePercentage();
    }
  }

  goBack(): void {
    this.isDetailedView = false;
    this.selectedPlan = null;
    this.router.navigate(['dashboard/history'], {
      state: {
        returnTo: 'initial-view'
      }
    });
  }

  goToDetails(): void {
    if (this.selectedPlan?.monthlyPlan_id) {
      this.router.navigate(['dashboard/history-category-details'], {
        queryParams: { monthlyPlanId: this.selectedPlan.monthlyPlan_id }
      });
    } else {
      console.error('Selected Plan or monthlyPlan_id is undefined');
    }
  }

  openCategoryDetails(categoryName: string): void {
    this.router.navigate(['dashboard/history-category-details'], {
      queryParams: { category: categoryName, monthlyPlanId: this.selectedPlan?.monthlyPlan_id }
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
