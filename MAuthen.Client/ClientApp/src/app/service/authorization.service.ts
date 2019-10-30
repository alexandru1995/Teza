import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login.model';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
    providedIn: 'root'
})
export class AuthorizationService{
    constructor(
        private http: HttpClient
    ){}

    login(user: Login): Observable<User>{
        return this.http.post<User>("https://localhost:5001/Authorization",user)
    }
}