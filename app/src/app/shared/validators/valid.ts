import { ValidatorFn, FormGroup } from '@angular/forms';

export function CheckPassword(pass1: string, pass2: string): ValidatorFn {
    return (form: FormGroup): { [key: string]: any } | null => {
        if (form.controls[pass1].value != form.controls[pass2].value) {
            return { passwordError: 'the password is mismatch' }
        }
        return null;
    }
}