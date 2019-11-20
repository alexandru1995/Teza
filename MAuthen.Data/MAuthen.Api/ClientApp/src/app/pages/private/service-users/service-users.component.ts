import { Component, OnInit } from '@angular/core';
import { UserServiceModel } from 'src/app/models/service-models/user-service.model';
import { ServiceService } from 'src/app/service/service.service';
import { ActivatedRoute } from '@angular/router';

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

  users: UserServiceModel[];

  constructor(
    private service: ServiceService,
    private route: ActivatedRoute
  ) { }

  get userTable(): UserServiceModel[] {
    return this.users
      .map((user, i) => ({ id: i + 1, ...user }))
      .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }

  ngOnInit() {
    var serviceId = this.route.snapshot.params.id
    this.getUser(serviceId)
  }

  private getUser(serviceId){
    this.loading = true;
    this.service.getUsers(serviceId).subscribe(users => {
      this.users = users
      this.loading = false;
    })
  }
}
