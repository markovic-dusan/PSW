import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LogInInfo } from '../../model/LogInInfo';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl = 'https://localhost:7147/api/auth/login'; 


  constructor(private http: HttpClient) { }

  login(username: string, password: string){
    var body = { loginUserName: username, loginPassword: password }
    return this.http.post<LogInInfo>(this.apiUrl, body);
  }

  loginSetUser(loginInfo: LogInInfo){
    localStorage.setItem('currentUser', JSON.stringify(loginInfo));
    localStorage.setItem('jwt', loginInfo.token);
    localStorage.setItem('username', loginInfo.username);
    localStorage.setItem('userRole', loginInfo.role);
    localStorage.setItem('userId', loginInfo.userId);
    localStorage.setItem('email', loginInfo.email)
  }

  getHeaders(){
    if(this.isUserLoggedIn()){
      const userToken = localStorage.getItem('token');
      const headers = {
          'Content-Type' : 'application/json',
          Authorization: 'Bearer ' + userToken,
      };
      return headers;
    }else{
      const headers = {
        'Content-Type' : 'application/json'
      };
      return headers;
    }
  }

  loggedIn(){
    return !!localStorage.getItem('userRole');
  }

  adminAccess(){
    var role = localStorage.getItem('userRole');
    if(role == 'admin'){
      return true;
    }
    return false;
  }

  authorAccess(){
    var role = localStorage.getItem('userRole');
    if(role == 'author'){
      return true;
    }
    return false;
  }

  logout(){
    var user = new LogInInfo();
    window.location.href = '/login';
    this.loginSetUser(new LogInInfo());
  }

  getCurrentUser(): LogInInfo{
    let currentUser = localStorage.getItem('currentUser');
    if(currentUser != null){
      return JSON.parse(localStorage.getItem('currentUser')!);
    }else{
      return new LogInInfo();
    }
  }

  isUserLoggedIn(){
    let currentUser = this.getCurrentUser();
    if(currentUser == null || currentUser.token==undefined){
      return false;
    }
    return true;
  }

}
