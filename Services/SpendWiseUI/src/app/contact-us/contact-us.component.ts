import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContactUsService } from './contact-us.service';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.scss']
})
export class ContactUsComponent {
  contactUsForm: FormGroup;
  message: string = '';
  messageType: 'success' | 'error' | '' = ''; 

  constructor(private fb: FormBuilder, private contactUsService: ContactUsService) {
    this.contactUsForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      message: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.contactUsForm.invalid) {
      this.message = 'Please complete all required fields.';
      this.messageType = 'error';
      return;
    }

    this.contactUsService.contactUs(this.contactUsForm.value).subscribe(
      (response: string) => {
        console.log('Message sent successfully!', response);
        this.contactUsForm.reset();
        this.message = 'Thank you for your message!';
        this.messageType = 'success';
      },
      (error: any) => {
        console.error('Message sending failed!', error);
        this.message = 'An error occurred. Please try again later.';
        this.messageType = 'error';
      }
    );
  }
}
