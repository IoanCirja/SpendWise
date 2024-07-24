import {Component, OnDestroy, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import {AccountService} from "../auth/account.service";
import {AuthenticatedUser} from "../auth/models/AuthenticatedUser";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  user: AuthenticatedUser | null = null;
  subscriptions: Subscription[] = [];

  constructor(private router: Router,
              private accountService: AccountService) {}

  ngOnInit(): void {
    this.loadCurrentUser();
  }

  loadCurrentUser(): void {
    const subscription = this.accountService.currentUser$.subscribe(currentUser => {
      this.user = currentUser;
    })
    this.subscriptions.push(subscription);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription =>
      subscription.unsubscribe()
    );
  }

  logout(): void {
    this.accountService.logout();
    this.user = null;
    this.router.navigate(['/']);
  }
}
