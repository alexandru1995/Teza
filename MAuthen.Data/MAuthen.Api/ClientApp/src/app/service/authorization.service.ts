import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, tap, mapTo } from 'rxjs/operators';
import { Token } from '../models/token.medel';
import { SimpleUser } from '../models/simpleUser.model';

@Injectable({
    providedIn: 'root'
})
export class AuthorizationService {

    public currentUserSubject: BehaviorSubject<any>;
    public currentUser: Observable<any>;


    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<any>(
            JSON.parse(localStorage.getItem('User'))
        );
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): any {
        return this.currentUserSubject.value;
    }

    login(data: Login): Observable<boolean> {

        return this.http.post<SimpleUser>(`Authorization/`, data)
            .pipe(
                tap((user: SimpleUser) => this.doLoginUser(user)),
                mapTo(true)
            );
    }

    remoteLogin(data: Login): Observable<any> {
        return this.http.post('Authorization/RemoteSignin', data);
    }

    logout() {
        return this.http.get<any>(`Authorization/signout`)
            .pipe(map(() => {
                this.currentUserSubject.next(null);
                localStorage.clear();
            }));
    }

    isLoggedIn() {
        return !!this.getJwtToken();
    }

    refresh() {
        var refreshToken = localStorage.getItem('refreshToken');
        var accessToken = localStorage.getItem('accessToken');
        return this.http.post<Token>(`Authorization/refresh`, { 'refreshToken': refreshToken, 'accessToken': accessToken })
            .pipe(
                tap(((tokens: Token) => {
                    this.storeTokens(tokens)
                })
                )
            );
    }

    private doLoginUser(user: SimpleUser) {
        this.storeTokens(user.tokens)
        user.tokens = null;
        localStorage.setItem("User", JSON.stringify(user));
        this.currentUserSubject.next(user);
    }

    private storeTokens(tokens: Token) {
        localStorage.setItem("accessToken", tokens.accessToken)
        localStorage.setItem("refreshToken", tokens.refreshToken)
    }

    getJwtToken() {
        return localStorage.getItem("accessToken");
    }
}
