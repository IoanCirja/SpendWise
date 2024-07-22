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

  constructor(private router: Router, private currentPlanService: CurrentPlanService) { }

  ngOnInit(): void {
    const userId = 'E28D2431-5530-4C67-9CCB-152E7317DCE4'; 
    this.currentPlanService.getCurrentPlan(userId).subscribe(
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
