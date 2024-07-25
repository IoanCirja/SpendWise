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

  changePassword(data : PasswordReset): Observable<any>{

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post<any>(this.endPoint, data, {headers});
  }

}