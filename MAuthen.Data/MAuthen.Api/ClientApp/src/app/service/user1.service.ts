import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService1 {
    constructor(
        private http: HttpClient
      ) { }

      getUser(): Observable<User>{
        return this.http.get<User>("https://localhost:5001/user/88B81B68-3488-4716-D97F-08D766CC4B25");
      }
}