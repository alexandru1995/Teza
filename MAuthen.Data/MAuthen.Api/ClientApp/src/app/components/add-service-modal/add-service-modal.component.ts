import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Service } from 'src/app/models/service-models/service.model';

@Component({
  selector: 'app-add-service-modal',
  templateUrl: './add-service-modal.component.html',
  styleUrls: ['./add-service-modal.component.css']
})
export class AddServiceModalComponent implements OnInit {


  @Input() title;
  @Input() name: string;
  @Input() domain: string;

  submitted: boolean = false;
  addServiceForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public modal: NgbActiveModal) {}

  get f() {
    return this.addServiceForm.controls;
  }

  ngOnInit(): void {
    this.addServiceForm = this.formBuilder.group({
      name: [this.name , [Validators.required]],
      domain: [this.domain, [Validators.required]]
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
