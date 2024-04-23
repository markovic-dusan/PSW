import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../../service/loginService/login.service';
import { LogInInfo } from '../../model/LogInInfo';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [
    FormsModule,
  ]
})
export class LoginComponent implements OnInit {
  username = '';
  password = '';

  constructor(private router: Router, private loginService: LoginService) { }

  ngOnInit(): void {
  }

  login(){
    if(this.username != '' && this.password != ''){
      this.loginService.login(this.username, this.password).subscribe((data: any) =>{
        if(data){
          console.log('successful login');
          this.successfulLogin(data);
          window.location.href = '/homepage'
        }
      }, (err) => {
        alert("Wrong credentials");
      });
    } else{
      alert("Fill all fields!");
    }
  }

  successfulLogin(loginInfo: LogInInfo){
    this.loginService.loginSetUser(loginInfo);
  }

  navigateToRegistration(): void {
    this.router.navigate(['/register']); 
  }

}