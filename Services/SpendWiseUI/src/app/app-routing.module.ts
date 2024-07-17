import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ContactUsComponent} from "./contact-us/contact-us.component";
import {OurTeamComponent} from "./our-team/our-team.component";

const routes: Routes = [
  { path: 'auth', loadChildren: () =>
      import('./auth/auth.module').then((m) => m.AuthModule),
  },
  { path: 'budget-plans', loadChildren: () =>
    import('./budget-plan-choosing/budget-plan-choosing.module').then((m) => m.BudgetPlansModule),
  },
  { path: 'contact-us', component: ContactUsComponent },
  { path: 'our-team', component: OurTeamComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
