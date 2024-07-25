import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ChangePasswordService } from './change-password.service';
import { Router } from '@angular/router';
import { PersonalInformationService } from './personal-information.service';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.scss']
})

export class AccountSettingsComponent {
  changePasswordForm: FormGroup;
  personalInformationForm: FormGroup;
  user: any;

  constructor(private fb: FormBuilder, private changePasswordService: ChangePasswordService, private personalInformationService: PersonalInformationService, private router: Router) {
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
      console.log(this.user);
    }else {
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
    const changePasswordData = this.changePasswordForm.value;

    this.changePasswordService.changePassword(changePasswordData).subscribe(
      (response) => {
        console.log('Password changed successfully', response);
        this.router.navigate(['/account-settings']);
      },
      (error) => {
        console.error('Error changing password', error);
      }
    );
    
  }
  
  onSubmitPersonalInfo(){
    if(this.personalInformationForm.invalid){
      console.log(this.personalInformationForm.value);
      return;
    }
    const personalInfoData = this.personalInformationForm.value;

    this.personalInformationService.personalInformation(personalInfoData).subscribe(
      (response: any) =>{
        console.log('Personal Information updated successfully', response);
        this.router.navigate(['/account-settings']);
      },
      (error: any) =>{
        console.error('Error updating personal information', error);
      }
    )
  }
}
