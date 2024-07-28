import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PasswordReset } from '../models/PasswordReset';

@Injectable({
  providedIn: 'root'
})
export class ChangePasswordService {
  private endPoint = 'https://localhost:7154/Authentication/ResetPassword';
  
  constructor(private http: HttpClient){}

  changePassword(data: PasswordReset): Observable<string> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post<string>(this.endPoint, data, { headers, responseType: 'text' as 'json' });
  }
}
