import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  submitted = false;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private loginService: LoginService,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get f() {
    return this.loginForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) {
      console.log(this.loginForm.value);
      return;
    }
    const loginData = this.loginForm.value;

    this.loginService.login(loginData).subscribe(
      response => {
        this.successMessage = 'Login successful!';
        this.errorMessage = null;
        console.log('Login successful', response);

        // Store user data and token in local storage
        localStorage.setItem('currentUser', JSON.stringify({
          id: response.id,
          name: response.name,
          email: response.email,
          phone: response.phone,
          role: response.role
        }));
        localStorage.setItem('token', response.jwtToken);

        this.router.navigate(['/home']);
      },
      error => {
        this.errorMessage = 'Login failed. Please try again.';
        if (error.error && error.error.message) {
          this.errorMessage = error.error.message;
        }
        this.successMessage = null;
        console.error('Login failed', error);
      }
    );
  }
}
