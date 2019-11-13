import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthorizationService } from 'src/app/service/authorization.service';
import { fadeAnimation } from '../animation';
import { DataService } from 'src/app/service/data.service';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { UserService } from 'src/app/service/user.service';

@Component({
    selector: 'card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.css'],
    animations: [fadeAnimation]
})
export class CardComponent implements OnInit {


    @Output() switch = new EventEmitter<boolean>();

    loginForm: FormGroup;
    error : string;
    submitted = false;
    loading = false;
    returnUrl: string;

    constructor(
        private formBuilder: FormBuilder,
        private authorization: AuthorizationService,
        private route: ActivatedRoute,
        private data: DataService,
        private userService: UserService,
        private readonly router: Router
    ) {
        if (this.authorization.currentUserValue) {
            this.router.navigate(['/service']);
        }

        this.loginForm = this.formBuilder.group({
            username: ['', [Validators.required]],
            password: ['', [Validators.required]]
        });
    }
    get f() { return this.loginForm.controls; }

    ngOnInit(): void {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    onSubmit() {
        this.submitted = true;
        if (this.loginForm.invalid) {
            return;
        }

        this.authorization.login(this.loginForm.value)
            .pipe(first())
            .subscribe(
                data => {
                    this.getCurentUser();
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    this.error = error;
                    this.loading = false;
                }
            )
    }

    getCurentUser() {
        this.userService.getUser().subscribe(
            data => {
                this.data.changeMessage(data);
                localStorage.setItem("user", JSON.stringify(data));
            },
            error => {
                this.error = error;
                this.loading = false;
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
