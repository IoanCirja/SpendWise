import {Component, Input, OnInit, Output} from '@angular/core';
import { MatCard } from '@angular/material/card';
import {DisplayPopularPlanService} from "./services/display-popular-plan.service";
import {BudgetPlanGetPopular} from "./models/BudgetPlanGetPopular";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  mostPopularBudgetPlans: BudgetPlanGetPopular[] = [];

  constructor (
    public displayPopularPlanService: DisplayPopularPlanService,
  ){
    console.log("[HomeComponent] constructor]")
  }

  ngOnInit(): void {
    this.displayPopularPlanService.getPopularBudgetPlans().subscribe(
      response => {
        this.mostPopularBudgetPlans = response;
        console.log('getPopularBudgetPlans successful', response);
      },
      error => {
        console.error('getPopularBudgetPlans failed', error);
      }
    );
    console.log("[HomeComponent] init]")
  }

}
