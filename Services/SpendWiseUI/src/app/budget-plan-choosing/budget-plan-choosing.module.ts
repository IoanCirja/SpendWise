import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BudgetPlanCardComponent } from './budget-plan-card/budget-plan-card.component';
import { BudgetPlanListComponent } from './budget-plan-list/budget-plan-list.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';
import { BudgetPlanChoosingRoutingModule } from './budget-plan-choosing-routing.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    BudgetPlanCardComponent,
    BudgetPlanListComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    HttpClientModule,
    BudgetPlanChoosingRoutingModule,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule
  ],
  exports: [
    BudgetPlanCardComponent,
    BudgetPlanListComponent
    
  ]
})
export class BudgetPlansModule { }