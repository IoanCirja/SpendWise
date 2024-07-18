import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BudgetPlanCardComponent } from './budget-plan-card/budget-plan-card.component';
import { BudgetPlanListComponent } from './budget-plan-list/budget-plan-list.component';
import { BudgetPlanModalComponent } from './budget-plan-modal/budget-plan-modal.component';

import { HttpClientModule } from '@angular/common/http';
import { BudgetPlanChoosingRoutingModule } from './budget-plan-choosing-routing.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import {MatDialogModule} from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';


@NgModule({
  declarations: [
    BudgetPlanCardComponent,
    BudgetPlanListComponent,
    BudgetPlanModalComponent
    
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
    MatSelectModule,
    FormsModule,
    HttpClientModule,
    BudgetPlanChoosingRoutingModule



    
  ],
  exports: [
    BudgetPlanCardComponent,
    BudgetPlanListComponent
    
  ],
})
export class BudgetPlansModule { }