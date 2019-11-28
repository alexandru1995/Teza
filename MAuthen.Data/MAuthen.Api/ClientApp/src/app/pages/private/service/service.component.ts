import { Component, OnInit } from '@angular/core';
import { Service } from 'src/app/models/service-models/service.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddServiceModalComponent } from 'src/app/components/add-service-modal/add-service-modal.component';
import { ServiceService } from 'src/app/service/service.service';
import { ContactEditModalComponent } from 'src/app/components/contact-edit-modal/contact-edit-modal.component';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css']
})
export class ServiceComponent implements OnInit {

  services: Service[];
  serviceForm: boolean = false;
  loading: boolean = false;

  page = 1;
  pageSize = 9;
  collectionSize;

  constructor(
    private modalService: NgbModal,
    private service: ServiceService,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit() {
    this.getService();
  }

  get servicesTable(): Service[] {
    return this.services
      .map((service, i) => ({ id: i + 1, ...service }))
      .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  }

  addService() {
    var modalRef = this.modalService.open(AddServiceModalComponent, { centered: true });
    modalRef.componentInstance.title = "Add Service";
    modalRef.result
      .then((rez) => {
        console.log(rez);
        this.service.add(rez).subscribe(service => {
          if(service!=null){
            var data = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(service));
            var downloader = document.createElement('a');

            downloader.setAttribute('href', data);
            downloader.setAttribute('download', 'Settings.json');
            downloader.click();
          }
            this.getService();
        })
      })
      .catch(err => { })
  }

  updateService(service) {
    var modalRef = this.modalService.open(AddServiceModalComponent, { centered: true });
    modalRef.componentInstance.title = "Edit Service";
    modalRef.componentInstance.name = service.name;
    modalRef.componentInstance.isshuer = service.isshuer;
    modalRef.result
      .then((rez) => {
        service.name = rez.name;
        service.domain = rez.domain;
        this.service.update(service).subscribe(service => {
          this.getService()
        })
      }
      )
      .catch(err => { })
  }

  deleteService(id: string) {
    var modalRef = this.modalService.open(ContactEditModalComponent);

    modalRef.componentInstance.title = "Delete Service";
    modalRef.componentInstance.message = "Are you sure you want to delete this Service ?";
    modalRef.componentInstance.isConfirmation = true;
    modalRef.result
      .then(() => {
        this.service.delete(id).subscribe(
          data => {
            this.getService();
          },
          err => { }
        )
      })
      .catch(err => { })
  }

  private getService() {
    this.loading = true;
    this.service.get().subscribe(service => {
      this.services = service;
      this.loading = false;
      this.collectionSize = this.services ? this.services.length : 0;
    })
  }

}
