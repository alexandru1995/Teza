import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, switchMap, filter } from 'rxjs/operators';

import { AuthorizationService } from '../service/authorization.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(
        private authenticationService: AuthorizationService,
        private readonly router: Router
    ) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(request).pipe(
            catchError(err => {
                
                if (err.status === 401) {
                    if (err.headers.has('Token-Expired')) {
                        return this.handle401Error(request, next);
                    }if(err.headers.has('Invalid-refresh')) {
                        localStorage.clear();
                        this.authenticationService.currentUserSubject.next(null);
                        this.router.navigate([''])
                    }
                    else {
                        localStorage.clear();
                        return throwError(err);
                    }
                }
                //const error = err.error || err.statusText;
                return throwError(err);
            }))
    }
    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            return this.authenticationService.refresh().pipe(
                switchMap((token: any) => {
                    this.isRefreshing = false;
                    this.refreshTokenSubject.next(token.accessToken);
                    return next.handle(this.addToken(request, token.accessToken));
                },));

        } else {
            return this.refreshTokenSubject.pipe(
                filter(token => token != null),
                switchMap(accessToken => {
                    return next.handle(this.addToken(request, accessToken.toString()));
                }));
        }
    }

    private addToken(request: HttpRequest<any>, token: string) {
        return request.clone({
            setHeaders: {
                'Authorization': `Bearer ${token}`
            }
        });
    }
}
