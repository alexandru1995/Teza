import { AbstractControl } from '@angular/forms';

export function phoneNumberValidator(control: AbstractControl): { [key: string]: any } | null {
    const valid = /^(0+([0-9]){8})$/.test(control.value);
    return valid ? null : {
        invalidNumber: {
            valid: false,
            value: control.value
        }
    };
}

export function passwordComplexity(control: AbstractControl): { [key: string]: any } | null {
    const validUppercase = /^((?:(?=.*[A-Z]))).{1,}$/.test(control.value);
    const validNumber = /^((?=.*\d)).{1,}$/.test(control.value);
    const lengthValidation = /^.{6,}$/.test(control.value)

    if(!validUppercase)
    {
        return {
            invalidPassword: {
                valid: false, 
                message:"The password must contain at least one capital letter",
                value: control.value
            }
        }
    }
    else if(!validNumber)
    {
        return {
            invalidPassword: {
                valid: false, 
                message:"The password must contain at least one number",
                value: control.value
            }
        }
    }
    else if(!lengthValidation){
        return {
            invalidPassword: {
                valid: false, 
                message:"The password must contain at least 6 characters",
                value: control.value
            }
        }
    }else{
        return null;
    }
}