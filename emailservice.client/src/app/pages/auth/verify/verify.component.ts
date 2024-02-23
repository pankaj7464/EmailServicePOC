import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment'
import { ApiService } from '../../../services/api.service';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'app-verify',
  templateUrl: './verify.component.html',
  styleUrl: './verify.component.css'
})
export class VerifyComponent {
  token!: string;
  BASE_URL = environment.apiUrl

  constructor(private route: ActivatedRoute,private snackBar: MatSnackBar,private apiService:ApiService,private router:Router) { 

  }

  ngOnInit(): void {
    // Subscribe to the route parameters to get the token from the URL
    this.route.params.subscribe(params => {
      this.token = params['token'];
      this.apiService.verify(this.token).subscribe(
        response => {
          // Handle success
          console.log('Registration successful:', response);
          // Display success toast
          this.apiService.showSuccessToast('User verified successfully , Now you can login!');
          this.router.navigate(['/login']);
        },
        error => {
          // Handle error
          console.error('Registration error:', error);
          this.apiService.showSuccessToast('Registration error!');
        }
      );
      console.log('Token:', this.token);
    });
  }
}