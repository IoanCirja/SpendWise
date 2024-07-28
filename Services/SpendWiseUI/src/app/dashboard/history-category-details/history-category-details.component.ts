import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import { HistoryService } from '../services/history.service';
import { TransactionService } from '../services/transaction-service';
import { StateService } from '../services/state-service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { firstValueFrom } from 'rxjs';

interface Transaction {
  name: string;
  amount: number;
  date: string;
  category?: string;
}

@Component({
  selector: 'app-history-category-details',
  templateUrl: './history-category-details.component.html',
  styleUrls: ['./history-category-details.component.scss']
})
export class HistoryCategoryDetailsComponent implements OnInit {
  monthlyPlanId: string | null = null;
  selectedPlan: MonthlyPlan | null = null;
  displayedTransactions: Transaction[] = [];
  categoriesWithDetails: { name: string, price: number, spent: number, transactions: Transaction[] }[] = [];
  selectedCategory: string = 'all';
  selectedCategoryDetails: { name: string, price: number, spent: number, transactions: Transaction[] } | undefined;
  sortCriteria: string = 'dateAsc';

  totalSpent: number = 0;
  totalPrice: number = 0;
  averagePercentage: number = 0;

  constructor(
    private route: ActivatedRoute,
    private historyService: HistoryService,
    private transactionService: TransactionService,
    private router: Router,
    private stateService: StateService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.monthlyPlanId = params['monthlyPlanId'];
      this.selectedCategory = params['category'] || 'all';

      if (this.monthlyPlanId) {
        this.loadPlanDetails(this.monthlyPlanId);
      } else {
        console.error('monthlyPlanId is not available');
      }
    });
  }

  async loadPlanDetails(monthlyPlanId: string): Promise<void> {
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
      const transactions = await firstValueFrom(this.transactionService.getAllTransactions(monthlyPlanId));
      this.integrateTransactions(transactions);
    } catch (error) {
      console.error('Error fetching transactions', error);
    }
  }

  integrateTransactions(transactions: any[]): void {
    this.categoriesWithDetails = this.extractCategoryDetails();
    this.categoriesWithDetails.forEach(category => {
      category.transactions = transactions.filter(transaction => transaction.category === category.name);
    });
    this.filterTransactions();
  }

  extractCategoryDetails(): { name: string, price: number, spent: number, transactions: Transaction[] }[] {
    if (this.selectedPlan) {
      const categories = this.selectedPlan.category.split(',').map(c => c.trim());
      const prices = this.selectedPlan.priceByCategory.split(',').map(price => parseFloat(price.trim()));
      const spends = this.selectedPlan.spentOfCategory.split(',').map(spend => parseFloat(spend.trim()));

      return categories.map((category, index) => ({
        name: category,
        price: prices[index] || 0,
        spent: spends[index] || 0,
        transactions: []
      }));
    }
    return [];
  }

  filterTransactions(): void {
    if (this.selectedCategory === 'all') {
      this.displayedTransactions = this.categoriesWithDetails.flatMap(category =>
        category.transactions.map(transaction => ({ ...transaction, category: category.name }))
      );
      this.selectedCategoryDetails = undefined;
      this.calculateTotalAndAverage();
    } else {
      const selectedCategory = this.categoriesWithDetails.find(category => category.name === this.selectedCategory);
      this.displayedTransactions = selectedCategory ? [...selectedCategory.transactions] : [];
      this.selectedCategoryDetails = selectedCategory;
      this.totalSpent = this.totalPrice = this.averagePercentage = 0; // Reset values
    }
    this.sortTransactions();
  }

  calculateTotalAndAverage(): void {
    this.totalSpent = this.categoriesWithDetails.reduce((total, category) => total + category.spent, 0);
    this.totalPrice = this.categoriesWithDetails.reduce((total, category) => total + category.price, 0);

    if (this.totalPrice > 0) {
      this.averagePercentage = Math.round((this.totalSpent / this.totalPrice) * 100);
    } else {
      this.averagePercentage = 0;
    }
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

  getRoundedPercentage(spent: number, price: number): number {
    if (price === 0) {
      return 0;
    }
    return Math.round((spent / price) * 100);
  }

  goBack(): void {
    if (this.selectedPlan) {
      const navigationExtras: NavigationExtras = {
        state: {
          returnTo: 'detailed-view',
          planId: this.selectedPlan.monthlyPlan_id
        }
      };
      this.router.navigate(['dashboard/history'], navigationExtras);
    } else {
      console.error('Selected Plan or monthlyPlan_id is undefined');
    }
  }
}
