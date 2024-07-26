import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { HistoryService } from '../services/history.service';
import { AccountService } from '../../auth/account.service';
import { HistoryPlan } from '../models/HistoryPlan';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { firstValueFrom } from 'rxjs';

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

  constructor(
    private historyService: HistoryService,
    private accountService: AccountService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadCurrentUser();
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
    } else {
      console.error('Selected plan does not have a valid monthlyPlan_id');
    }
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
      // Split the values and handle possible extra spaces
      const categories = this.selectedPlan.category.split(',').map(c => c.trim());
      const prices = this.selectedPlan.priceByCategory.split(',').map(price => parseFloat(price.trim()));
      const spends = this.selectedPlan.spentOfCategory.split(',').map(spend => parseFloat(spend.trim()));
  
      // Handle cases where prices or spends might be NaN or undefined
      const validPrices = prices.map(price => isNaN(price) ? 0 : price);
      const validSpends = spends.map(spend => isNaN(spend) ? 0 : spend);
  
      // Ensure arrays are aligned by length
      const maxLength = Math.max(categories.length, validPrices.length, validSpends.length);
  
      // Map categories to their details
      this.categoriesWithDetails = categories.map((category, index) => ({
        name: category,
        price: validPrices[index] !== undefined ? validPrices[index] : 0,
        spent: validSpends[index] !== undefined ? validSpends[index] : 0
      }));
  
      // Log final result for debugging
      console.log('Categories With Details:', this.categoriesWithDetails);
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

  goBack(): void {
    this.isDetailedView = false;
    this.selectedPlan = null;
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

// The method is unchanged
openCategoryDetails(categoryName: string): void {
  this.router.navigate(['dashboard/history-category-details'], {
    queryParams: { category: categoryName, monthlyPlanId: this.selectedPlan?.monthlyPlan_id }
  });
}


  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}