import { Component, Output, EventEmitter } from '@angular/core';
import {  NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html'
})
export class DatepickerComponent {
  model: NgbDateStruct;
  @Output() birthday = new EventEmitter<any>()
  onDateSelect($event){
    this.birthday.emit($event)
  }
}