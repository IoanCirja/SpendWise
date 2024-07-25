import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChangePasswordService {
  private endPoint = 'https://localhost:7154/Authentication/ResetPassword';
  
  constructor(private http: HttpClient){}

  changePassword(data: any): Observable<any>{

    return this.http.post<any>(this.endPoint,data);
  }

}