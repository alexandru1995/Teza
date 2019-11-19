import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanLoad } from '@angular/router';

import { AuthorizationService } from '../service/authorization.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate, CanLoad {

    constructor(
        private router: Router,
        private authenticationService: AuthorizationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (!this.authenticationService.isLoggedIn()) {
            this.router.navigate([''], { queryParams: { returnUrl: state.url } });
        }
        return this.authenticationService.isLoggedIn();
    }
    canLoad() {
        if (!this.authenticationService.isLoggedIn()) {
            this.router.navigate(['']);
        }
        return this.authenticationService.isLoggedIn();
    }
}