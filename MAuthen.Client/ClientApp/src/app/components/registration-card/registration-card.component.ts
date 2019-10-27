import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { phoneNumberValidator } from 'src/app/validators/phone-number';

@Component({
  selector: 'registration-card',
  templateUrl: './registration-card.component.html',
  styleUrls: ['./registration-card.component.css']
})
export class RegistrationCardComponent implements OnInit {

  @Output() switch = new EventEmitter<boolean>();

  registrationForm: FormGroup;

  constructor() {
    this.registrationForm = this.createFormGroup();
  }

  get f() { return this.registrationForm.controls; }

  ngOnInit() {
  }

  swichToAuth() {
    this.switch.emit(true)
  }

  revert() {
    this.registrationForm.reset();
  }
  
  onSubmit() {
    console.log(this.registrationForm.value)
  }

  createFormGroup() {
    return new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      mobile: new FormControl('', [Validators.required, phoneNumberValidator]),
      password: new FormControl('', Validators.required),
      confirmPassword: new FormControl('', Validators.required)
    });
  }
}
