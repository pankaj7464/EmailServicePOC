import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../environments/environment'
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../../../services/api.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registrationForm!: FormGroup;
  BASE_URL = environment.apiUrl
  constructor(private formBuilder: FormBuilder,private snackBar: MatSnackBar,private apiService:ApiService,private router:Router) { }

  ngOnInit(): void {
    this.registrationForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
  }


  onSubmit() {
    if (this.registrationForm.valid) {
      console.log(this.registrationForm.value)
      this.apiService.register(this.registrationForm.value).subscribe(
        response => {
          // Handle success
          console.log('Registration successful:', response);
          // Display success toast
          this.apiService.showSuccessToast('User registered successfully,Please verify it on your email!');
          this.router.navigate(['/login']);
        },
        error => {
          // Handle error
          console.error('Registration error:', error);
        }
      );
  }
}
}
