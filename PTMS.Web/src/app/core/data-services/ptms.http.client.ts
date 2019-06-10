import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppPaginationResponse } from '../akita-extensions/app-paged-entity-state';
import { PageResult } from '../dtos/page.result';

@Injectable({
  providedIn: 'root'
})
export class PtmsHttpClient {

  constructor(private http: HttpClient) {
  }

  private getFullUrl(relativeUrl: string) {
    return environment.apiUrl + relativeUrl;
  }

  getPage<T>(
    relativeUrl: string,
    page: number = 1,
    pageSize: number = 10,
    params: object = null): Observable<AppPaginationResponse<T>> {

    page = page || 1;
    pageSize = pageSize || 10;

    let httpParams = this.convertToHttpParams(params)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    const options = { params: httpParams };

    return this.http.get<PageResult<T>>(this.getFullUrl(relativeUrl), options)
      .pipe(
        map(response => {
          let result = {
            currentPage: page,
            perPage: pageSize,
            data: response.page,
            total: response.totalCount
          } as AppPaginationResponse<T>;

          return result;
        })
      );
  }

  get<T>(relativeUrl: string, params: object = null): Observable<T> {
    const options = { params: this.convertToHttpParams(params) };
    return this.http.get<T>(this.getFullUrl(relativeUrl), options);
  }

  post<T>(relativeUrl: string, dto: any = null): Observable<T> {
    return this.http.post<T>(this.getFullUrl(relativeUrl), dto);
  }

  put<T>(relativeUrl: string, dto: any): Observable<T> {
    return this.http.put<T>(this.getFullUrl(relativeUrl), dto);
  }

  private convertToHttpParams(params: object): HttpParams {
    let result = new HttpParams();

    for (var key in params)
    {
      let value = params[key];

      if (value !== null && value !== undefined && value !== '') {
        result = result.set(key, value);
      }
    }

    return result;
  }
}
