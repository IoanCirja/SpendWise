import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private apiUrl = 'https://localhost:7154/Statistics/GetStatistics';
  private baseUrl = 'https://localhost:7154/Transactions/GetAllTransactionForUser'; // Update with your API base URL

  constructor(private http: HttpClient) { }

  getStatistics(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${userId}`);
  }

  getAllTransactionsForUser(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/${userId}`);
  }
}
