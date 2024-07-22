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
    const userId = 'E28D2431-5530-4C67-9CCB-152E7317DCE4'; 

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
