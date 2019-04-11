import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserDto } from '@app/core/dtos/UserDto';
import { UserDataService } from '@app/core/data-services/user.data.service';
import { UserQuery, UserStore } from './user.state';

@Injectable()
export class UserService {
  public readonly isLoading$: Observable<boolean>;
  public readonly list$: Observable<UserDto[]>;

  constructor(
    private userQuery: UserQuery,
    private userStore: UserStore,
    private userDataService: UserDataService)
  {
    this.isLoading$ = this.userQuery.selectLoading();
    this.list$ = this.userQuery.selectAll();
  }  

  async loadData() {
    let data = await this.userDataService.getAll().toPromise();
    this.userStore.set(data);
    this.userStore.setLoading(false);
  }

  onDestroy() {
    this.userStore.reset();
  }
}
