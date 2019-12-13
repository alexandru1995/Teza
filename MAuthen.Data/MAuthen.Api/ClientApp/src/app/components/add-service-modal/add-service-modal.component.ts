import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ServiceService } from 'src/app/service/service.service';
import { FullService } from 'src/app/models/service-models/full-service.model';

@Component({
  selector: 'app-add-service-modal',
  templateUrl: './add-service-modal.component.html',
  styleUrls: ['./add-service-modal.component.css']
})
export class AddServiceModalComponent implements OnInit {


  @Input() title;
  @Input() existService: FullService;


  submitted: boolean = false;
  addServiceForm: FormGroup;

  time: string;

  constructor(
    private formBuilder: FormBuilder,
    public modal: NgbActiveModal,
    private service: ServiceService) { }

  get f() {
    return this.addServiceForm.controls;
  }

  ngOnInit(): void {
    if (this.existService) {
      this.addServiceForm = this.formBuilder.group({
        name: [this.existService.name, [Validators.required]],
        issuer: [this.existService.issuer, [Validators.required]],
        logOutUrl: [this.existService.logoutUrl, [Validators.pattern("^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$"), Validators.required]]
      })
    } else {
      this.addServiceForm = this.formBuilder.group({
        name: ['', [Validators.required]],
        issuer: ['', [Validators.required]],
        logOutUrl: ['', [Validators.pattern("^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$"), Validators.required]]
      })
    }

  }

  save() {
    this.submitted = true;
    if (this.addServiceForm.invalid) {
      return;
    }
    if (!this.existService) {
      this.addService();
    } else {
      this.updateService();
    }
  }

  private addService() {
    this.service.add(this.addServiceForm.value).subscribe(service => {
      if (service != null) {
        var data = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(service));
        var downloader = document.createElement('a');
        downloader.setAttribute('href', data);
        downloader.setAttribute('download', 'Settings.json');
        downloader.click();
      }
      this.modal.close(this.addServiceForm.value)
    },
      error => {
        const name = this.addServiceForm.controls['name']
        name.setErrors({
          fildExist:
          {
            valid: false,
            message: error.error
          }
        })
      }
    )
  }
  private updateService() {
    this.existService.name = this.addServiceForm.controls["name"].value;
    this.existService.issuer = this.addServiceForm.controls["issuer"].value;
    this.existService.logoutUrl = this.addServiceForm.controls["logOutUrl"].value;

    this.service.update(this.existService).subscribe(service => {
      this.modal.close()
    },
      error => {
        const name = this.addServiceForm.controls['name']
        name.setErrors({
          fildExist:
          {
            valid: false,
            message: error.error
          }
        })
      }
    )
  }
}


