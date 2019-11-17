import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

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
        console.log(user);
        return this.http.post<any>(`Authorization/`, user)
            .pipe(map(user => {
                localStorage.setItem('User', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }
    logout() {
        return this.http.get<any>(`Authorization/signout`)
        .pipe(map(data => {
            localStorage.removeItem('User');
            this.currentUserSubject.next(null);
        }))
    }
}
