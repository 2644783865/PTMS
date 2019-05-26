import { FormControl, ValidationErrors } from '@angular/forms';

function isNotNullOrEmpty(value: any) {
  return value !== null && value !== '' && value !== undefined;
}

export function requiredIf(otherControlName: string, conditionalFunc: Function = null): ValidationErrors {

  let thisControl: FormControl;
  let otherControl: FormControl;

  return function requiredIfValidate(control: FormControl) {

    if (!control.parent) {
      return null;
    }

    // Initializing the validator.
    if (!thisControl) {
      thisControl = control;
      otherControl = control.parent.get(otherControlName) as FormControl;
      if (!otherControl) {
        throw new Error('requiredIfValidator(): other control is not found in parent group');
      }
      otherControl.valueChanges.subscribe(() => {
        thisControl.updateValueAndValidity();
      });
    }

    if (!otherControl) {
      return null;
    }

    let needToValidate = conditionalFunc ? conditionalFunc() : isNotNullOrEmpty(otherControl.value);
    let isRequired = needToValidate && !isNotNullOrEmpty(thisControl.value);

    if (isRequired) {
      return {
        required: true
      }
    }

    return null;
  }
}
