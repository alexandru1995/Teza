import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { phoneNumberValidator, passwordComplexity } from 'src/app/validators/phone-number';
import { UserService } from 'src/app/service/user.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { User } from 'src/app/models/user';
import { Contact } from 'src/app/models/contact.model';

@Component({
    selector: 'registration-card',
    templateUrl: './registration-card.component.html',
    styleUrls: ['./registration-card.component.css']
})
export class RegistrationCardComponent {

    @Output() switch = new EventEmitter<boolean>();
    @Output() registrationError = new EventEmitter<any>();

    registrationForm: FormGroup;
    submitted = false;
    gender:boolean;
    
  model: NgbDateStruct;

    constructor(
        private formBuilder: FormBuilder,
        private user: UserService
    ) {
        this.registrationForm = this.formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            username: ['', Validators.required],
            birthday: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            phoneNumber: ['', [Validators.required, phoneNumberValidator]],
            password: ['', [Validators.required, passwordComplexity]],
            confirmPassword: ['', Validators.required],
            gender:['', Validators.required]
        }, { validator: this.checkPasswords });
    }


    get f() { return this.registrationForm.controls; }

    swichToAuth() {
        this.switch.emit(true)
    }

    revert() {
        this.registrationForm.reset();
    }

    onSubmit() {
        this.submitted = true;
        if (this.registrationForm.invalid) {
            return;
        }
        var form = this.registrationForm.value;
        var user = new User();
        
        user.firstName = form.firstName;
        user.lastName = form.lastName;
        user.userName = form.username;
        user.contacts = [{email :form.email, phone: form.phoneNumber}];
        user.birthday = form.birthday.month + "/" + form.birthday.day + "/" + form.birthday.year;
        user.password = form.password;
        user.gender = form.gender;
        console.log(user)
        this.user.add(user).subscribe(
        data => {
            console.log(data)
        },
        error => {
            //TODO set error to username
            const email = this.registrationForm.controls['email']
            email.setErrors({
                fildExist:
                {
                    valid: false,
                    message: error.error
                }
            })
        })
    }

    checkPasswords(group: FormGroup) {
        let pass = group.controls.password.value
        let confirmPass = group.controls.confirmPassword.value;
        return (pass === confirmPass) ? null : { notSame: true }
    }
}
