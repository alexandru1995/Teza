import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { CardComponent } from './card/card.component';
import { RegistrationCardComponent } from './registration-card/registration-card.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    NavbarComponent,
    CardComponent,
    RegistrationCardComponent
  ],
  exports: [
    NavbarComponent,
    CardComponent,
    RegistrationCardComponent,

  ],
  imports: [
    ReactiveFormsModule,
    CommonModule
  ]
})
export class ComponentsModule { }
