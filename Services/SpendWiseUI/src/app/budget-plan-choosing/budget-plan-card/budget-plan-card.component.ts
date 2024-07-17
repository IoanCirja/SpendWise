import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-budget-plan-card',
  templateUrl: './budget-plan-card.component.html',
  styleUrls: ['./budget-plan-card.component.scss']
})
export class BudgetPlanCardComponent {
  @Input() plan_id!: string;
  @Input() name!: string;
  @Input() description!: string;
  @Input() noCategory!: number;
  @Input() category!: string;
  @Input() image!: string;
  @Input() created_by!: string;



}