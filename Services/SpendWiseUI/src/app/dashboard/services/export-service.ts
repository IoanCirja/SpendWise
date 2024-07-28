import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class ExportService {
  private baseUrl = 'https://localhost:7154';

  constructor(private http: HttpClient) {}

  exportPlan(userId: string, year: number, month: number): Observable<Blob> {
    const url = `${this.baseUrl}/MonthlyPlan/ExportDetailsByMonthAndYearToPdf/${userId}?year=${year}&month=${month}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  downloadPlan(blob: Blob, fileName: string): void {
    saveAs(blob, fileName);
  }
}
