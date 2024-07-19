import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ContactUsComponent} from "./contact-us/contact-us.component";
import {OurTeamComponent} from "./our-team/our-team.component";
import { CurrentPlanComponent } from './dashboard/current-plan/current-plan.component';
import { DashboardNavigationComponent } from './dashboard/dashboard-navigation/dashboard-navigation.component';

const routes: Routes = [
  { path: 'auth', loadChildren: () =>
      import('./auth/auth.module').then((m) => m.AuthModule),
  },
  { path: 'budget-plans', loadChildren: () =>
    import('./budget-plan-choosing/budget-plan-choosing.module').then((m) => m.BudgetPlansModule),
  },
  { path: 'contact-us', component: ContactUsComponent },
  { path: 'our-team', component: OurTeamComponent },
  { path: 'dashboard', component: DashboardNavigationComponent}


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
