import { Injectable } from '@angular/core';
//added
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Segment } from '../models/segment.model';

const baseUrl = 'http://localhost:8080/api/new-client';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private http: HttpClient) { }

  getAllSegments(): Observable<Segment[]> {
    return this.http.get<Segment[]>('http://localhost:8080/api/new-segment');
  }
  create(data: any): Observable<any> {
    return this.http.post(baseUrl, data);
  }
}
