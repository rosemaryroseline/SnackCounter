import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Refill } from '../models/refill.model';

@Injectable({
  providedIn: 'root'
})
export class RefillService {

  private apiUrl = 'https://localhost:7190/api/Refills';

  constructor(private http: HttpClient) {}

  getRefills(): Observable<Refill[]> {
    return this.http.get<Refill[]>(this.apiUrl);
  }

  createRefill(request: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, request);
  }

  declineRefill(id: number, employeeId: number) {
  return this.http.post(
    `${this.apiUrl}/${id}/decline?employeeId=${employeeId}`,
    {}
  );
}
acceptRefill(id: number, employeeId: number): Observable<any> {
  return this.http.post(
    `${this.apiUrl}/${id}/accept?employeeId=${employeeId}`,
    {}
  );
}
purchaseRefill(id: number, purchaseData: any): Observable<any> {
  return this.http.post(
    `${this.apiUrl}/${id}/purchase`,
    purchaseData
  );
}
}