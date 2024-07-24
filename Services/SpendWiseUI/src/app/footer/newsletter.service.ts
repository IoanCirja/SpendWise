import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NewsletterService {
  private endPoint = 'https://localhost:7154/Newsletter/AddSubscription';
  constructor(private http: HttpClient) { }
  submitNewsletter(email: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = JSON.stringify(email);
    return this.http.post(this.endPoint, body, { headers, responseType: 'text' });
  }
}
