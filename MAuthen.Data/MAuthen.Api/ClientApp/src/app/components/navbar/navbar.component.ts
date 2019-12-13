import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from 'src/app/service/authorization.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  user;

  constructor(
    private authorizationService: AuthorizationService,
    private router: Router
  ) { }
  menuItems = [
    { name: "Services", link: "/services" },
    { name: "Account", link: "/account" }
  ];

  ngOnInit() {
    this.authorizationService.currentUser.subscribe(user => this.user = user);
    if (this.user == null) {
      this.user = JSON.parse(localStorage.getItem("User"))
    }
  }

  signOut() {    
    this.user = null;
    this.router.navigateByUrl("/");
    this.authorizationService.logout().subscribe();

  }
}
