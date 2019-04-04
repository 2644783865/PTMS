import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PtmsHttpClient {

  constructor(private http: HttpClient) {
  }

  private getFullUrl(relativeUrl: string) {
    return 'https://localhost:44355/' + relativeUrl;
  }

  get<T>(relativeUrl: string): Observable<T> {
    return this.http.get<T>(this.getFullUrl(relativeUrl));
  }

  post<T>(relativeUrl: string, dto: any): Observable<T> {
    return this.http.post<T>(this.getFullUrl(relativeUrl), dto);
  }
}
