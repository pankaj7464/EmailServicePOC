import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../services/api.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  token!: string;
  resetPasswordForm!: FormGroup;
  constructor(private route: ActivatedRoute, private fb: FormBuilder, private snackBar: MatSnackBar, private apiService: ApiService,private router:Router) {
    this.route.params.subscribe(params => {
      this.token = params['token'];
      console.log(this.token)
    }
    )
  }


  ngOnInit(): void {
    this.resetPasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
  }

  onResetPassword() {
    if (this.resetPasswordForm.valid) {
      // Call your reset password API or service here
      console.log('Resetting password...');
      console.log(this.resetPasswordForm.value)
      console.log(this.token)
      console.log({ ...this.resetPasswordForm.value, token: this.token })
      this.apiService.resetPassword({ ...this.resetPasswordForm.value, token: this.token }).subscribe(
        response => {
          // Handle success
          console.log('Login successful:', response);
          // Display success toast
          this.router.navigate(['/login']);
          this.apiService.showSuccessToast('Your Password has been successfully reset!');
        },
        error => {
          // Handle error
          console.error('Registration error:', error);
        }
      )

    } else {


    }
  }


}
