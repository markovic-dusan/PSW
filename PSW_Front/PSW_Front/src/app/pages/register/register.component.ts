import { UserService } from './../../service/userService/user.service';
import { Component, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../../model/User';
import { NgFor } from '@angular/common';
import { Router } from '@angular/router';


@Component({
  standalone:true,
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [
    FormsModule,
    NgFor,
  ]
})

export class RegisterComponent {
  confirmPassword: string = '';
  userData: User = new User('', '', '', 1, '', '', [], '');
  interests: string[] = ['ADVENTURE', 'CHILL', 'SPIRITUAL', 'SIGHTSEEING'];
  selectedInterests: number[] = [];

  constructor(private router: Router, private userService: UserService) { }
  
  toggleInterest(interestIndex: number) {
    const index = this.selectedInterests.indexOf(interestIndex);
    if (index === -1) {
      this.selectedInterests.push(interestIndex);
    } else {
      this.selectedInterests.splice(index, 1);
    }
  }
  
  onSubmit() {
    this.userData.interests = this.selectedInterests.map(interestIndex => ({ interestValue: interestIndex })); // Izmene su ovde
    this.userData.normalizedUserName = this.userData.userName;
    this.userService.registerUser(this.userData)
      .subscribe(response => {
        console.log(response);
        this.router.navigate(['/login']); 
      }, error => {
        console.error(error);
      });
  }
}