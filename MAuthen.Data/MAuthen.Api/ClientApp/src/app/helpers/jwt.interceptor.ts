import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthorizationService } from '../service/authorization.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthorizationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.authenticationService.isLoggedIn) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${this.authenticationService.getJwtToken()}`
                }
            });
        }
        return next.handle(request);
    }
}