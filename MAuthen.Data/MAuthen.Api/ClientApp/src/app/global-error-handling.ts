import { Injectable, ErrorHandler, Injector } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

@Injectable()
export class GlobalErrorHandling implements ErrorHandler {
    
    constructor(
        private injector: Injector
    ) { }

    handleError(error: any) {
        const route = this.injector.get(Router)

        if (error instanceof HttpErrorResponse) {
            switch (error.status) {
                case 401: {
                    console.log("Access denide asdasdasd")
                    break;
                }
                // case 404: {
                //     //route.navigate(['/notFound']);
                // }
            }
            console.log("An error occured: ", error);
        }
    }
}