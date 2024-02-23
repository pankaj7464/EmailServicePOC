import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../../services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginForm: any;
  forgotPasswordClicked: boolean = false;
  constructor(private formBuilder: FormBuilder, private snackBar: MatSnackBar, private apiService: ApiService, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {

      // Add registration logic here

      this.apiService.login(this.loginForm.value).subscribe(
        response => {
          // Handle success
          console.log('Login successful:', response);
          // Display success toast
          localStorage.setItem("userdata",JSON.stringify(response))
          this.apiService.showSuccessToast('Congratulations you have login successfully!');
          this.router.navigate(['/dashboard']);
        },
        error => {
          // Handle error
          console.error('Registration error:', error);
        }
      );
    } else {
      // Mark all fields as touched to display validation errors
      this.loginForm.markAllAsTouched();
    }
  }

  forgotPassword() {
    console.log(this.loginForm.value?.email)
    if (this.loginForm.value?.email)
      this.apiService.forgotPassword(this.loginForm.value.email).subscribe(
        response => {
          // Handle success
          console.log('Login successful:', response);
          // Display success toast
          this.apiService.showSuccessToast('We have sended forgot password link to your email');
        },
        error => {
          // Handle error
          console.error('Registration error:', error);
        }
      )
      else{
        this.apiService.showSuccessToast('Please enter email address');
      }
  }
  
}
