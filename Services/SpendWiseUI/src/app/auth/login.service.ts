import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private endPoint = 'https://localhost:7154/Authentication/LoginUser';

  constructor(private http: HttpClient) {}

  login(data: any): Observable<any> {
    console.log(data);
    return this.http.post<any>(this.endPoint, data);
  }
}
