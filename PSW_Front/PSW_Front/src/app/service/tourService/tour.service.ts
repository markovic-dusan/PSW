import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tour } from '../../model/Tour';
import { Keypoint } from '../../model/Keypoint';
import { TourPurchase } from '../../model/TourPurchase';

@Injectable({
  providedIn: 'root'
})
export class TourService {

  private localhost = 'https://localhost:7147/'; 
  private tourApi = 'api/tours/'
  private userApi = 'api/users/'

  constructor(private http: HttpClient) { }

  getAllTours(): Observable<any>{
    return this.http.get(this.localhost+this.tourApi);
  }

  getUserTours(): Observable<any>{
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours')
  }

  getRecommendedTours(): Observable<any>{
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/recommended')
  }

  archieveTour(tourId: number) {
    var url = this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours/'+tourId+'/archieve'
    console.log('Archieve tour url: ' ,url)
    return this.http.put(url, {})
  }

  publishTour(tourId: number){
    var url = this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours/'+tourId+'/publish';
    return this.http.put(url, {});
  }

  createTour(tour: Tour): Observable<any>{
    return this.http.post<any>(this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours', tour)
  } 

  addKeypoint(kp: Keypoint): Observable<any>{
    return this.http.post<any>(this.localhost+this.tourApi+kp.tourId+'/keypoints', kp);
  }

  getKeypoints(tour: Tour): Observable<any>{
    return this.http.get(this.localhost+this.tourApi+tour.tourId+'/keypoints');
  }

  purchaseTour(tour: Tour): Observable<any>{
    var tp = new TourPurchase(tour.tourId, localStorage.getItem('userId'), new Date().toISOString());
    return this.http.post<any>(this.localhost+this.tourApi+tp.tourId, tp)
  }
}


