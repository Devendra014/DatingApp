import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  loggedIn: boolean
  constructor(public accountServices: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }
  login() {
    this.accountServices.login(this.model).subscribe(Response => {
      this.router.navigateByUrl('/members');
    });
  }
  logout() {
    this.accountServices.logout();
    this.router.navigateByUrl('/');
  }

  getCurrentUser() {
    this.accountServices.currentUser$.subscribe(user => {
      this.loggedIn = !!user;
    }, error => {
      console.log(error)
    });
  }
}
