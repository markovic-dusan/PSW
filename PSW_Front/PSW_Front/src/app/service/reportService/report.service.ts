import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, interval, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private localhost = 'https://localhost:7147/'; 
  private userApi = 'api/users/'
  private reportApi = 'api/report/'

  constructor(private http:HttpClient) { }

  generateReportsPeriodically(days: number){
    //interval je u ms
    interval(days * 24 * 60 * 60 * 1000).pipe(switchMap( () => this.generateReports()));

  }

  getAuthorReports(): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/report', { headers: headers })
  }

  generateReports(){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    console.log("generate reports called")
    return this.http.post(this.localhost+this.reportApi, {}, { headers: headers })
  }

  getFailingTours() : Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/failingTours', { headers: headers })
  }
}
