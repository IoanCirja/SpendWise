import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BudgetPlanListComponent } from './budget-plan-list/budget-plan-list.component';

const routes: Routes = [
  { path: '', component: BudgetPlanListComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BudgetPlanChoosingRoutingModule { }
