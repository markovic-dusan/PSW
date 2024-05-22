import { HttpClient } from '@angular/common/http';
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
    return this.http.post(this.localhost+this.problemApi, problem);
  }

  getAllProblems(): Observable<any>{
    return this.http.get(this.localhost+this.problemApi)
  }

  getUserProblems(): Observable<any>{
    return this.http.get(this.localhost+this.userApi+localStorage.getItem('userId')+'/problem')
  }

  getProblemStatus(problemId: number): Observable<any>{
    return this.http.get(this.localhost+this.problemApi+problemId+'/status')
  }

  getProblemStatusChangeHistory(problemId: number): Observable<any>{
    return this.http.get(this.localhost+this.problemApi+problemId+'/history')
  }

  solveProblem(problemId: number){
    return this.http.post(this.localhost+this.problemApi+problemId+'/solve', {})
  }

  sendToRevision(problemId: number){
    return this.http.post(this.localhost+this.problemApi+problemId+'/revision', {})
  }

  reviewProblem(problemId: number, isValid: boolean){
    return this.http.post(this.localhost+this.problemApi+problemId+'/review', isValid)
  }

}
