import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonalInformation } from '../models/PersonalInformation';

@Injectable({
  providedIn: 'root'
})
export class PersonalInformationService {
  private endPoint = 'https://localhost:7154/Authentication/SaveAccountSettings';

  constructor(private http: HttpClient) { }

  personalInformation(data: PersonalInformation): Observable<any>{

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post<any>(this.endPoint, data, {headers});
  }
}
