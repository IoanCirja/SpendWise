import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CurrentPlanComponent } from './current-plan/current-plan.component';
import { CategoryDetailsComponent } from './category-details/category-details.component';
import { HistoryComponent } from './history/history.component';
import { AccountSettingsComponent } from './account-settings/account-settings.component';
import { ManagePlansComponent } from './manage-plans/manage-plans.component';
import { DashboardNavigationComponent } from './dashboard-navigation/dashboard-navigation.component'; 
import { StatisticsComponent } from './statistics/statistics.component';
import { HistoryCategoryDetailsComponent } from './history-category-details/history-category-details.component';
import { DashboardGuard } from '../guards/DashboardGuard';
const routes: Routes = [
  {
    path: 'dashboard', 
    component: DashboardNavigationComponent, 
    canActivate: [DashboardGuard],
    children: [
      { path: 'current-plan', component: CurrentPlanComponent },
      { path: 'category-details', component: CategoryDetailsComponent },
      { path: 'history-category-details', component: HistoryCategoryDetailsComponent },
      { path: 'history', component: HistoryComponent },
      { path: 'account-settings', component: AccountSettingsComponent },
      { path: 'manage-plans', component: ManagePlansComponent },
      { path: 'statistics', component: StatisticsComponent },
      { path: '', redirectTo: 'current-plan', pathMatch: 'full' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)], 
  exports: [RouterModule]
})

export class DashboardRoutingModule { }
