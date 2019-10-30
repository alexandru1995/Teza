import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthorizationService } from 'src/app/service/authorization.service';
import { CompileShallowModuleMetadata } from '@angular/compiler';

@Component({
    selector: 'card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.css'],
})
export class CardComponent {

    @Output() switch = new EventEmitter<boolean>()

    loginForm: FormGroup;
    error: string;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private authorization: AuthorizationService
    ) {
        this.loginForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required]]
        });
    }
    get f() { return this.loginForm.controls; }
    onSubmit() {
        this.submitted = true;
        if (this.loginForm.invalid) {
            return;
        }
        this.authorization.login(this.loginForm.value).subscribe(
            data => {
                console.log(data)
            },
            error => {
                switch (error.status) {
                    case 401:
                        this.error = error.error;
                        break;
                    default:
                        break;
                }
                console.log(error)
                //this.error = error.error;
            }
        )
    }

    swichToTegistration() {
        this.switch.emit(true)
    }
}
