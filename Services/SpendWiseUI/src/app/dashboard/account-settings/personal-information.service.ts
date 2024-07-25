import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PersonalInformationService {
  private endPoint = 'https://localhost:7154/Authentication/SaveAccountSettings';

  constructor(private http: HttpClient) { }
  user: any;

  personalInformation(data: any ): Observable<any>{

    return this.http.post<any>(this.endPoint, data);
  }
}
