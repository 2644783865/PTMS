import { InjectionToken, inject, Type } from '@angular/core';
import { QueryEntity, ID } from '@datorama/akita';
import { AppPaginator, AppPaginatorPlugin } from './app-paginator.plugin';

export function createPaginator<TModel>(name: string, query: Type<QueryEntity<any, TModel, ID>>): InjectionToken<AppPaginatorPlugin<TModel>> {
  return new InjectionToken(name, {
    factory: () => {
      return new AppPaginator<TModel>(inject(query));
    }
  });
}
