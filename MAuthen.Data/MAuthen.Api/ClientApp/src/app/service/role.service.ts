import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '../models/role-models/role.model';
import { ChangeRole } from '../models/role-models/change-user-role';

@Injectable({
    providedIn: 'root'
})
export class RoleService{
    constructor(
        private http: HttpClient
    ) { }
    add(name: string, serviceId:string): Observable<any>{
        return this.http.post<any>("/role",{name,serviceId});
    }
    get(serviceId: string): Observable<Role[]>{
        return this.http.get<Role[]>("/role/ServiceRoles/"+serviceId)
    }
    chenge(role: ChangeRole):Observable<any>{
        return this.http.post<any>("/role/Change",role)
    }
}