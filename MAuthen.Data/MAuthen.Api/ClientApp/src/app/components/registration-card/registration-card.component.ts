import { Component, Output, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { phoneNumberValidator, passwordComplexity } from 'src/app/validators/phone-number';
import { UserService } from 'src/app/service/user.service';
import { User } from 'src/app/models/user';

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
    gender: boolean;

    constructor(
        private formBuilder: FormBuilder,
        private user: UserService,
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
            gender: ['', Validators.required]
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
        user.contacts = [{ id: null, email: form.email, phone: form.phoneNumber }];
        user.birthday = form.birthday.month + "/" + form.birthday.day + "/" + form.birthday.year;
        user.password = form.password;
        user.gender = form.gender;
        this.user.add(user).subscribe(
            data => {
                this.swichToAuth();
            },
            error => {
                const username = this.registrationForm.controls['username']
                username.setErrors({
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
