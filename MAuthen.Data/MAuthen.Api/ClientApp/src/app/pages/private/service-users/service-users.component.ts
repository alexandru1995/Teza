import { Component, OnInit } from '@angular/core';
import { UserServiceModel } from 'src/app/models/service-models/user-service.model';
import { ServiceService } from 'src/app/service/service.service';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContactEditModalComponent } from 'src/app/components/contact-edit-modal/contact-edit-modal.component';

@Component({
  selector: 'app-service-users',
  templateUrl: './service-users.component.html',
  styleUrls: ['./service-users.component.css']
})
export class ServiceUsersComponent implements OnInit {

  page = 1;
  pageSize = 9;
  collectionSize;
  loading : boolean = false;
  serviceId: string;

  users: UserServiceModel[];

  constructor(
    private service: ServiceService,
    private route: ActivatedRoute,
    private modalService: NgbModal,
  ) { }

  get userTable(): UserServiceModel[] {
    return this.users
      .map((user, i) => ({ id: i + 1, ...user }))
      .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }

  ngOnInit() {
    this.serviceId = this.route.snapshot.params.id
    this.getUser()
  }

  blockUser(userId){
    var modalRef = this.modalService.open(ContactEditModalComponent);
    modalRef.componentInstance.title = "Block User";
    modalRef.componentInstance.message = "Are you sure you want to block this User ?";
    modalRef.componentInstance.isConfirmation = true;
    modalRef.result
      .then(() => {
        this.service.blockUser(userId,this.serviceId).subscribe(
          data => {
            console.log("success")
          },
          err => { 
            console.log(err)
          }
        )
      })
      .catch(err => { })
  }

  private getUser(){
    this.loading = true;
    this.service.getUsers(this.serviceId).subscribe(users => {
      this.users = users
      this.loading = false;
    })
  }
}
