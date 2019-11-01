import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { CardComponent } from './card/card.component';
import { RegistrationCardComponent } from './registration-card/registration-card.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastComponent } from './toast/toast.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    NavbarComponent,
    CardComponent,
    RegistrationCardComponent,
    ToastComponent
  ],
  exports: [
    NavbarComponent,
    CardComponent,
    RegistrationCardComponent,
    ToastComponent
  ],
  imports: [
    ReactiveFormsModule,
    RouterModule,
    NgbModule,
    CommonModule
  ]
})
export class ComponentsModule { }
