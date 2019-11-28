import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-service-modal',
  templateUrl: './add-service-modal.component.html',
  styleUrls: ['./add-service-modal.component.css']
})
export class AddServiceModalComponent implements OnInit {


  @Input() title;
  @Input() name: string = '';
  @Input() isshuer: string;
  @Input() logOutUrl: string;
  @Input() password: string = '';
  @Input() tokenExpirationTime: string;


  submitted: boolean = false;
  addServiceForm: FormGroup;

  time:string;

  constructor(
    private formBuilder: FormBuilder,
    public modal: NgbActiveModal,
    private modalService: NgbModal) { }

  get f() {
    return this.addServiceForm.controls;
  }

  ngOnInit(): void {
    this.addServiceForm = this.formBuilder.group({
      name: [this.name, [Validators.required]],
      issuer: [this.isshuer, [Validators.required]],
      logOutUrl: [this.logOutUrl, [Validators.pattern("^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$"), Validators.required]]
    })
  }

  save() {
    this.submitted = true;
    if (this.addServiceForm.invalid) {
      return;
    }
    this.modal.close(this.addServiceForm.value)
  }
}


