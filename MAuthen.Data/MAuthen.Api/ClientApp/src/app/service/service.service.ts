import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Service } from '../models/service-models/service.model';

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
    add(service: Service) : Observable<Service>{
        return this.http.post<any>("/service", service);
    }
    update(service: Service):Observable<any>{
        return this.http.put<any>("/service", service);
    }
    delete(id: string): Observable<any>{
        return this.http.delete("/service/"+id)
    }
}