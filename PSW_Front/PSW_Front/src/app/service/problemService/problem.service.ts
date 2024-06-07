import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Problem } from '../../model/Problem';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProblemService {

  private localhost = 'https://localhost:7147/'; 
  private problemApi = 'api/problem/'
  private userApi = 'api/users/'

  constructor(private http:HttpClient) { }

  reportProblem(problem: Problem){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.post(this.localhost+this.problemApi, problem, { headers: headers });
  }

  getAllProblems(): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.problemApi, { headers: headers })
  }

  getUserProblems(): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/problem', { headers: headers })
  }

  getProblemStatus(problemId: number): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.problemApi+problemId+'/status', { headers: headers })
  }

  getProblemStatusChangeHistory(problemId: number): Observable<any>{
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get(this.localhost+this.problemApi+problemId+'/history', { headers: headers })
  }

  solveProblem(problemId: number){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.post(this.localhost+this.problemApi+problemId+'/solve', {}, { headers: headers })
  }

  sendToRevision(problemId: number){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.post(this.localhost+this.problemApi+problemId+'/revision', {}, { headers: headers })
  }

  reviewProblem(problemId: number, isValid: boolean){
    let token = localStorage.getItem('jwt');
    let headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.post(this.localhost+this.problemApi+problemId+'/review', isValid, { headers: headers })
  }

}
