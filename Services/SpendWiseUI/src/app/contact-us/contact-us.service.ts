import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ContactUsService {
  private endPoint = 'https://localhost:7154/ContactUs/AddSubscription';

  constructor(private http: HttpClient) {}

  contactUs(data: any): Observable<any> {
    return this.http.post<any>(this.endPoint, data);
  }

}
