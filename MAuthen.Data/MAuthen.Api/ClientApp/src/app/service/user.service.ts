import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient
  ) { }

  add(user: User): Observable<User> {
    console.log(user)
    return this.http.post<User>("https://localhost:5001/user",user);
  }
}
