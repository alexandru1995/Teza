import { Component, OnInit } from '@angular/core';
import { ToastService } from 'src/app/components/toast/toast.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  showRegistration: boolean = false;
  constructor(public toastService: ToastService) { }

  ngOnInit() {
  }

  registrationError(error){
    this.toastService.show(error.message);
  }

  changeToRedgistration($event) {
    if (this.showRegistration) {
      this.showRegistration = false;
    } else {
      this.showRegistration = true;
    }
  }
}
