import { ChangeDetectorRef, Component } from '@angular/core';
import { User } from '../../model/User';
import { Router } from '@angular/router';
import { AdminService } from '../../service/adminService/admin.service';
import { NgFor, NgIf, NgStyle } from '@angular/common';

@Component({
  selector: 'app-blocked-users',
  standalone: true,
  imports: [
    NgFor,
    NgStyle,
    NgIf
  ],
  templateUrl: './blocked-users.component.html',
  styleUrl: './blocked-users.component.css'
})
export class BlockedUSersComponent {

  users: User[] = []
  selectedUser: User = new User('', '', '', 0, '', '', [], '')

  constructor(private router: Router, private adminService: AdminService, private cdr: ChangeDetectorRef){}

  ngOnInit(){
    this.getUsers()
  }

  getUsers(){
    this.adminService.getBlockedUsers().subscribe(
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

  unblockUser(user: User){
    this.adminService.unblockUser(user).subscribe(
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
  }
}
