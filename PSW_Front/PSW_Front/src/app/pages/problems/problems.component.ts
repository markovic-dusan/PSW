import { Component, ChangeDetectorRef  } from '@angular/core';
import { ProblemService } from '../../service/problemService/problem.service';
import { Router } from '@angular/router';
import { Problem } from '../../model/Problem';
import { NgFor, NgIf, NgStyle } from '@angular/common';
import { NgSelectOption } from '@angular/forms';
import { ProblemStatusChange } from '../../model/ProblemStatusChange';

@Component({
  selector: 'app-problems',
  standalone: true,
  imports: [
    NgFor,
    NgStyle,
    NgIf
  ],
  templateUrl: './problems.component.html',
  styleUrl: './problems.component.css'
})
export class ProblemsComponent {

  problems: Problem[] = [];
  selectedProblem: Problem = new Problem(0,"", "", "", 0);
  problemStatusChanges: ProblemStatusChange[] = [];
  showStatus: boolean = false;
  status: number = 6;

  statusMapping: { [key: number]: string } = {
    0: 'ON HOLD',
    1: 'ON REVISION',
    2: 'SOLVED',
    3: 'DISMISSED',
    4: 'ERROR'
  };

  constructor(private router: Router, private problemService: ProblemService, private cdr: ChangeDetectorRef){}

  ngOnInit(){
    this.problemService.getUserProblems().subscribe(
      (data: Problem[]) => {
        this.problems = data;
      },
      (error) => {
        console.error('Error getting problems: ', error);
      }
    );
  }

  selectProblem(problem: Problem){
    this.showStatus = true;
    this.selectedProblem = problem;
    this.problemService.getProblemStatusChangeHistory(this.selectedProblem.problemId).subscribe(
      (data: ProblemStatusChange[]) => {
        this.problemStatusChanges = data;
      },
      (error) => {
        console.error(error);
      }
    );
    this.problemService.getProblemStatus(this.selectedProblem.problemId).subscribe(
      (data: number) => {
        this.status = data;
      }
    )
  }

  isTourist() {
    return localStorage.getItem('userRole') === 'tourist';
  }

  isAuthor() {
    return localStorage.getItem('userRole') === 'author';
  }

  isAdmin() {
    return localStorage.getItem('userRole') === 'admin';
  }

  solve(){
    this.problemService.solveProblem(this.selectedProblem.problemId).subscribe(
      response => {
        this.selectProblem(this.selectedProblem)
        this.cdr.detectChanges();
        console.log('solved');
      }, error => {
        console.error(error)
      }
    );
  }

  sendToRevision(){
    this.problemService.sendToRevision(this.selectedProblem.problemId).subscribe(
      response => {
        this.selectProblem(this.selectedProblem)
        this.cdr.detectChanges();
        console.log('solved');
      }, error => {
        console.error(error)
      }
    );
  }

  review(isValid: boolean){
    this.problemService.reviewProblem(this.selectedProblem.problemId, isValid).subscribe(
      response => {
        this.selectProblem(this.selectedProblem)
        this.cdr.detectChanges();
        console.log('reviewed');
      }, error => {
        console.error(error)
      }
    );
  }
}
