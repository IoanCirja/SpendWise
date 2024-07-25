import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HistoryService } from '../services/history.service';
import { TransactionService } from '../services/transaction-service';
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
  displayedTransactions: any[] = [];
  categoriesWithDetails: any[] = [];
  selectedCategory: string = 'all';
  sortCriteria: string = 'dateAsc';
  isDetailedView = false;
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    private historyService: HistoryService,
    private transactionService: TransactionService,
    private accountService: AccountService,
    private router: Router
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
        if (this.selectedPlan?.monthlyPlan_id) {
          await this.loadTransactions(this.selectedPlan.monthlyPlan_id);
        } else {
          console.error('Selected Plan or monthlyPlan_id is undefined');
        }
      } else {
        console.error('Plan returned from service is undefined');
      }
    } catch (error) {
      console.error('Error fetching plan details', error);
    }
  }

  async loadTransactions(monthlyPlanId: string): Promise<void> {
    try {
      this.displayedTransactions = await firstValueFrom(this.transactionService.getAllTransactions(monthlyPlanId));
      this.extractCategoryDetails();
      this.filterTransactions();
    } catch (error) {
      console.error('Error fetching transactions', error);
    }
  }

  extractCategoryDetails(): void {
    if (this.selectedPlan) {
      const categories = this.selectedPlan.category.split(', ').map(c => c.trim());
      const prices = this.selectedPlan.priceByCategory.split(', ').map(price => Number(price.trim()));
      const spends = this.selectedPlan.spentOfCategory.split(', ').map(spend => Number(spend.trim()));

      this.categoriesWithDetails = categories.map((category, index) => ({
        name: category,
        price: prices[index] || 0,
        spent: spends[index] || 0,
        transactions: []  // Populate this if needed
      }));
    }
  }

  filterTransactions(): void {
    if (this.selectedCategory === 'all') {
      this.displayedTransactions = this.displayedTransactions.map(transaction => ({
        ...transaction,
        category: this.categoriesWithDetails.find(category => category.name === transaction.category)?.name
      }));
    } else {
      const selectedCategory = this.categoriesWithDetails.find(category => category.name === this.selectedCategory);
      this.displayedTransactions = selectedCategory ? [...selectedCategory.transactions] : [];
    }
    this.sortTransactions();
  }

  sortTransactions(): void {
    this.displayedTransactions.sort((a, b) => {
      switch (this.sortCriteria) {
        case 'dateAsc': return new Date(a.date).getTime() - new Date(b.date).getTime();
        case 'dateDesc': return new Date(b.date).getTime() - new Date(a.date).getTime();
        case 'amountAsc': return a.amount - b.amount;
        case 'amountDesc': return b.amount - a.amount;
        case 'nameAsc': return a.name.localeCompare(b.name);
        case 'nameDesc': return b.name.localeCompare(a.name);
        default: return 0;
      }
    });
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

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
