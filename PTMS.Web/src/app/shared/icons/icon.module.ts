import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { EditIconComponent } from './edit-icon.component';
import { DeleteIconComponent } from './delete-icon.component';
import { SaveIconComponent } from './save-icon.component';
import { CancelIconComponent } from './cancel-icon.component';
import { MenuIconDropdownComponent } from './menu-icon-dropdown.component';
import { InputClearIconComponent } from './input-clear-icon.component';
import { MatTooltipModule } from '@angular/material';
import { HintIconComponent } from './hint-icon.component';

@NgModule({
  imports: [
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatTooltipModule
  ],
  declarations: [
    EditIconComponent,
    DeleteIconComponent,
    SaveIconComponent,
    CancelIconComponent,
    MenuIconDropdownComponent,
    InputClearIconComponent,
    HintIconComponent
  ],
  exports: [
    EditIconComponent,
    DeleteIconComponent,
    SaveIconComponent,
    CancelIconComponent,
    MenuIconDropdownComponent,
    InputClearIconComponent,
    HintIconComponent
  ]
})
export class IconModule { }
