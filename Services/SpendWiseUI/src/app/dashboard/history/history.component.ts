import { Component, OnInit } from '@angular/core';
import { HistoryService } from '../services/history.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {

  historyPlans: any[] = [];
  userId: string | null = null;

  constructor(private historyService: HistoryService) { }

  ngOnInit(): void {
    this.userId = this.getUserIdFromLocalStorage();
    if (this.userId) {
      this.loadHistoryPlans(this.userId);
    } else {
      console.error('User ID not found in local storage.');
    }
  }

  getUserIdFromLocalStorage(): string | null {
    const userString = localStorage.getItem('currentUser');
    if (userString) {
      const user = JSON.parse(userString);
      return user.id;
    }
    return null;
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
}
