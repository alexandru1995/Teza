import { Component, Input, Output, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { phoneNumberValidator } from 'src/app/validators/phone-number';

@Component({
  selector: 'app-contact-edit-modal',
  templateUrl: './contact-edit-modal.component.html',
  styleUrls: ['./contact-edit-modal.component.css']
})
export class ContactEditModalComponent implements OnInit {


  @Input() title;
  @Input() label;
  @Input() type;
  @Input() value;
  @Input() message;
  @Input() isConfirmation: boolean = false;

  contactForm: FormGroup;
  name: string ;
  submitted: boolean=false;
  isEmail: boolean = false;
  constructor(
    private formBuilder: FormBuilder,
    public modal: NgbActiveModal) { }
  ngOnInit(): void {
    if (this.type === "email") {
      this.isEmail = true;
      this.contactForm = this.formBuilder.group({
        name: [this.value, [Validators.required, Validators.email]]
      })
    } else {
      this.contactForm = this.formBuilder.group({
        name: [this.value, [Validators.required, phoneNumberValidator]]
      })
    }
  }
  get f() { return this.contactForm.controls; }

  save(){
    this.submitted = true;
    if(this.contactForm.invalid){
      return;
    }
    this.modal.close(this.contactForm.value['name'])
  }
}

