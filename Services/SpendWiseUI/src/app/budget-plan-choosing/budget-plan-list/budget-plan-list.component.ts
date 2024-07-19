import { Component, OnInit } from '@angular/core';
import { BudgetPlans } from '../models/BudgetPlan';
import { DisplayPlanService } from '../services/display-plans.service';

@Component({
  selector: 'app-budget-plan-list',
  templateUrl: './budget-plan-list.component.html',
  styleUrls: ['./budget-plan-list.component.scss']
})
export class BudgetPlanListComponent implements OnInit {
  budgetPlans: BudgetPlans = [];
  filteredBudgetPlans: BudgetPlans = [];
  searchQuery: string = '';

  viewOptions: string[] = ['View All', 'Pages'];
  sortOptions: string[] = [
    'Sort by Name (A-Z)',
    'Sort by Name (Z-A)',
    'Sort by No. of Categories (Ascending)',
    'Sort by No. of Categories (Descending)',
  ];
  selectedSortOption: string = this.sortOptions[0];
  selectedViewOption: string = this.viewOptions[0];

  currentPage: number = 1;
  itemsPerPage: number = 2;
  totalPages: number = 1;

  constructor(private displayPlanService: DisplayPlanService) {}

  ngOnInit(): void {
    this.displayPlanService.getBudgetPlans().subscribe({
      next: (data) => {
        this.budgetPlans = data;
        this.filteredBudgetPlans = data; // Initialize filteredBudgetPlans
        console.log('Fetched data:', this.budgetPlans);
        this.sortBudgetPlans();
        this.updatePagination();
      },
      error: (err) => {
        console.error('Error fetching data:', err);
      }
    });
  }

  sortBudgetPlans(): void {
    switch (this.selectedSortOption) {
      case 'Sort by Name (A-Z)':
        this.filteredBudgetPlans.sort((a, b) => a.name.localeCompare(b.name));
        break;
      case 'Sort by Name (Z-A)':
        this.filteredBudgetPlans.sort((a, b) => b.name.localeCompare(a.name));
        break;
      case 'Sort by No. of Categories (Ascending)':
        this.filteredBudgetPlans.sort((a, b) => a.noCategory - b.noCategory);
        break;
      case 'Sort by No. of Categories (Descending)':
        this.filteredBudgetPlans.sort((a, b) => b.noCategory - a.noCategory);
        break;
    }
  }

  onSortChange(): void {
    this.sortBudgetPlans();
    this.applyFilter(); // Apply filter after sorting
  }

  applyFilter(): void {
    const filterValue = this.searchQuery.trim().toLowerCase();
    this.filteredBudgetPlans = this.budgetPlans.filter(plan => 
      plan.name.toLowerCase().includes(filterValue) || 
      plan.description.toLowerCase().includes(filterValue) || 
      plan.category.toLowerCase().includes(filterValue) || 
      plan.created_by.toLowerCase().includes(filterValue)
    );
    this.sortBudgetPlans(); // Ensure sorted order is maintained after filtering
    this.updatePagination(); // Update pagination after filtering
  }

  onViewChange(): void {
    this.currentPage = 1;
    this.updatePagination();
  }

  getDisplayedPlans(): BudgetPlans {
    if (this.selectedViewOption === 'Pages') {
      const startIndex = (this.currentPage - 1) * this.itemsPerPage;
      const endIndex = startIndex + this.itemsPerPage;
      return this.filteredBudgetPlans.slice(startIndex, endIndex);
    }
    return this.filteredBudgetPlans;
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredBudgetPlans.length / this.itemsPerPage);
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }
}
