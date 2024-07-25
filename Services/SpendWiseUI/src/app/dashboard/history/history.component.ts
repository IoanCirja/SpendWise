import { Component, OnDestroy, OnInit } from '@angular/core';
import { HistoryService } from '../services/history.service';
import { TransactionService } from '../services/transaction-service';
 import { Subscription } from 'rxjs';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { AccountService } from '../../auth/account.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit, OnDestroy {

  historyPlans: any[] = [];
  selectedPlan: MonthlyPlan | null = null;
  displayedTransactions: any[] = [];
  displayedColumns: string[] = ['category', 'name', 'amount', 'date'];
  categoriesWithDetails: any[] = [];
  selectedCategory: string = 'all';
  sortCriteria: string = 'dateAsc';
  isDetailedView = false;
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(
    private historyService: HistoryService,
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
        this.loadHistoryPlans(this.userId);
      }
    });
    this.subscriptions.push(subscription);
  }

  loadHistoryPlans(userId: string): void {
    this.historyService.getHistoryPlans(userId).subscribe(
      data => {
        this.historyPlans = data;
      },
      error => {
        console.error('Error fetching history plans', error);
      }
    );
  }

  onPlanSelect(plan: MonthlyPlan): void {
    this.selectedPlan = plan;
    this.isDetailedView = true;
    this.loadTransactions(plan.monthlyPlan_id);
  }

  loadTransactions(monthlyPlanId: string): void {
    this.transactionService.getAllTransactions(monthlyPlanId).subscribe(
      transactions => {
        this.displayedTransactions = transactions;
        this.extractCategoryDetails();
        this.filterTransactions();
      },
      error => {
        console.error('Error fetching transactions', error);
      }
    );
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
        transactions: []
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
    this.historyPlans = []; 
    this.loadHistoryPlans(this.userId!);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
