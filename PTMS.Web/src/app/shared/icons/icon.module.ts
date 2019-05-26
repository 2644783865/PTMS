import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { EditIconComponent } from './edit-icon.component';
import { DeleteIconComponent } from './delete-icon.component';
import { SaveIconComponent } from './save-icon.component';
import { CancelIconComponent } from './cancel-icon.component';
import { MenuIconDropdownComponent } from './menu-icon-dropdown.component';

@NgModule({
  imports: [
    MatButtonModule,
    MatIconModule,
    MatMenuModule
  ],
  declarations: [
    EditIconComponent,
    DeleteIconComponent,
    SaveIconComponent,
    CancelIconComponent,
    MenuIconDropdownComponent
  ],
  exports: [
    EditIconComponent,
    DeleteIconComponent,
    SaveIconComponent,
    CancelIconComponent,
    MenuIconDropdownComponent
  ]
})
export class IconModule { }
