import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentPlanService } from '../services/current-plan.service';
import { MonthlyPlan } from '../models/MonthlyPlan';

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit {

  categoriesWithDetails: { name: string, price: number, spent: number }[] = [];
  userId: string | null = null;

  constructor(
    private router: Router,
    private currentPlanService: CurrentPlanService
  ) { }

  ngOnInit(): void {
    this.userId = this.getUserIdFromLocalStorage();
    if (this.userId) {
      this.loadCurrentPlan();
    } else {
      console.error('User ID not found in local storage.');
    }
  }

  getUserIdFromLocalStorage(): string | null {
    const userString = localStorage.getItem('currentUser');
    if (userString) {
      const user = JSON.parse(userString);
      return user.id;
    }
    return null;
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
}
