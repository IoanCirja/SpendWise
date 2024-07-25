import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {ContactUsService} from "./contact-us.service";

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.scss']
})
export class ContactUsComponent {
  contactUsForm: FormGroup;

  constructor(private fb: FormBuilder, private contactUsService: ContactUsService) {
    this.contactUsForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      message: ['', Validators.required]
    });
  }
    onSubmit()
    {
      if (this.contactUsForm.valid) {
        this.contactUsService.contactUs(this.contactUsForm.value).subscribe(
          (response: any) => {
            console.log('Message sent successfully!', response);
          },
          (error: any) => {
            console.error('Message sending failed!', error);
          }
        );
      }
    }
  }

