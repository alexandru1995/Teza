import { AbstractControl } from '@angular/forms';

export function phoneNumberValidator(control: AbstractControl): { [key: string]: any } | null {
    const valid = /^(0+([0-9]){8})$/.test(control.value);
    console.log(valid)
    return valid ? null : {
        invalidNumber: {
            valid: false,
            value: control.value
        }
    };
}