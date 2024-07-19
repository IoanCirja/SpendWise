import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CurrentPlanComponent } from './current-plan/current-plan.component';
import { CategoryDetailsComponent } from './category-details/category-details.component';
import { HistoryComponent } from './history/history.component';
import { AccountSettingsComponent } from './account-settings/account-settings.component';
import { ManagePlansComponent } from './manage-plans/manage-plans.component';

const routes: Routes = [
  { path: 'current-plan', component: CurrentPlanComponent },
  { path: 'category-details', component: CategoryDetailsComponent },
  { path: 'history', component: HistoryComponent },
  { path: 'account-settings', component: AccountSettingsComponent },
  { path: 'manage-plans', component: ManagePlansComponent },
  { path: '', redirectTo: '/current-plan', pathMatch: 'full' } 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class DashboardRoutingModule { }
