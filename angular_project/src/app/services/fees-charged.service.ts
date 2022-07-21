import { Injectable } from '@angular/core';
//added
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FeesCharged } from '../models/feescharged.model';

const baseUrl = 'http://localhost:8080/api/fees-charged';

@Injectable({
  providedIn: 'root'
})
export class FeesChargedService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<FeesCharged[]> {
    return this.http.get<FeesCharged[]>(baseUrl);
  }

  get(cpf: any, id: any): Observable<FeesCharged> {
    return this.http.get(`${baseUrl}/${cpf}/${id}`);
  }

  create(data: any): Observable<any> {
    return this.http.post(baseUrl, data);
  }

  update(cpf: any, id: any, data: any): Observable<any> {
    return this.http.put(`${baseUrl}/${cpf}/${id}`, data);
  }

  delete(cpf: any, id: any): Observable<any> {
    return this.http.delete(`${baseUrl}/${cpf}/${id}`);
  }

  deleteAll(): Observable<any> {
    return this.http.delete(baseUrl);
  }

  findByClientAndSegment(cpf: any, segment: any): Observable<FeesCharged[]> {
    if (cpf && segment)
      return this.http.get<FeesCharged[]>(`${baseUrl}?client=${cpf}&segment=${segment}`);
    else if (cpf)
      return this.http.get<FeesCharged[]>(`${baseUrl}?client=${cpf}`);
    else if (segment)
      return this.http.get<FeesCharged[]>(`${baseUrl}?segment=${segment}`);
    return this.http.get<FeesCharged[]>(`${baseUrl}`);
  }
}
