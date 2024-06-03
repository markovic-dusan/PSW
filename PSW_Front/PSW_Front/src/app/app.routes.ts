import { Component } from '@angular/core';
import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { HomepageComponent } from './pages/homepage/homepage.component';
import { NewTourComponent } from './pages/new-tour/new-tour.component';
import { NewKeypointComponent } from './pages/new-keypoint/new-keypoint.component';
import { CartComponent } from './pages/cart/cart.component';
import { ReportsComponent } from './pages/reports/reports.component';
import { ProblemsComponent } from './pages/problems/problems.component';
import { MaliciousUsersComponent } from './pages/malicious-users/malicious-users.component';
import { BlockedUSersComponent } from './pages/blocked-users/blocked-users.component';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'homepage', component: HomepageComponent},
    { path: 'newtour', component: NewTourComponent},
    { path: 'keypoint', component: NewKeypointComponent},
    { path: 'cart', component: CartComponent},
    { path: 'reports', component: ReportsComponent},
    { path: 'problems', component: ProblemsComponent},
    {path: 'malicious', component: MaliciousUsersComponent},
    {path: 'blocked', component: BlockedUSersComponent}
  ];
