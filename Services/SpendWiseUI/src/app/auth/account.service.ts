import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map, Observable, ReplaySubject} from 'rxjs';
import {AuthenticatedUser} from "./models/AuthenticatedUser";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private loginEndPoint = 'https://localhost:7154/Authentication/LoginUser';
  private registerEndPoint = 'https://localhost:7154/Authentication/RegisterUser';

  private currentUserSource = new ReplaySubject<AuthenticatedUser | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {
    const storedUser = localStorage.getItem('currentUser');
    const user: AuthenticatedUser | null = storedUser ? JSON.parse(storedUser) : null;
    this.currentUserSource.next(user);
  }
  login(data: any): Observable<any> {
    return this.http.post<any>(this.loginEndPoint, data).pipe(
      map((response: any) => {
        const user: AuthenticatedUser = response;
        if (user) {
          localStorage.setItem('currentUser', JSON.stringify(user));
          localStorage.setItem('token', response.jwtToken);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(data: any): Observable<any> {
    return this.http.post<any>(this.registerEndPoint, data);
  }

  logout() {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
  }
}
