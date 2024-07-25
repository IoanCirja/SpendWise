import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentPlanService } from '../services/current-plan.service';
import { TransactionService } from '../services/transaction-service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { AccountService } from '../../auth/account.service';
import { Subscription } from 'rxjs';

interface Transaction {
  name: string;
  amount: number;
  date: string; 
  category?: string; 
}

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit, OnDestroy {

  categoriesWithDetails: { name: string, price: number, spent: number, transactions: Transaction[] }[] = [];
  displayedTransactions: Transaction[] = [];
  displayedColumns: string[] = ['category', 'name', 'amount', 'date'];
  selectedCategory: string = 'all';
  sortCriteria: string = 'dateAsc';
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private currentPlanService: CurrentPlanService,
    private transactionService: TransactionService,
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
      } else {
        console.error('No user found');
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
        const currentPlan = data[0];
        if (currentPlan) {
          this.extractCategoryDetails(currentPlan);
          this.loadTransactions(currentPlan.monthlyPlan_id);
        }
      },
      error => {
        console.error('Error fetching current plan', error);
      }
    );
  }

  extractCategoryDetails(plan: MonthlyPlan): void {
    const categories = plan.category.split(', ').map(c => c.trim());
    const prices = plan.priceByCategory.split(', ').map(price => Number(price.trim()));
    const spends = plan.spentOfCategory.split(', ').map(spend => Number(spend.trim()));

    this.categoriesWithDetails = categories.map((category, index) => ({
      name: category,
      price: prices[index] || 0,
      spent: spends[index] || 0,
      transactions: []
    }));
  }

  loadTransactions(monthlyPlanId: string): void {
    this.transactionService.getAllTransactions(monthlyPlanId).subscribe(
      transactions => {
        this.integrateTransactions(transactions);
      },
      error => {
        console.error('Error fetching transactions', error);
      }
    );
  }

  integrateTransactions(transactions: any[]): void {
    this.categoriesWithDetails.forEach(category => {
      category.transactions = transactions.filter(transaction => transaction.category === category.name);
    });
    this.filterTransactions();
  }

  filterTransactions(): void {
    if (this.selectedCategory === 'all') {
      this.displayedTransactions = this.categoriesWithDetails.flatMap(category =>
        category.transactions.map(transaction => ({ ...transaction, category: category.name }))
      );
      this.displayedColumns = ['category', 'name', 'amount', 'date'];
    } else {
      const selectedCategory = this.categoriesWithDetails.find(category => category.name === this.selectedCategory);
      this.displayedTransactions = selectedCategory ? [...selectedCategory.transactions] : [];
      this.displayedColumns = ['name', 'amount', 'date'];
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

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
