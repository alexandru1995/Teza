import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthorizationService } from '../service/authorization.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(
        private authenticationService: AuthorizationService,
        private readonly router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
        return next.handle(request).pipe(catchError(err => {
             if (err.status === 401) {
                if(err.headers.has('Token-Expired')){
                    this.authenticationService.refresh()
                }else{
                    this.authenticationService.logout();
                    this.router.navigate(['/']);
                }
            }
            const error = err.error || err.statusText;
            return throwError(error);
        }))
    }
}