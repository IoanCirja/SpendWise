import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ContactUsComponent} from "./contact-us/contact-us.component";
import {OurTeamComponent} from "./our-team/our-team.component";
import { CurrentPlanComponent } from './dashboard/current-plan/current-plan.component';
import { DashboardNavigationComponent } from './dashboard/dashboard-navigation/dashboard-navigation.component';
import { AccountSettingsComponent } from './dashboard/account-settings/account-settings.component';
import { CategoryDetailsComponent } from './dashboard/category-details/category-details.component';
import { HistoryComponent } from './dashboard/history/history.component';
import { ManagePlansComponent } from './dashboard/manage-plans/manage-plans.component';
import {HomeComponent} from "./home/home.component";

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'auth', loadChildren: () =>
      import('./auth/auth.module').then((m) => m.AuthModule),
  },
  { path: 'budget-plans', loadChildren: () =>
    import('./budget-plan-choosing/budget-plan-choosing.module').then((m) => m.BudgetPlansModule),
  },
  { path: 'contact-us', component: ContactUsComponent },
  { path: 'our-team', component: OurTeamComponent },



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
