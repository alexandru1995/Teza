import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Token } from '../models/token.medel';

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
        return this.http.post<any>(`Authorization/`, user)
            .pipe(map(user => {
                console.log(user)
                localStorage.setItem('User', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }

    refresh(){
        var tokens = JSON.parse(localStorage.getItem('User'));
        var refresh =  {accessToken : tokens.token, refreshToken : tokens.refreshToken};
        console.log(refresh);
        return this.http.post<Token>(`Authorization/refresh`, refresh)
            .pipe(map(newTokens => {
                localStorage.removeItem('User');
                tokens.accessToken = newTokens.accessToken;
                tokens.refreshToken = newTokens.refreshToken;
                localStorage.setItem("User", JSON.stringify(tokens));
            }));
    }

    logout() {
        this.currentUserSubject.next(null);
        localStorage.removeItem('User');
        return this.http.get<any>(`Authorization/signout`);
    }
}
