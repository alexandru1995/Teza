import { Component, OnInit } from '@angular/core';
import { UserService1 } from 'src/app/service/user1.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  user: User;
  birthDay: Date;
  constructor(
    private userService: UserService1
  ) { }

  ngOnInit() {
    this.userService.getUser().subscribe(user => { 
      this.user = user;
      this.birthDay = new Date(this.user.birthday)
      console.log(this.birthDay);
    })
  }
}
