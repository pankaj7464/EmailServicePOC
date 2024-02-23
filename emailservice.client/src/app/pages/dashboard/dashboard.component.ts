import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { User } from '../../models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']        
})
export class DashboardComponent implements OnInit {

  displayedColumns = ['id', 'email', 'verifiedAt', 'actions'];
  dataSource!: User[];

  constructor(private apiService: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers() {
    this.apiService.getAllUsers().subscribe(
      response => {
        // Handle success
        this.dataSource = response;
        console.log(this.dataSource);
      },
      error => {
        // Handle error
        console.error('Error while fetching user', error);
      }
    );
  }

  deleteUser(id: string) {
    this.apiService.deleteUsers(id).subscribe(
      () => {
        // Handle success
        console.log('User deleted successfully');
        this.apiService.showSuccessToast('User deleted successfully');
        // Refresh user list
        this.getAllUsers();
      },
      error => {
        // Handle error
        console.error('Error while deleting user:', error);
        // Display error toast
        this.apiService.showSuccessToast('Error deleting user!');
      }
    );
  }
  
  logoutUser() {
    localStorage.removeItem("userdata");
    this.apiService.showSuccessToast('Logout successfully');
    this.router.navigate(['/login']);
  }
}
