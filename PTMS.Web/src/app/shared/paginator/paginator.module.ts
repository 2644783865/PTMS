import { NgModule } from '@angular/core';
import { MatPaginatorModule, MatPaginatorIntl } from '@angular/material/paginator';

import { PaginatorComponent } from './paginator.component';
import { MatPaginatorIntlRus } from './paginator.intl';

@NgModule({
  imports: [
    MatPaginatorModule,
  ],
  declarations: [PaginatorComponent],
  providers: [{ provide: MatPaginatorIntl, useClass: MatPaginatorIntlRus }],
  exports: [
    MatPaginatorModule,
    PaginatorComponent
  ]
})
export class PaginatorModule { }
