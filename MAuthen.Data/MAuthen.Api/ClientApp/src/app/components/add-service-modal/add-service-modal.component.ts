import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-service-modal',
  templateUrl: './add-service-modal.component.html',
  styleUrls: ['./add-service-modal.component.css']
})
export class AddServiceModalComponent {

  @Input() title;

  submitted: boolean = false;
  addServiceForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public modal: NgbActiveModal) {
    this.addServiceForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      domain: ['', [Validators.required]]
    })
  }

  get f() { return this.addServiceForm.controls; }

  save() {
    this.submitted = true;
    if(this.addServiceForm.invalid){
      return;
    }
    this.modal.close(this.addServiceForm.value)
  }

}
