import { Component, OnInit, OnDestroy } from '@angular/core';
import { StatisticsService } from '../services/statistics-service';
import { AccountService } from '../../auth/account.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit, OnDestroy {
  statistics: any = {};
  userId: string | null = null;
  subscriptions: Subscription[] = [];
  transactions: any[] = [];
  chartOptions: any;

  constructor(
    private statisticsService: StatisticsService,
    private accountService: AccountService
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.prepareDummyChartOptions(); // For dummy data verification
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
        this.loadStatistics();
        this.loadTransactions();
      } else {
        console.error('No user found');
      }
    });
    this.subscriptions.push(subscription);
  }

  loadStatistics(): void {
    if (!this.userId) {
      console.error('User ID is required to load statistics.');
      return;
    }

    this.statisticsService.getStatistics(this.userId).subscribe(
      data => {
        this.statistics = data;
      },
      error => {
        console.error('Error fetching statistics', error);
      }
    );
  }

  loadTransactions(): void {
    if (!this.userId) {
      console.error('User ID is required to load transactions.');
      return;
    }

    this.statisticsService.getAllTransactionsForUser(this.userId).subscribe(
      data => {
        this.transactions = data;
        this.prepareChartOptions();
      },
      error => {
        console.error('Error fetching transactions', error);
      }
    );
  }

  prepareDummyChartOptions(): void {
    const dataPoints = [
      { x: new Date(2023, 6, 1), y: 100, label: 'Transaction 1' },
      { x: new Date(2023, 6, 2), y: 200, label: 'Transaction 2' },
      { x: new Date(2023, 6, 3), y: 150, label: 'Transaction 3' },
    ];

    this.chartOptions = {
      animationEnabled: true,
      theme: "light2",
      title: {
        text: "User Transactions"
      },
      axisX: {
        valueFormatString: "DD MMM YYYY",
        interval: 1,
        intervalType: "day",
        labelAngle: -50
      },
      axisY: {
        title: "Amount",
        prefix: "$"
      },
      data: [{
        type: "column",
        dataPoints: dataPoints
      }]
    };

    console.log(this.chartOptions); // Log the chart options to verify the data
  }

  prepareChartOptions(): void {
    // Group transactions by month for simplicity
    const groupedTransactions = this.groupTransactionsByMonth(this.transactions);
    const dataPoints = groupedTransactions.map(transaction => ({
      x: new Date(transaction.date),
      y: transaction.amount,
      label: transaction.name
    }));

    this.chartOptions = {
      animationEnabled: true,
      theme: "light2",
      title: {
        text: "User Transactions"
      },
      axisX: {
        valueFormatString: "MMM YYYY",
        interval: 1,
        intervalType: "month",
        labelAngle: -50
      },
      axisY: {
        title: "Amount",
        prefix: "$"
      },
      data: [{
        type: "column",
        dataPoints: dataPoints,
        click: this.onDataPointClick // Optional: Add click event for data points
      }],
      zoomEnabled: true, // Enable zooming
      panEnabled: true // Enable panning
    };

    console.log(this.chartOptions); // Log the chart options to verify the data
  }

  groupTransactionsByMonth(transactions: any[]): any[] {
    const grouped = transactions.reduce((acc, transaction) => {
      const date = new Date(transaction.date);
      const month = date.getFullYear() + '-' + (date.getMonth() + 1);
      if (!acc[month]) {
        acc[month] = { date: date, amount: 0, name: month };
      }
      acc[month].amount += transaction.amount;
      return acc;
    }, {});

    return Object.values(grouped);
  }

  onDataPointClick(e: any): void {
    console.log(`Data point clicked: ${e.dataPoint.label} - ${e.dataPoint.y}`);
    // Implement additional logic if needed
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
