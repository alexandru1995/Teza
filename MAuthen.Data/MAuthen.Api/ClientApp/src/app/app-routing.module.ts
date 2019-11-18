import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './helpers/auth.guard';



const routes: Routes = [
  { path: '', loadChildren: `./pages/public/home/home.module#HomeModule` },
  { path: 'account', canActivate: [AuthGuard], canLoad: [AuthGuard], loadChildren: 'src/app/pages/private/account/account.module#AccountModule'},

  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
