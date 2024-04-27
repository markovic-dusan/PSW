import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TourService } from '../../service/tourService/tour.service';
import { LoginService } from '../../service/loginService/login.service';
import { Tour } from '../../model/Tour';

@Component({
  standalone: true,
  selector: 'app-new-tour',
  imports: [
    FormsModule,
    NgFor,
    NgIf
  ],
  templateUrl: './new-tour.component.html',
  styleUrl: './new-tour.component.css'
})
export class NewTourComponent {
  tourData: Tour  = new Tour(0,'','',0,0,[],true,false,false, '');
  interests: string[] = ['ADVENTURE', 'CHILL', 'SPIRITUAL', 'SIGHTSEEING'];
  selectedInterests: number[] = [];

  constructor(private router: Router, private tourService: TourService, private loginService: LoginService) { }

  toggleInterest(interestIndex: number){
    const index = this.selectedInterests.indexOf(interestIndex);
    if (index === -1){
      this.selectedInterests.push(interestIndex);
    } else{
      this.selectedInterests.splice(index, 1);
    }
  }

  onSubmit(){
    if(localStorage.getItem('userId') != (null || '')){
      this.tourData.authorId = localStorage.getItem('userId');
    }
    this.tourData.interests = this.selectedInterests.map(interestIndex => ({ interestValue: interestIndex }));
    this.tourService.createTour(this.tourData)
      .subscribe(response => {
        console.log(response);
        this.router.navigate(['/homepage']); 
      }, error => {
        console.error(error);
      });
  }

  homepage(){
    this.router.navigate(['/homepage'])
  }

}
