import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {

  @Output() switch = new EventEmitter<boolean>()

  constructor() { }

  ngOnInit() {
  }

  swichToTegistration(){
    this.switch.emit(true)
  }
}
