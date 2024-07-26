import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {RegisterComponent} from "./register/register.component";
import { AuthGuard } from '../guards/AuthGuard';

const routes: Routes = [
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] }, // Use AuthGuard here
  { path: 'register', component: RegisterComponent, canActivate: [AuthGuard] } // Use AuthGuard here
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
