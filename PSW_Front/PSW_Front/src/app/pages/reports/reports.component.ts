import { NgFor, NgIf, NgStyle } from '@angular/common';
import { Component } from '@angular/core';
import { ReportService } from '../../service/reportService/report.service';
import { Router } from '@angular/router';
import { Report } from '../../model/Report';
import { Tour } from '../../model/Tour';
import { TourService } from '../../service/tourService/tour.service';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    NgFor,
    NgIf,
    NgStyle
  ],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.css'
})
export class ReportsComponent {

  reports: Report[] =[];
  failingTours: Tour[] =[];
  selectedReport: Report = new Report(0, '', '', 0, 0, 0, [], []);
  constructor(private router: Router, private reportService: ReportService, private tourService: TourService){}

  ngOnInit(): void{
    this.reportService.getAuthorReports().subscribe(
      (data: Report[]) => {
        this.reports = data;
      },
      (error) => {
        console.error('Error getting reports:', error);
      }
    );
    this.reportService.getFailingTours().subscribe(
      (data: Tour[]) => {
        this.failingTours = data;
      },
      (error) => {
        console.error('Error getting failing tours: ', error)
      }
    )
  }

  showDetails(report: Report){
    this.selectedReport = report;
  }

  archieveTour(tour: Tour){
    this.tourService.archieveTour(tour.tourId)
  }

}
