import { Injectable } from '@angular/core';
//added
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Segment } from '../models/segment.model';

const baseUrl = 'http://localhost:8080/api/new-segment';


@Injectable({
  providedIn: 'root'
})
export class SegmentService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Segment[]> {
    return this.http.get<Segment[]>(baseUrl);
  }
  create(data: any): Observable<any> {
    return this.http.post(baseUrl, data);
  }
}
