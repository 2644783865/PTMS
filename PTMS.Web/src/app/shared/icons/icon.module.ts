import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { EditIconComponent } from './edit-icon.component';
import { DeleteIconComponent } from './delete-icon.component';
import { SaveIconComponent } from './save-icon.component';
import { CancelIconComponent } from './cancel-icon.component';

@NgModule({
  imports: [
    MatButtonModule,
    MatIconModule
  ],
  declarations: [
    EditIconComponent,
    DeleteIconComponent,
    SaveIconComponent,
    CancelIconComponent
  ],
  exports: [
    EditIconComponent,
    DeleteIconComponent,
    SaveIconComponent,
    CancelIconComponent
  ]
})
export class IconModule { }
