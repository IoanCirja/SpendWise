import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {RegisterService} from "../register.service";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registrationForm: FormGroup;

  constructor(private fb: FormBuilder, private registerService: RegisterService, private router: Router) {
    this.registrationForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.registrationForm.valid) {
      this.registerService.register(this.registrationForm.value).subscribe(
          (response: any) => {
            console.log('Registration successful', response);
            const userId = response.user_id;
            localStorage.setItem('user_id', userId);
            this.router.navigate(['/auth/login']);
        },
          (error: any) => {
          console.error('Registration failed', error);
        }
      );
    }
  }
}
