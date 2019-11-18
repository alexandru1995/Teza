import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/service/user.service';
import { Contact } from 'src/app/models/contact.model';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  user: User;
  birthday: Date;
  contacts: Contact[];
  constructor(
    private userService: UserService
  ) { }

  ngOnInit() {
    this.userService.getUser().subscribe(user => { 
      this.birthday = new Date(user.birthday)
      this.contacts = user.contacts;
      this.user = user;
    })
  }
}
