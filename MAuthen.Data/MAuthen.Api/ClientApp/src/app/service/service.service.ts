import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Service } from '../models/service-models/service.model';
import { UserServiceModel } from '../models/service-models/user-service.model';
import { FullService } from '../models/service-models/full-service.model';

@Injectable({
    providedIn: 'root'
})
export class ServiceService {
    constructor(
        private http: HttpClient
    ) { }
    get() : Observable<Service[]>{
        return this.http.get<Service[]>("/service/GetServices");
    }
    add(service: FullService) : Observable<Service>{
        return this.http.post<any>("/service", service);
    }
    update(service: FullService):Observable<any>{
        return this.http.put<any>("/service", service);
    }
    delete(id: string): Observable<any>{
        return this.http.delete("/service/"+id)
    }
    getUsers(id:string):Observable<UserServiceModel[]>{
        return this.http.get<UserServiceModel[]>("/service/"+id);
    }
    blockUser(userId:string,serviceId:string): Observable<any>{
        return this.http.get<any>("/service/BlockUser/"+serviceId+"/"+userId);
    }
    unBlockUser(userId:string,serviceId:string): Observable<any>{
        return this.http.get<any>("/service/UnBlockUser/"+serviceId+"/"+userId);
    }
}