import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServiceUsersRoutingModule } from './service-users-routing.module';
import { ServiceUsersComponent } from './service-users.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    ServiceUsersComponent
  ],
  imports: [
    CommonModule,
    ServiceUsersRoutingModule,
    NgbModule
  ]
})
export class ServiceUsersModule { }
