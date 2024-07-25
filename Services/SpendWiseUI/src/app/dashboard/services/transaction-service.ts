import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private apiUrl = 'https://localhost:7154/Transactions/AddTransaction';
  private getapiUrl = 'https://localhost:7154/Transactions/GetAllTransactions'; 

  constructor(private http: HttpClient) { }

  saveTransaction(transactionData: any): Observable<any> {
    return this.http.post(this.apiUrl, transactionData);
  }

  getAllTransactions(monthlyPlanId: string): Observable<any[]> {
    const url = `${this.getapiUrl}/${monthlyPlanId}`;
    return this.http.get<any[]>(url);
  }
}
