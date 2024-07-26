import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BudgetPlans } from '../models/BudgetPlan';
import { DisplayPlanService } from '../services/display-plans.service';
import { CreateBudgetPlanModalComponent } from '../create-budget-plan-modal/create-budget-plan-modal.component';

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
    'Sort by Creation Date (Newest First)',  
    'Sort by Creation Date (Oldest First)'   
  ];
  selectedSortOption: string = 'Sort by Creation Date (Newest First)'; 
  selectedViewOption: string = this.viewOptions[0];

  currentPage: number = 1;
  itemsPerPage: number = 2;
  totalPages: number = 1;
  
  isAdmin: boolean = false;
  showMyPlansOnly: boolean = false; // Property to control the checkbox state
  
  constructor(
    private displayPlanService: DisplayPlanService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.checkUserRole();
    this.displayPlanService.getBudgetPlans().subscribe({
      next: (data) => {
        this.budgetPlans = data;
        this.filteredBudgetPlans = data;
        this.sortBudgetPlans();
        this.updatePagination();
      },
      error: (err) => {
        console.error('Error fetching data:', err);
      }
    });
  }

  checkUserRole(): void {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.isAdmin = user.role === 'admin';
    } else {
      console.error('No user data found in local storage.');
    }
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
      case 'Sort by Creation Date (Newest First)':
        this.filteredBudgetPlans.sort((a, b) => new Date(b.creationDate).getTime() - new Date(a.creationDate).getTime());
        break;
      case 'Sort by Creation Date (Oldest First)':
        this.filteredBudgetPlans.sort((a, b) => new Date(a.creationDate).getTime() - new Date(b.creationDate).getTime());
        break;
    }
  }

  onSortChange(): void {
    this.sortBudgetPlans();
    this.applyFilter();
  }

  applyFilter(): void {
    const filterValue = this.searchQuery.trim().toLowerCase();
    const user = JSON.parse(localStorage.getItem('currentUser') || '{}');
    const username = user.name || '';

    this.filteredBudgetPlans = this.budgetPlans.filter(plan => {
      let matchesSearch = plan.name.toLowerCase().includes(filterValue) || 
                          plan.description.toLowerCase().includes(filterValue) || 
                          plan.category.toLowerCase().includes(filterValue) || 
                          plan.created_by.toLowerCase().includes(filterValue);

      if (this.isAdmin && this.showMyPlansOnly) {
        matchesSearch = matchesSearch && plan.created_by === username;
      }

      return matchesSearch;
    });

    this.sortBudgetPlans();
    this.updatePagination();
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

  openCreateDialog(): void {
    if (this.isAdmin) {
      const dialogRef = this.dialog.open(CreateBudgetPlanModalComponent, {
        width: '500px',
        disableClose: true
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.displayPlanService.getBudgetPlans().subscribe(data => {
            this.budgetPlans = data;
            this.filteredBudgetPlans = data;
            this.sortBudgetPlans();
            this.updatePagination();
          });
        }
      });
    } else {
      alert('You do not have permission to create a budget plan.');
    }
  }

  toggleShowMyPlans(): void {
    this.showMyPlansOnly = !this.showMyPlansOnly;
    this.applyFilter();
  }
}
