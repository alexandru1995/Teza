import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-role-modal',
  templateUrl: './add-role-modal.component.html',
  styleUrls: ['./add-role-modal.component.css']
})
export class AddRoleModalComponent implements OnInit {

  @Input() title;
  @Input() name: string;

  submitted: boolean = false;
  addRoleForm: FormGroup;
  
  constructor(
    private formBuilder: FormBuilder,
    public modal: NgbActiveModal) {}

  get f() {
    return this.addRoleForm.controls;
  }

  ngOnInit(): void {
    this.addRoleForm = this.formBuilder.group({
      name: [this.name , [Validators.required]]
    })
  }

  save() {
    this.submitted = true;
    if (this.addRoleForm.invalid) {
      return;
    }
    this.modal.close(this.addRoleForm.value)
  }

}
