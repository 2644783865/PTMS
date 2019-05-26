import { matchOther } from './match-other.validator';
import { requiredIf } from './required-if.validator';

export class CustomValidators {
  static matchOther = matchOther;
  static requiredIf = requiredIf;
}
