import { NgFor, NgIf, NgStyle } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { NgSelectOption } from '@angular/forms';
import { User } from '../../model/User';
import { Router } from '@angular/router';
import { AdminService } from '../../service/adminService/admin.service';
import emailjs from '@emailjs/browser'

@Component({
  selector: 'app-malicious-users',
  standalone: true,
  imports: [
    NgFor,
    NgStyle,
    NgIf
  ],
  templateUrl: './malicious-users.component.html',
  styleUrl: './malicious-users.component.css'
})
export class MaliciousUsersComponent {
  users: User[] = []
  selectedUser: User = new User('', '', '', 0, '', '', [], '')

  constructor(private router: Router, private adminService: AdminService, private cdr: ChangeDetectorRef){}

  ngOnInit(){
    this.getUsers()
  }

  getUsers(){
    this.adminService.getMaliciousUsers().subscribe(
      (data: User[]) => {
        this.users = data;
      },
      (error) => {
        console.error('Error getting malicious users: ', error);
      }
    );
  }

  selectUser(user: User){
    this.selectedUser = user
  }

  blockUser(user: User){
    this.adminService.blockUser(user).subscribe(
      response => {
        this.users = this.users.filter(u => u !== user);
        this.cdr.detectChanges()
        this.getUsers()
      },
      (error) => {
        this.cdr.detectChanges()
        console.error('Error blocking user: ', error);
        this.getUsers()
      }
    );
    this.sendMail(user.email)
  }

  async sendMail(email: string){
    emailjs.init('I_N-Q7jZxOfozBdZo')
    var to = email
    var message = 'You have been blocked from our platform due to malicious behaviour.'
    emailjs.send("service_bm7fbu6","template_z2uz1oq",{
      message: message,
      reply_to: "",
      to: to,
      });
  }
}
