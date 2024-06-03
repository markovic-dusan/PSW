import { HttpClient } from '@angular/common/http';
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
    return this.http.put(this.localhost+this.adminApi+user.userName+'/block', {})
  }

  unblockUser(user: User){
    return this.http.put(this.localhost+this.adminApi+user.userName+'/unblock', {})
  }

  getMaliciousUsers() : Observable<any> {
    return this.http.get(this.localhost+this.adminApi+'malicious')
  }

  getBlockedUsers() : Observable<any> {
    return this.http.get(this.localhost+this.adminApi+'blocked')
  }
}
