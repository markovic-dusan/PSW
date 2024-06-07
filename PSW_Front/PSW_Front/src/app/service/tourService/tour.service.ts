import { HttpClient, HttpHeaders } from '@angular/common/http';
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
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.tourApi, { headers: headers });
  }

  getRewardedTours(): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.tourApi+'awarded', { headers: headers })
  }

  getUserTours(): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours', { headers: headers })
  }

  getRecommendedTours(): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/recommended', { headers: headers })
  }

  archieveTour(tourId: number) {
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    var url = this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours/'+tourId+'/archieve'
    console.log('Archieve tour url: ' ,url)
    return this.http.put(url, {})
  }

  publishTour(tourId: number){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    var url = this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours/'+tourId+'/publish';
    return this.http.put(url, {}, { headers: headers });
  }

  createTour(tour: Tour): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.post<any>(this.localhost+this.userApi+localStorage.getItem('userId')+'/mytours', tour, { headers: headers })
  } 

  addKeypoint(kp: Keypoint): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.post<any>(this.localhost+this.tourApi+kp.tourId+'/keypoints', kp, { headers: headers });
  }

  getKeypoints(tour: Tour): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.tourApi+tour.tourId+'/keypoints', { headers: headers });
  }

  purchaseTour(tour: Tour): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    var tp = new TourPurchase(tour.tourId, localStorage.getItem('userId'), new Date().toISOString());
    return this.http.post<any>(this.localhost+this.tourApi+tp.tourId, tp, { headers: headers })
  }
}


