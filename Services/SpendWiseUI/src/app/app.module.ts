import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BudgetPlansModule } from './budget-plan-choosing/budget-plan-choosing.module';

import { BrowserModule } from '@angular/platform-browser';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import {OurTeamComponent} from "./our-team/our-team.component";
import {ContactUsComponent} from "./contact-us/contact-us.component";
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    OurTeamComponent,
    ContactUsComponent
  ],
  imports: [
    AppRoutingModule,
    BudgetPlansModule,
    RouterOutlet,
    MatToolbarModule,
    BrowserModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

