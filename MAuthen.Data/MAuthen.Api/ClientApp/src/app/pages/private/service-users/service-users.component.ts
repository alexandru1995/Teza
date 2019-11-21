import { Component, OnInit } from '@angular/core';
import { UserServiceModel } from 'src/app/models/service-models/user-service.model';
import { ServiceService } from 'src/app/service/service.service';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContactEditModalComponent } from 'src/app/components/contact-edit-modal/contact-edit-modal.component';
import { fadeAnimation } from 'src/app/components/animation';
import { RoleService } from 'src/app/service/role.service';
import { Role } from 'src/app/models/role-models/role.model';
import { AddRoleModalComponent } from 'src/app/components/add-role-modal/add-role-modal.component';
import { ChangeRoleModalComponent } from 'src/app/components/change-role-modal/change-role-modal.component';
import { ChangeRole } from 'src/app/models/role-models/change-user-role';

@Component({
  selector: 'app-service-users',
  templateUrl: './service-users.component.html',
  styleUrls: ['./service-users.component.css'],
  animations: [fadeAnimation]
})
export class ServiceUsersComponent implements OnInit {

  page = 1;
  pageSize = 9;
  collectionSize;
  loading: boolean = false;
  serviceId: string;
  error: string;

  users: UserServiceModel[];

  constructor(
    private service: ServiceService,
    private roleService: RoleService,
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

  blockUser(user) {
    var modalRef = this.modalService.open(ContactEditModalComponent);
    modalRef.componentInstance.title = "Block User";
    modalRef.componentInstance.message = "Are you sure you want to block this User ?";
    modalRef.componentInstance.isConfirmation = true;
    modalRef.result
      .then(() => {
        this.service.blockUser(user.id, this.serviceId).subscribe(
          data => {
            this.getUser()
          },
          (err) => {
            if (err.status == 403) {
              this.error = err.error;
              setTimeout(() => this.hidenError(), 3000);
            }
          }
        )
      })
      .catch(err => { })
  }

  unBlockUser(user) {
    this.service.unBlockUser(user.id, this.serviceId).subscribe(
      data => {
        this.getUser()
      },
      err => {
      })
  }

  addRole(){
    var modalRef = this.modalService.open(AddRoleModalComponent);
    modalRef.componentInstance.title = "Add Service Role";
    modalRef.result
      .then((role) => { 
        this.roleService.add(role.name,this.serviceId).subscribe(
          data => {
            this.getUser()
          },
          (err) => {
            if (err.status == 403) {
              this.error = err.error;
              setTimeout(() => this.hidenError(), 3000);
            }
          }
        )
      })
      .catch(err => { })
  }

  changeRole(userId){
    var modalRef = this.modalService.open(ChangeRoleModalComponent);
    modalRef.componentInstance.title = "Change Role";
    modalRef.componentInstance.serviceId = this.serviceId;
    modalRef.result
      .then((role) => { 
        var newRole : ChangeRole={
          roleId :role.id,
          serviceId: this.serviceId,
          userId: userId
        }
        this.roleService.chenge(newRole).subscribe(
          data => {
            this.getUser()
          },
          (err) => {
            if (err.status == 403) {
              this.error = err.error;
              setTimeout(() => this.hidenError(), 3000);
            }
          }
        )
      })
      .catch(err => { })
  }

  private getUser() {
    this.loading = true;
    this.service.getUsers(this.serviceId).subscribe(users => {
      this.users = users
      this.loading = false;
    })
  }
  private hidenError() {
    this.error = null
  }
}
