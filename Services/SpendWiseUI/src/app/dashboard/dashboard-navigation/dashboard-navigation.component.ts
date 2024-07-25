import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TransactionModalComponent } from '../transaction-modal/transaction-modal.component'; 

@Component({
  selector: 'app-dashboard-navigation',
  templateUrl: './dashboard-navigation.component.html',
  styleUrls: ['./dashboard-navigation.component.scss']
})
export class DashboardNavigationComponent {
  constructor(public dialog: MatDialog) {}

  openTransactionModal(): void {
    this.dialog.open(TransactionModalComponent, {
      width: '300px', 
      data: {} 
    });
  }
}
