import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { CardComponent } from './card/card.component';



@NgModule({
  declarations: [NavbarComponent,  CardComponent],
  exports:[
    NavbarComponent,
    CardComponent
  ],
  imports: [
    CommonModule
  ]  
})
export class ComponentsModule { }
