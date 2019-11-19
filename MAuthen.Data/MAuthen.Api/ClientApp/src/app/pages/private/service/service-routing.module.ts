import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ServiceComponent } from './service.component';
import { AuthGuard } from 'src/app/helpers/auth.guard';


const routes: Routes = [
  {
    path: '', canActivate: [AuthGuard], component: ServiceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ServiceRoutingModule { }
