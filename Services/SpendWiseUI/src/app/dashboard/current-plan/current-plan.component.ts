import { Component, OnInit } from '@angular/core';
import { CurrentPlanService } from '../services/current-plan.service';
import { MonthlyPlan } from '../models/MonthlyPlan';
import { Router } from '@angular/router';

@Component({
  selector: 'app-current-plan',
  templateUrl: './current-plan.component.html',
  styleUrls: ['./current-plan.component.scss']
})
export class CurrentPlanComponent implements OnInit {

  currentPlan: MonthlyPlan | null = null;
  categoriesWithDetails: { name: string, price: number, spent: number }[] = [];

  constructor(
    private currentPlanService: CurrentPlanService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCurrentPlan();
  }

  loadCurrentPlan(): void {
    const userId = 'E28D2431-5530-4C67-9CCB-152E7317DCE4'; 
    this.currentPlanService.getCurrentPlan(userId).subscribe(
      data => {
        this.currentPlan = data[0]; 
        if (this.currentPlan) {
          this.extractCategoryDetails();
        }
      },
      error => {
        console.error('Error fetching current plan', error);
      }
    );
  }

  extractCategoryDetails(): void {
    if (this.currentPlan) {
      const categories = this.currentPlan.category.split(', ');
      const prices = this.currentPlan.priceByCategory.split(', ').map(Number);
      const spends = this.currentPlan.spentOfCategory.split(', ').map(Number);

      this.categoriesWithDetails = categories.map((category, index) => ({
        name: category,
        price: prices[index] || 0,
        spent: spends[index] || 0
      }));
    }

    console.log(this.categoriesWithDetails);
  }

  cancelPlan(): void {
    if (this.currentPlan && this.currentPlan.monthlyPlan_id) {
      this.currentPlanService.cancelCurrentPlan(this.currentPlan.monthlyPlan_id).subscribe(
        response => {
          console.log('Plan canceled successfully', response);
          // Navigate to the same route to refresh the component
          this.router.navigate([this.router.url]); // This should reload the component
        },
        error => {
          console.error('Error canceling the plan', error);
        }
      );
    }
  }
}
