import { Component } from '@angular/core';
import { Tour } from '../../model/Tour';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { TourService } from '../../service/tourService/tour.service';
import { Observable } from 'rxjs';
import { LoginService } from '../../service/loginService/login.service';

@Component({
  standalone: true,
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css'],
  imports: [
    FormsModule,
    NgFor,
    NgIf
  ]
})
export class HomepageComponent {
  selectedTour: Tour = new Tour(0,'','',0,0,[],false,false,false,'');
  tours: Tour[] = [];
  filteredTours: Tour[] = [];
  isAuthor: boolean = localStorage.getItem('userRole') === 'author';
  filter: keyof Tour | '' = ''; 
  showMenu: boolean = false;

  interestMapping: { [key: number]: string } = {
    0: 'ADVENTURE',
    1: 'CHILL',
    2: 'SPIRITUAL',
    3: 'SIGHTSEEING'
  };

  interestColorMapping: { [key: string]: string } = {
    ADVENTURE: '#FF5733',
    CHILL: '#33FF57',
    SPIRITUAL: '#5733FF',
    SIGHTSEEING: '#FFFF33'
  };

  allToursSelected: boolean = true;
  myToursSelected: boolean = false;
  recommendedToursSelected: boolean = false;
  selectedDifficulty: number =5 ;

  constructor(private router: Router, private tourService: TourService, private loginService: LoginService) { }

  ngOnInit(): void {
    console.log(localStorage.getItem('currentUser'));
    console.log(localStorage.getItem('jwt'));
    console.log(localStorage.getItem('username'));
    console.log(localStorage.getItem('userRole'));

    this.loadAllTours();
  }

  loadAllTours() {
    this.tourService.getAllTours().subscribe(
      (data: Tour[]) => {
        this.tours = data;
        this.applyFilter();
      },
      (error) => {
        console.error('Error getting tours:', error);
      }
    );
  }

  loadMyTours(){
    this.tourService.getUserTours().subscribe(
      (data: Tour[]) => {
        this.tours = data;
        this.applyFilter();
      },
      (error) => {
        console.error('Error getting tours:', error);
      }
    );
  }

  loadRecommendedTours(){
    this.tourService.getRecommendedTours().subscribe(
      (data: Tour[]) => {
        this.tours = data;
        this.applyFilter();
      },
      (error) => {
        console.error('Error getting tours:', error);
      }
    );
  }

  showRecommendedTours(){
    this.resetFilters();
    this.resetSelectedTour();
    this.loadRecommendedTours();
    this.recommendedToursSelected = true;
    this.allToursSelected = false;
    this.myToursSelected = false;
    console.log('Showing Recommended Tours');
  }

  showMyTours() {
    this.resetFilters();
    this.resetSelectedTour()
    this.loadMyTours();
    this.allToursSelected = false;
    this.recommendedToursSelected = false;
    this.myToursSelected = true;
    console.log('Showing My Tours');
  }

  showAllTours() {
    this.resetFilters();
    this.resetSelectedTour()
    this.loadAllTours();
    this.allToursSelected = true;
    this.myToursSelected = false;
    this.recommendedToursSelected = false;
    console.log('Showing All Tours');
  }

  logout(){
    this.loginService.logout();
  }

  goToProfile(){
  }

  // filtriranje za autora
  setFilter(attr: keyof Tour | '') {
    this.filter = attr;
    this.applyFilter();
  }
  applyFilter() {
    if (!this.filter) {
      this.filteredTours = this.tours;
    } else {
      this.filteredTours = this.tours.filter((tour: Tour) => this.filter && tour.hasOwnProperty(this.filter) && tour[this.filter]);
    }
    this.resetSelectedTour()
  }
  resetFilters() {
    this.applyFilterTourist(5)
    this.filter = '';
    this.applyFilter();
  }
  //filtriranje za korisnika
  setFilterTourist(difficulty: number) {
    this.applyFilterTourist(difficulty);
  }  
  applyFilterTourist(difficulty: number) {
    this.selectedDifficulty = difficulty;
    if (this.selectedDifficulty === 5) {
      this.filteredTours = this.tours;
    } else {
      this.filteredTours = this.tours.filter(tour => tour.difficulty === difficulty);
    }
  }

  resetSelectedTour(){
    this.selectedTour = new Tour(0,'','',0,0,[],false,false,false,'');
  }

  publishTour(tour: Tour) {
  }
  
  archiveTour(tour: Tour) {
    console.log('Archieve tour')
    this.tourService.archieveTour(tour.tourId).subscribe(
      () => {
        this.showMyTours()
        this.applyFilter()
        console.log('Tour archieved');
      },
      (error) => {
        console.error('Error archiving tour:', error);
      }
    );
    console.log('Tour archieved')
    
  }

  isTourist() {
    return localStorage.getItem('userRole') === 'tourist';
  }

  showDetails(tour: Tour) {
    this.selectedTour = tour;
  }

  addToCart(tour: Tour) {}
}