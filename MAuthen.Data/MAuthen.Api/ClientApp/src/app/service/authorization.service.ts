import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { User } from '../models/user';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthorizationService {

    private token: BehaviorSubject<User>;
    public currentToken: Observable<User>;

    constructor(private http: HttpClient) {
        this.token = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('Token')));
        this.currentToken = this.token.asObservable();
    }

    public get currentUserValue(): User {
        return this.token.value;
    }

    login(user: Login) {
        return this.http.post<any>(`Authorization/`, user)
            .pipe(map(token => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('Token', JSON.stringify(token));
                this.token.next(token);
                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('User');
        this.token.next(null);
    }
}
