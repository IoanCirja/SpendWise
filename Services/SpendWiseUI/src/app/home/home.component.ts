import {ElementRef, Component, Input, OnInit} from '@angular/core';
import {DisplayPopularPlanService} from "./services/display-popular-plan.service";
import {BudgetPlanGetPopular} from "./models/BudgetPlanGetPopular";
import {Router} from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  mostPopularBudgetPlans: BudgetPlanGetPopular[] = [];
  duplicatedPlans: BudgetPlanGetPopular[] = [];
  @Input() image!: string;
  @Input() name!: string;

  constructor (
    public displayPopularPlanService: DisplayPopularPlanService,
    private el: ElementRef,
    private router: Router
  ){
    console.log("[HomeComponent] constructor]")
  }

  ngOnInit(): void {
    this.displayPopularPlanService.getPopularBudgetPlans().subscribe(
      response => {
        this.mostPopularBudgetPlans = response;
        this.duplicatedPlans = [...response, ...response];
        console.log('getPopularBudgetPlans successful', response);
      },
      error => {
        console.error('getPopularBudgetPlans failed', error);
      }
    );
    console.log("[HomeComponent] init]")
  }

  redirectToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}
