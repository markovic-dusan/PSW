import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../model/User';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private localhost = 'https://localhost:7147/'; 
  private adminApi = 'api/admin/'

  constructor(private http:HttpClient) { }

  blockUser(user: User){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.put(this.localhost+this.adminApi+user.userName+'/block', {}, {headers: headers})
  }

  unblockUser(user: User){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.put(this.localhost+this.adminApi+user.userName+'/unblock', {}, {headers: headers})
  }

  getMaliciousUsers() : Observable<any> {
    console.log(localStorage.getItem('jwt'))
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost + this.adminApi + 'malicious', { headers: headers });
  }

  getBlockedUsers() : Observable<any> {
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.adminApi+'blocked', {headers: headers})
  }
}
