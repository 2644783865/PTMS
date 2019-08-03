import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PtmsHttpClient } from '../data-services/ptms.http.client';
import { BlockTypeDto } from '../dtos/BlockTypeDto';

@Injectable({
  providedIn: 'root'
})
export class BlockTypeDataService {

  constructor(private http: PtmsHttpClient) {
  }

  getAll(): Observable<BlockTypeDto[]> {
    return this.http.get<BlockTypeDto[]>('blockTypes');
  }
}
