import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Token } from '../models/token.medel';
import { SimpleUser } from '../models/simpleUser.model';

@Injectable({
    providedIn: 'root'
})
export class AuthorizationService {

    private currentUserSubject: BehaviorSubject<any>;
    public currentUser: Observable<any>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('User')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): any {
        return this.currentUserSubject.value;
    }

    login(user: Login) {
        return this.http.post<SimpleUser>(`Authorization/`, user)
            .pipe(map(user => {
                localStorage.setItem('User', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }

    refresh() {
        var user = JSON.parse(localStorage.getItem('User'));
        return this.http.post<Token>(`Authorization/refresh`, user.tokens)
            .pipe(map(newTokens => {
                localStorage.removeItem('User')
                user.tokens = newTokens;
                localStorage.setItem("User", JSON.stringify(user));
            }));
    }

    logout() {
        return this.http.get<any>(`Authorization/signout`)
            .pipe(map(removeUser => {
                this.currentUserSubject.next(null);
                localStorage.removeItem('User');
            }));
    }
}
