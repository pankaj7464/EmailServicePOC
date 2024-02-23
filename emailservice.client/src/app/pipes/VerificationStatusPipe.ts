import { Pipe, PipeTransform } from '@angular/core';
import { User } from '../models/User';
@Pipe({
  name: 'verificationStatus'
})
export class VerificationStatusPipe implements PipeTransform {
  transform(verifiedAt:string|null): string {
    if (verifiedAt) {
       var verify =  new Date(verifiedAt);
      return verify.toLocaleString();
    } else {
      return 'Not Verified';
    }
  }
}
