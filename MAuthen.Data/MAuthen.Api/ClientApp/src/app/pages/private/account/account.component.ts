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
    this.getUser();
  }

  addEmail() {
    var modalRef = this.modalService.open(ContactEditModalComponent);
    modalRef.componentInstance.label = "Email";
    modalRef.componentInstance.title = "Add Email";
    modalRef.componentInstance.type = "email"
    modalRef.result
      .then((email) => {
        this.userService.addContact({ "email": email })
          .subscribe(contacts => this.contacts = contacts)
      }
      )
      .catch(err => { })
  }

  addPhone() {
    var modalRef = this.modalService.open(ContactEditModalComponent);
    modalRef.componentInstance.label = "Phone Number";
    modalRef.componentInstance.title = "Add Phone Number";
    modalRef.componentInstance.type = "phone"
    modalRef.result
      .then((phone) => {
        this.userService.addContact({ "phone": phone })
          .subscribe(contacts => this.contacts = contacts)
      }
      )
      .catch(err => { })
  }

  updateContact(contact: Contact, type: string) {
    var modalRef = this.modalService.open(ContactEditModalComponent);
    if (type === "phone") {
      modalRef.componentInstance.label = "Phone Number";
      modalRef.componentInstance.title = "Update Phone Number";
      modalRef.componentInstance.type = "phone";
      modalRef.componentInstance.value = contact.phone;
      modalRef.result
        .then((value) => {
          contact.phone = value;
          this.userService.updateContact(contact)
            .subscribe(
              data => { },
              err => { }
            )
        })
        .catch(err => { })
    }
    else if (type === "email") {
      modalRef.componentInstance.label = "Email";
      modalRef.componentInstance.title = "Update Email";
      modalRef.componentInstance.type = "email"
      modalRef.componentInstance.value = contact.email;
      modalRef.result
        .then((value) => {
          contact.email = value;
          this.userService.updateContact(contact)
            .subscribe(
              data => { },
              err => { }
            )
        })
        .catch(err => { })
    }
  }
  DeleteContact(contact: Contact, type: string) {
    var modalRef = this.modalService.open(ContactEditModalComponent);
    if (type === "phone") {
      modalRef.componentInstance.title = "Delete Phone Number";
      modalRef.componentInstance.message = "Are you sure you want to delete this Phone Number ?";
      modalRef.componentInstance.isConfirmation = true;
      modalRef.result
        .then(() => {
          if (contact.email != null) {
            contact.phone = null;
            this.userService.updateContact(contact)
              .subscribe(
                data => { },
                err => { }
              )
          } else {
            this.userService.deleteContact(contact.id)
              .subscribe(
                data => {
                  this.getUser();
                },
                err => { }
              )
          }
        })
        .catch(err => { })
    }
    else if (type === "email") {
      modalRef.componentInstance.title = "Delete Email address";
      modalRef.componentInstance.message = "Are you sure you want to delete this email address ?";
      modalRef.componentInstance.isConfirmation = true;
      modalRef.result
        .then(() => {
          
          console.log("-----")
          if (contact.phone != null) {
            contact.email = null;
            this.userService.updateContact(contact)
              .subscribe(
                data => { },
                err => { }
              )
          } else {
            this.userService.deleteContact(contact.id)
              .subscribe(
                data => {
                  this.getUser();
                },
                err => { }
              )
          }
        })
        .catch(err => { 
          console.log(err)
        })
    }
  }

  private getUser() {
    this.userService.getUser().subscribe(user => {
      console.log(user);
      this.contacts = user.contacts;
      this.user = user;
    })
  }
}

