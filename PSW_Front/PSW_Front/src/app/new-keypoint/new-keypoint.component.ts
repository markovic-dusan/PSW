import { NgFor, NgIf } from '@angular/common';
import { Component, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TourService } from '../service/tourService/tour.service';
import { Keypoint } from '../model/Keypoint';

@Component({
  selector: 'app-new-keypoint',
  standalone: true,
  imports: [
    FormsModule,
    NgFor,
    NgIf
  ],
  templateUrl: './new-keypoint.component.html',
  styleUrl: './new-keypoint.component.css'
})
export class NewKeypointComponent {
  kpData: Keypoint  = new Keypoint(0,'','','',0,0,0)
  tourId = 0
  constructor(private route: ActivatedRoute, private router: Router, private tourService: TourService) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const tourId = params['tourId'];
      this.tourId = tourId
    });
  }

  createKeypoint(){
    this.kpData.tourId = this.tourId
    this.tourService.addKeypoint(this.kpData)
      .subscribe(response => {
        console.log(response);
        this.router.navigate(['/homepage']); 
      }, error => {
        console.error(error);
      });
  }
}
