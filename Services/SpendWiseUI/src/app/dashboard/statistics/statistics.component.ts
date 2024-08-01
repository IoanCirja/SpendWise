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

  constructor(
    private statisticsService: StatisticsService,
    private accountService: AccountService
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser();
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
        this.loadStatistics();
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

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
