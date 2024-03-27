import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../../model/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7147/api/users'; 

  constructor(private http: HttpClient) { }

  registerUser(user: User): Observable<any> {
    return this.http.post<any>(this.apiUrl, user);
  }
}