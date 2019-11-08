import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { DataService } from 'src/app/service/data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  user: User;

  constructor(
    private data: DataService
    ) { }
  

    menuItems = [
    { name: "Home", link: "/" },
    { name: "Services", link: "/" },
    { name: "Account", link: "/account" }
  ];

  ngOnInit() {
    this.data.currentMessage.subscribe(user => this.user = user);
    if(this.user == null){
      this.user = JSON.parse(localStorage.getItem("user"))
    }
  }

  signOut(){
    localStorage.removeItem("user")
    this.user = null;
  }

}
