import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; 

import { DashboardRoutingModule } from './dashboard-routing.module';
import { CurrentPlanComponent } from './current-plan/current-plan.component';
import { DashboardNavigationComponent } from './dashboard-navigation/dashboard-navigation.component';
import { MatMenuModule } from '@angular/material/menu';
import { HttpClientModule } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { CategoryDetailsComponent } from './category-details/category-details.component';
import { HistoryComponent } from './history/history.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { ManagePlansComponent } from './manage-plans/manage-plans.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { TransactionModalComponent } from './transaction-modal/transaction-modal.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';


import { MatIcon } from '@angular/material/icon';
import { ConfirmCancelDialogComponent } from './cancel-plan-confirmation-modal/cancel-plan-confirmation-modal.component';
import { FluidIndicatorComponent } from '../budget-plan-choosing/fluid-indicator/fluid-indicator.component';
import { HistoryCategoryDetailsComponent } from './history-category-details/history-category-details.component';

@NgModule({
  declarations: [
    CurrentPlanComponent,
    DashboardNavigationComponent,
    CategoryDetailsComponent,
    HistoryComponent,
    StatisticsComponent,
    ManagePlansComponent,
    TransactionModalComponent,
    ConfirmCancelDialogComponent,
    HistoryCategoryDetailsComponent,
    FluidIndicatorComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MatMenuModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDialogModule,
    FormsModule,
    MatFormFieldModule,
    HttpClientModule,
    MatToolbarModule,
    RouterModule,  
    MatProgressSpinnerModule,
    MatTableModule,
    MatSortModule,
    
    
  ]
})
export class DashboardModule { }
