import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ReportService } from './service/reportService/report.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'PSW_Front';

  constructor(private reportService: ReportService){}
  ngOnInit(): void {
    console.log("generate periodically called")
    this.reportService.generateReportsPeriodically(1)
  }
}