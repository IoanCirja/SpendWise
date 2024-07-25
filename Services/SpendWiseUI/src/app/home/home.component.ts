import {AfterViewInit,Renderer2,ElementRef, Component, Input, OnInit, Output} from '@angular/core';
import { MatCard } from '@angular/material/card';
import {DisplayPopularPlanService} from "./services/display-popular-plan.service";
import {BudgetPlanGetPopular} from "./models/BudgetPlanGetPopular";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, AfterViewInit {
  mostPopularBudgetPlans: BudgetPlanGetPopular[] = [];
  duplicatedPlans: BudgetPlanGetPopular[] = [];

  constructor (
    public displayPopularPlanService: DisplayPopularPlanService,
    private renderer: Renderer2,
    private el: ElementRef
  ){
    console.log("[HomeComponent] constructor]")
  }

  ngOnInit(): void {
    this.displayPopularPlanService.getPopularBudgetPlans().subscribe(
      response => {
        this.mostPopularBudgetPlans = response;
        this.duplicatedPlans = [...response, ...response];
        this.updateAnimation();
        console.log('getPopularBudgetPlans successful', response);
      },
      error => {
        console.error('getPopularBudgetPlans failed', error);
      }
    );
    console.log("[HomeComponent] init]")
  }

  getScrollWidth(): string {
    return `calc(-500px * ${this.mostPopularBudgetPlans.length})`;
  }

  ngAfterViewInit(): void {
    this.updateAnimation();
  }

  updateAnimation(): void {
    const scrollWidth = this.mostPopularBudgetPlans.length * 520;
    const keyframes = `@keyframes scroll {
      from {
        transform: translateX(0);
      }
      to {
        transform: translateX(-${scrollWidth}px);
      }
    }`;

    const styleSheet = this.renderer.createElement('style');
    styleSheet.type = 'text/css';
    styleSheet.innerHTML = keyframes;
    this.renderer.appendChild(this.el.nativeElement, styleSheet);
  }
}
