import { Routes } from '@angular/router';
import { LoginComponent } from '../app/components/login/login.component'; // Adjust the path if necessary
import { HomeComponent } from '../app/components/home/home.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },  // Redirect the root path to login
  { path: '**', redirectTo: '/login' }  // Wildcard route for handling 404 - redirects to login
];
