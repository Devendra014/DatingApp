import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  loggedIn: boolean
  constructor(private accountServices: AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }
  login() {
    this.accountServices.login(this.model).subscribe(Response => {
      console.log(Response)
      this.loggedIn = true;
    }, error => {
      console.log(error)
    });
  }
  logout() {
    this.loggedIn = false;
    this.accountServices.logout();
  }

  getCurrentUser() {
    this.accountServices.currentUser$.subscribe(user => {
      this.loggedIn = !!user;
    }, error => {
      console.log(error)
    });
  }
}
