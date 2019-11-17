import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/service/user.service';
import { AuthorizationService } from 'src/app/service/authorization.service';
import { Router } from '@angular/router';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  user;

  constructor(
    private authorizationService: AuthorizationService,
    private router : Router
    ) { }
  

    menuItems = [
    { name: "Home", link: "/" },
    { name: "Services", link: "/" },
    { name: "Account", link: "/account" }
  ];

  ngOnInit() {
    this.authorizationService.currentUser.subscribe(user => this.user = user);
    if(this.user == null){
      this.user = JSON.parse(localStorage.getItem("User"))
    }
  }

  signOut(){
    this.user = null;
    this.authorizationService.logout().subscribe();
    this.router.navigate(['/'])
  }
}
