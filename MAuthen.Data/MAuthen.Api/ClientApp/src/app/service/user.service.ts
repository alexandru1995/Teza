import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Contact } from '../models/contact.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(
    private http: HttpClient
  ) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('User')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }


  add(user: User): Observable<User> {
    return this.http.post<User>("/user", user);
  }

  getUser(): Observable<User> {
    return this.http.get<User>("/user");
  }

  addContact(contact: Contact): Observable<Contact[]> {
    console.log(contact)
    return this.http.post<Contact[]>("/user/AddContact", contact);
  }
}