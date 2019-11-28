import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthorizationService } from 'src/app/service/authorization.service';
import { fadeAnimation } from '../animation';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { Login } from 'src/app/models/login.model';

@Component({
    selector: 'card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.css'],
    animations: [fadeAnimation]
})
export class CardComponent implements OnInit {


    @Output() switch = new EventEmitter<boolean>();

    loginForm: FormGroup;
    error: string;
    submitted = false;
    loading = false;
    returnUrl: string;
    serviceName: string;

    constructor(
        private formBuilder: FormBuilder,
        private authorization: AuthorizationService,
        private route: ActivatedRoute,
        private readonly router: Router
    ) {
        this.loginForm = this.formBuilder.group({
            username: ['', [Validators.required]],
            password: ['', [Validators.required]]
        });
    }
    get f() { return this.loginForm.controls; }

    ngOnInit(): void {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/account';
        this.serviceName = this.route.snapshot.paramMap.get("serviceName") || 'MAuthen';
    }

    onSubmit() {
        this.submitted = true;
        if (this.loginForm.invalid) {
            return;
        }
        this.loading = true;
        let user: Login = {
            Password : this.loginForm.controls['password'].value,
            ServiceName  : this.serviceName,
            Username : this.loginForm.controls['username'].value
        };
        this.authorization.login(user)
            .pipe(first())
            .subscribe(
                data => {
                    this.router.navigate([this.returnUrl])
                },
                error => {
                    this.error = error.error;
                    this.loading = false;
                    setTimeout(()=>this.hidenError(), 3000);
                }
            )
    }

    hidenError() {
        this.error = null
    }

    swichToTegistration() {
        this.switch.emit(true)
    }
}
