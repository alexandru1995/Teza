import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { CardComponent } from './card/card.component';
import { RegistrationCardComponent } from './registration-card/registration-card.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastComponent } from './toast/toast.component';
import { RouterModule } from '@angular/router';
import { DatepickerComponent } from './datepicker/datepicker.component';
import { ContactEditModalComponent } from './contact-edit-modal/contact-edit-modal.component';
import { AddServiceModalComponent } from './add-service-modal/add-service-modal.component';

@NgModule({
  declarations: [
    NavbarComponent,
    CardComponent,
    RegistrationCardComponent,
    ToastComponent,
    DatepickerComponent,
    ContactEditModalComponent,
    AddServiceModalComponent
  ],
  exports: [
    NavbarComponent,
    CardComponent,
    RegistrationCardComponent,
    ToastComponent,
    DatepickerComponent,
    ContactEditModalComponent,
    AddServiceModalComponent
  ],
  entryComponents:[
    ContactEditModalComponent,
    AddServiceModalComponent
  ],
  imports: [
    ReactiveFormsModule,
    RouterModule,
    FormsModule,
    NgbModule,
    CommonModule
  ],
  bootstrap: [DatepickerComponent]
})
export class ComponentsModule { }
