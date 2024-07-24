import {Component, OnDestroy, OnInit} from '@angular/core';
import { HistoryService } from '../services/history.service';
import {Subscription} from "rxjs";
import {AccountService} from "../../auth/account.service";

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit,OnDestroy{

  historyPlans: any[] = [];
  userId: string | null = null;
  subscriptions: Subscription[] = [];

  constructor(private historyService: HistoryService,
              private accountService: AccountService) { }

  ngOnInit(): void {
    this.loadCurrentUser();
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      if (currentUser) {
        this.userId = currentUser.id;
        this.loadHistoryPlans(this.userId);
      }
    })
    this.subscriptions.push(subscription);
  }

  loadHistoryPlans(userId: string): void {
    this.historyService.getHistoryPlans(userId).subscribe(
      data => {
        this.historyPlans = data;
      },
      error => {
        console.error('Error fetching history plans', error);
      }
    );
  }

  onPlanSelect(planId: string): void {
    console.log('Selected plan ID:', planId);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }
}
