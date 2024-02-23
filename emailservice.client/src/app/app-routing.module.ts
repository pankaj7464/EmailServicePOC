import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './pages/auth/register/register.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { ResetPasswordComponent } from './pages/auth/reset-password/reset-password.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AuthGuard } from './services/AuthGuard';
import { VerifyComponent } from './pages/auth/verify/verify.component';

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'verification/:token', component: VerifyComponent },
  { path: 'verification-success', component: VerifyComponent },
  { path: 'login', component: LoginComponent },
  { path: 'resetpassword/:token', component: ResetPasswordComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },

  // Default route to login
  { path: '', redirectTo: '/login', pathMatch: 'full' }, 
  // Redirect to login for any other unmatched routes
  { path: '**', redirectTo: '/login' } 
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
