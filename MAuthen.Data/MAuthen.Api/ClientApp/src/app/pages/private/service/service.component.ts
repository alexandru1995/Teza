import { Component, OnInit } from '@angular/core';
import { Service } from 'src/app/models/service-models/service.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddServiceModalComponent } from 'src/app/components/add-service-modal/add-service-modal.component';

@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css']
})
export class ServiceComponent implements OnInit {

  services: Service[];
  serviceForm: boolean = false;

  constructor(
    private modalService: NgbModal
  ) { }

  ngOnInit() {
  }

  addService() {
    var modalRef = this.modalService.open(AddServiceModalComponent, { centered: true });
    modalRef.componentInstance.title = "Add Service"
    modalRef.result
      .then((rez) => {
        console.log(rez)
        // this.userService.addContact({ "email": email })
        //   .subscribe(contacts => this.contacts = contacts)
      }
      )
      .catch(err => { })
  }

}
