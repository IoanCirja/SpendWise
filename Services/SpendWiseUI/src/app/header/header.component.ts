import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  user: any = null;

  constructor(private router: Router) {}

  ngOnInit(): void {
    const userString = localStorage.getItem('currentUser');
    if (userString) {
      this.user = JSON.parse(userString);
    }
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
    this.user = null;
    this.router.navigate(['/']);
  }
}
