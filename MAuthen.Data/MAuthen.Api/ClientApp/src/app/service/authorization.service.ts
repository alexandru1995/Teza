import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Login } from '../models/login.model';
import { Observable, throwError } from 'rxjs';
import { User } from '../models/user';
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthorizationService {
    constructor(
        private http: HttpClient
    ) { }

    login(user: Login): Observable<User> {
        return this.http.post<User>("Authorization/", user)
    }
}