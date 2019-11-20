import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ServiceUsersComponent } from './service-users.component';


const routes: Routes = [
  {path:'', component:ServiceUsersComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ServiceUsersRoutingModule { }
