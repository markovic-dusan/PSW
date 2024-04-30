import { Component } from '@angular/core';
import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { HomepageComponent } from './pages/homepage/homepage.component';
import { NewTourComponent } from './pages/new-tour/new-tour.component';
import { NewKeypointComponent } from './new-keypoint/new-keypoint.component';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'homepage', component: HomepageComponent},
    { path: 'newtour', component: NewTourComponent},
    { path: 'keypoint', component: NewKeypointComponent}
  ];
