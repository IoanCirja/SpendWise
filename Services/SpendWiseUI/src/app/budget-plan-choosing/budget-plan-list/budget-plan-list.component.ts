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

  constructor(private displayPlanService: DisplayPlanService) {}

  ngOnInit(): void {
    this.displayPlanService.getBudgetPlans().subscribe({
      next: (data) => {
        this.budgetPlans = data;
        this.filteredBudgetPlans = data; // Initialize filteredBudgetPlans
        console.log('Fetched data:', this.budgetPlans);
        this.sortBudgetPlans();
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
    this.applyFilter(); 
  }

  applyFilter(): void {
    const filterValue = this.searchQuery.trim().toLowerCase();
    this.filteredBudgetPlans = this.budgetPlans.filter(plan => 
      plan.name.toLowerCase().includes(filterValue) || 
      plan.description.toLowerCase().includes(filterValue) || 
      plan.category.toLowerCase().includes(filterValue) || 
      plan.created_by.toLowerCase().includes(filterValue)
    );
    this.sortBudgetPlans(); 
  }
}
