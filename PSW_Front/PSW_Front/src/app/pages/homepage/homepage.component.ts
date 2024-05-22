import { Component } from '@angular/core';
import { Tour } from '../../model/Tour';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, NgStyle } from '@angular/common';
import { Router } from '@angular/router';
import { TourService } from '../../service/tourService/tour.service';
import { Observable } from 'rxjs';
import { LoginService } from '../../service/loginService/login.service';
import { Keypoint } from '../../model/Keypoint';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import * as L from 'leaflet';
import 'leaflet-routing-machine';
import Openrouteservice from 'openrouteservice-js';



import { CartServiceService } from '../../service/cartService/cart-service.service';
import { Problem } from '../../model/Problem';
import { ProblemService } from '../../service/problemService/problem.service';

@Component({
  standalone: true,
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css'],
  imports: [
    FormsModule,
    NgFor,
    NgIf,
    MatSnackBarModule,
    NgStyle
  ]
})
export class HomepageComponent {
  selectedTour: Tour = new Tour(0,'','',0,0,[],false,false,false,'');
  keypoints: Keypoint[] = [];
  tours: Tour[] = [];
  awardedToursSelected: boolean = false;
  awardedTours: Tour[] = [];
  filteredTours: Tour[] = [];
  tempTours: Tour[] = [];
  isAuthor: boolean = localStorage.getItem('userRole') === 'author';
  filter: keyof Tour | '' = ''; 
  showMenu: boolean = false;
  map: any;
  problem = new Problem( 0,'', '', '', 0)
  reportToggled: boolean = false;


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

  constructor(private router: Router, private problemService: ProblemService, private tourService: TourService, private loginService: LoginService, private snackbar: MatSnackBar, private cartService: CartServiceService) { }

  ngOnInit(): void {
    console.log(localStorage.getItem('currentUser'));
    console.log(localStorage.getItem('jwt'));
    console.log(localStorage.getItem('username'));
    console.log(localStorage.getItem('userRole'));

    this.loadAllTours();
    if(this.isTourist()){
      this.loadAwardedTours();
    }
    //this.initMap();
  }

  loadAwardedTours(){
    this.tourService.getRewardedTours().subscribe(
      (data: Tour[]) => {
        this.awardedTours = data;
      },
      (error) => {
        console.error('Error getting tours:', error);
      }
    );
  }

  initMap(kp: Keypoint): void {
    console.log('initMap called')
    this.map = L.map('map').setView([kp.latitude, kp.longitude], 15);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    }).addTo(this.map);
    
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
    this.selectedTour = new Tour(0,'','',0,0,[],false,false,false,'');
    this.resetFilters();
    this.resetSelectedTour();
    this.loadRecommendedTours();
    this.recommendedToursSelected = true;
    this.allToursSelected = false;
    this.myToursSelected = false;
    console.log('Showing Recommended Tours');
  }

  showMyTours() {
    this.selectedTour = new Tour(0,'','',0,0,[],false,false,false,'');
    this.resetFilters();
    this.resetSelectedTour()
    this.loadMyTours();
    this.allToursSelected = false;
    this.recommendedToursSelected = false;
    this.myToursSelected = true;
    console.log('Showing My Tours');
  }

  showAllTours() {
    this.selectedTour = new Tour(0,'','',0,0,[],false,false,false,'');
    this.resetFilters();
    this.resetSelectedTour()
    this.loadAllTours();
    this.allToursSelected = true;
    this.toggleAwardedTours(false)
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
    this.tourService.publishTour(tour.tourId).subscribe(
      () => {
        this.showMyTours()
        this.applyFilter()
        console.log('Tour published');
      },
      (error) => {
        this.showNotification('Error publishing tour. At least 2 keypoints needed!');
        console.error('Error publishing tour:', error);
      }
    );
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
    this.reportToggled = false;
    this.selectedTour = tour;
    this.tourService.getKeypoints(tour).subscribe(
      (data: Keypoint[]) => {
        this.keypoints = data;
        if(this.keypoints.length > 0){
          this.displayKeypointsOnMap(data);
        }
      },
      (error) => {
        console.error('Error getting tours:', error);
      }
    ); 
  }

  displayKeypointsOnMap(keypoints: Keypoint[]): void {
    // Inicijalizujemo novu mapu
    if (this.map) {
      this.map.remove();
  }
    const map = L.map('map').setView([keypoints[0].latitude, keypoints[0].longitude], 15);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {}).addTo(map);
    

    // Obrisi prethodno
    if (this.map) {
        this.map.eachLayer((layer: any) => {
            if (!(layer instanceof L.TileLayer)) {
                this.map.removeLayer(layer);
            }
        });
    }
    this.map = map; // Postavljamo referencu na novu mapu

    // Convers
    const latLngs: L.LatLng[] = keypoints.map((kp: Keypoint) => {
        return L.latLng(kp.latitude, kp.longitude);
    });

    // Sa profilom za pesake
    const osrmv1 = new L.Routing.OSRMv1({
        serviceUrl: 'http://router.project-osrm.org/route/v1',
        profile: 'foot' // Profil za pesake
    });

    // Iscrtavanje
    L.Routing.control({
        router: osrmv1,
        waypoints: latLngs,
        routeWhileDragging: false,
    }).addTo(map);

    // Markeri
    const markers = keypoints.map((kp: Keypoint) => {
        const popupContent = `
            <div class="leaflet-popup-content">
                <h3>${kp.name}</h3>
                <p>${kp.description}</p>
                <img src="${kp.imageUrl}" alt="Keypoint Image" style="max-width: 100%; height: auto;">
            </div>
        `;

        // Marker i popup
        return L.marker([kp.latitude, kp.longitude]).bindPopup(popupContent);
    });

    // Dodavanje markera na mapu
    markers.forEach((marker) => {
        marker.addTo(map);
    });
}


  addToCart(tour: Tour) {
    this.cartService.addToCart(tour)
  }

  newTour(){
    this.router.navigate(['/newtour'])
  }

  addKeypoint(tour: Tour){
    this.router.navigate(['/keypoint'], { queryParams: { tourId: tour.tourId } });
  }

  private showNotification(message: string): void {
    this.snackbar.open(message, 'Close', {
      duration: 3000, 
      verticalPosition: 'top', 
      horizontalPosition: 'center' 
    });
  }

  goToCart(){
    this.router.navigate(['/cart'])
  }

  goToReports(){
    this.router.navigate(['/reports'])
  }

  toggleAwardedTours(b : boolean){
    this.awardedToursSelected = b;
    if(this.awardedToursSelected){
      this.tempTours = this.filteredTours;
      this.filteredTours = this.awardedTours;
    }else{
      this.filteredTours = this.tempTours;
    }
  }

  goToProblems(){
    this.router.navigate(['/problems'])
  }

  reportProblem(){
    this.reportToggled = false;
    this.problem.tourId = this.selectedTour.tourId
    if(localStorage.getItem('userId') != (null || '')){
      this.problem.touristId = localStorage.getItem('userId');
    }

    this.problemService.reportProblem(this.problem).subscribe(response =>{
      this.showNotification('Problem reported')
      console.log(response);
    }, error => {
      console.error(error);
    })
    this.problem = new Problem(0, '', '', '', 0)
  }

  toggleReport(){
    this.reportToggled = true;
  }
}