import { Component, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthorizationService } from 'src/app/service/authorization.service';
import { fadeAnimation } from '../animation';
import { DataService } from 'src/app/service/data.service';
import { Router } from '@angular/router';

@Component({
    selector: 'card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.css'],
    animations: [fadeAnimation]
})
export class CardComponent {

    @Output() switch = new EventEmitter<boolean>();

    loginForm: FormGroup;
    error: string;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private authorization: AuthorizationService,
        private data: DataService,
        private readonly router: Router
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
                this.data.changeMessage(data);
                localStorage.setItem("user", JSON.stringify(data));
                this.router.navigateByUrl('/account');

            },
            error => {
                switch (error.status) {
                    case 401:
                        this.error = error.error;
                        setTimeout(()=>this.hidenError(), 3000);
                        break;
                }
            }
        )
    }

    hidenError(){
        this.error = null
    }

    swichToTegistration() {
        this.switch.emit(true)
    }
}
