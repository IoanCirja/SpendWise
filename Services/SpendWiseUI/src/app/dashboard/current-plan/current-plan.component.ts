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

  constructor(private currentPlanService: CurrentPlanService, private router: Router) { }

  ngOnInit(): void {
    const userId = '04A7CE09-3B90-4940-835A-1EFEA93370B7'; 
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


}
