import { Component, OnInit } from '@angular/core';
import { HistoryService } from '../services/history.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {

  historyPlans: any[] = []; 

  constructor(private historyService: HistoryService) { }

  ngOnInit(): void {
    const userId = '04A7CE09-3B90-4940-835A-1EFEA93370B7'; 

    this.loadHistoryPlans(userId);
  }

  loadHistoryPlans(userId:string): void {
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
