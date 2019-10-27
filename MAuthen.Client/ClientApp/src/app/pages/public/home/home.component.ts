import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  showRegistration: boolean = false;
  constructor() { }

  ngOnInit() {
  }

  changeToRedgistration($event) {
    if (this.showRegistration) {
      this.showRegistration = false;
    } else {
      this.showRegistration = true;
    }
  }
}
