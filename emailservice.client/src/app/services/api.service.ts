import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7089/api/';
  private loading: boolean = false;

  constructor(private http: HttpClient, private snackBar: MatSnackBar) { }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      if (error.status === 400 && error.headers.get('content-type')?.startsWith('text/plain')) {
        // If the response is text/plain, handle it differently
        errorMessage = error.error;
      } else {
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    this.snackBar.open(errorMessage, 'Close', {
      duration: 5000,
      panelClass: ['error-snackbar']
    });
    return throwError(errorMessage);
  }

  private showLoader(): void {
    this.loading = true;
  }

  private hideLoader(): void {
    this.loading = false;
  }

  isLoading(){
    return this.loading;
  }

  private handleLoader<T>(): (source: Observable<T>) => Observable<T> {
    return (source: Observable<T>) => {
      return source.pipe(
        catchError(error => {
          this.hideLoader();
          return this.handleError(error);
        }),
        finalize(() => {
          this.hideLoader();
        })
      );
    };
  }

  showSuccessToast(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['success-snackbar']
    });
  }

  register(data: any): Observable<any> {
    this.showLoader();
    return this.http.post<any>(this.apiUrl + 'User/register', data,{ responseType: 'text' as 'json' }).pipe(
      this.handleLoader()
    );
  }

  verify(token: string): Observable<any> {
    this.showLoader();
    return this.http.post<any>(`${this.apiUrl}User/verify?token=${token}`, null,{ responseType: 'text' as 'json' }).pipe(
      this.handleLoader()
    );
  }

  login(data: any): Observable<any> {
    this.showLoader();
    return this.http.post<any>(this.apiUrl + 'User/login', data,{ responseType: 'text' as 'json' }).pipe(
      this.handleLoader()
    );
  }

  forgotPassword(email: string): Observable<any> {
    this.showLoader();
    return this.http.post<any>(this.apiUrl + `User/forgot-password?email=${email}`, { email },{ responseType: 'text' as 'json' }).pipe(
      this.handleLoader()
    );
  }

  resetPassword(data: any): Observable<any> {
    this.showLoader();
    return this.http.post<any>(this.apiUrl + 'User/reset-password', data,{ responseType: 'text' as 'json' }).pipe(
      this.handleLoader()
    );
  }

   // Method to get all users
   getAllUsers(): Observable<any[]> {
    return this.http.get<User[]>(this.apiUrl + 'User');
  }

  // Method to delete all users
  deleteUsers(id:string): Observable<any> {
    return this.http.delete<any>(this.apiUrl + 'User/'+id);
  }
}
