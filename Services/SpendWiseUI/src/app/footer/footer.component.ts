import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NewsletterService } from './newsletter.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  newsLetterForm: FormGroup;
  subscriptionSuccess: boolean = false;

  constructor(
    private router: Router,
    private newsletterService: NewsletterService,
    private formBuilder: FormBuilder,
  ) {
    this.newsLetterForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  redirectToRegister() {
    this.router.navigate(['/auth/register']);
  }

  subscribeToNewsletter() {
    if (!this.newsLetterForm.invalid) {
      const email = this.newsLetterForm.get('email')?.value;
      this.newsletterService.submitNewsletter(email).subscribe(
        (response: any) => {
          console.log('Subscription successful:', response);
          this.newsLetterForm.reset();
          this.subscriptionSuccess = true;
        },
        (error: any) => {
          console.error('Subscription failed:', error);
        }
      );
    } else {
      alert('Please enter a valid email address.');
    }
  }

  dismissThankYou() {
    this.subscriptionSuccess = false;
  }
}
