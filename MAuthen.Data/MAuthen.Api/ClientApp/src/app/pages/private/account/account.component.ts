import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/service/user.service';
import { Contact } from 'src/app/models/contact.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContactEditModalComponent } from 'src/app/components/contact-edit-modal/contact-edit-modal.component';

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
    private userService: UserService,
    private modalService: NgbModal
  ) { }

  ngOnInit() {
    this.userService.getUser().subscribe(user => {
      this.birthday = new Date(user.birthday)
      this.contacts = user.contacts;
      this.user = user;
    })
  }

  addEmail() {
    console.log("Add Email");
    var modalRef = this.modalService.open(ContactEditModalComponent);
    modalRef.componentInstance.label = "Email";
    modalRef.componentInstance.title = "Add Email";
    modalRef.result
      .then((email) => {
        console.log(email)
      })
  }
}
