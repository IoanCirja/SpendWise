import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ChangePasswordService } from './change-password.service';
import { Router } from '@angular/router';
import { PersonalInformationService } from './personal-information.service';
import { PasswordReset } from '../models/PasswordReset';
import { User } from '../models/User';
import { PersonalInformation } from '../models/PersonalInformation';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.scss']
})

export class AccountSettingsComponent {
  changePasswordForm: FormGroup;
  personalInformationForm: FormGroup;
  user: User = {
    id: '',
    email: '', 
    name: '', 
    phone: '',
    role: ''
  };

  constructor(
    private fb: FormBuilder,
    private changePasswordService: ChangePasswordService,
    private personalInformationService: PersonalInformationService,
    private router: Router
  ) {
    this.changePasswordForm = this.fb.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmNewPassword: ['', [Validators.required]]
    }, {
      validators: this.passwordsMatch
    });

    this.personalInformationForm = this.fb.group({
      fullname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    const userString = localStorage.getItem('currentUser');
    if (userString) {
      this.user = JSON.parse(userString);
      this.personalInformationForm.patchValue({
        fullname: this.user.name,
        email: this.user.email,
        phone: this.user.phone
      });
    } else {
      console.error('User not found in localStorage');
    }
  }

  passwordsMatch(group: FormGroup) {
    const newPassword = group.get('newPassword')?.value;
    const confirmNewPassword = group.get('confirmNewPassword')?.value;
    return newPassword === confirmNewPassword ? null : { notMatching: true };
  }

  onSubmitPassword() {
    if (this.changePasswordForm.invalid) {
      console.log(this.changePasswordForm.value);
      return;
    }

    const currentPassword = this.changePasswordForm.get('currentPassword')?.value;
    const newPassword = this.changePasswordForm.get('newPassword')?.value;
    const confirmNewPassword = this.changePasswordForm.get('confirmNewPassword')?.value;
    const ID = this.user?.id.toUpperCase();

    const passwordReset: PasswordReset = {
      userID: ID,
      currentPassword,
      newPassword,
      confirmNewPassword
    };

    this.changePasswordService.changePassword(passwordReset).subscribe(
      (response) => {
        console.log('Password changed successfully', response);
        alert('Password changed successfully');
        this.router.navigate(['/account-settings']);
      },
      (error) => {
        console.error('Error changing password', error);
        alert('Error changing password');
      }
    );

    this.changePasswordForm.reset();
  }
  
  onSubmitPersonalInfo() {
    if (this.personalInformationForm.invalid) {
      console.log(this.personalInformationForm.value);
      return;
    }

    const fullName = this.personalInformationForm.get('fullname')?.value;
    const email = this.personalInformationForm.get('email')?.value;
    const phone = this.personalInformationForm.get('phone')?.value;
    const ID = this.user?.id.toUpperCase();

    const personalInfo: PersonalInformation = {
      ID,
      Name: fullName,
      email,
      phone
    };

    this.personalInformationService.personalInformation(personalInfo).subscribe(
      (response) => {
        console.log('Personal Information updated successfully', response);
        alert('Personal Information updated successfully');
        
        // Update local storage with new information
        this.user = { ...this.user, name: fullName, email, phone };
        localStorage.setItem('currentUser', JSON.stringify(this.user));

        this.router.navigate(['/account-settings']);
      },
      (error) => {
        console.error('Error updating personal information', error);
        alert('Error updating personal information');
      }
    );

    this.personalInformationForm.reset();
  }
}
