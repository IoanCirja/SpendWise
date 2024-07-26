import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { DashboardButtonService } from '../services/dashboard-button-service';
import { TransactionModalComponent } from '../transaction-modal/transaction-modal.component'; 

@Component({
  selector: 'app-dashboard-navigation',
  templateUrl: './dashboard-navigation.component.html',
  styleUrls: ['./dashboard-navigation.component.scss']
})
export class DashboardNavigationComponent implements OnInit {
  hasCurrentPlan$: Observable<boolean> | undefined;

  constructor(
    public dialog: MatDialog,
    private dashboardButtonService: DashboardButtonService
  ) {}

  ngOnInit(): void {
    this.hasCurrentPlan$ = this.dashboardButtonService.hasCurrentPlan();
  }

  openTransactionModal(): void {
    this.dialog.open(TransactionModalComponent, {
      width: '300px', 
      data: {} 
    });
  }
}
