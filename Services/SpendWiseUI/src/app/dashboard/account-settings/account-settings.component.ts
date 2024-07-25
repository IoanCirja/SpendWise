import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ChangePasswordService } from './change-password.service';
import { Router } from '@angular/router';
import { PersonalInformationService } from './personal-information.service';
import { PasswordReset } from '../models/PasswordReset';
import {User} from '../models/User';
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

    console.log(this.user?.id);

    const currentPassword = this.changePasswordForm.get('currentPassword')?.value;
    const newPassword = this.changePasswordForm.get('newPassword')?.value;
    const confirmNewPassword = this.changePasswordForm.get('confirmNewPassword')?.value;
    const ID = this.user?.id.toUpperCase();


    class passwordReset implements PasswordReset{

      userID: string;
      currentPassword: string;
      newPassword: string;
      confirmNewPassword: string;
      
      constructor (userID: string, currentpassword: string, newpassword: string, confirmnewpassword: string){
        this.userID = userID;
        this.currentPassword = currentpassword;
        this.newPassword = newpassword;
        this.confirmNewPassword = confirmnewpassword;
      }

    };

    let passwordreset = new passwordReset(ID, currentPassword, newPassword, confirmNewPassword);

    this.changePasswordService.changePassword(passwordreset).subscribe(
      (response) => {
        console.log('Password changed successfully', response);
        this.router.navigate(['/account-settings']);
      },
      (error) => {
        console.log(currentPassword);
        console.log(newPassword);
        console.log(confirmNewPassword);
        console.error('Error changing password', error);
      }
    );
    
  }
  
  onSubmitPersonalInfo(){
    if(this.personalInformationForm.invalid){
      console.log(this.personalInformationForm.value);
      return;
    }
    console.log(this.user?.id);

    // const personalInfoData = this.personalInformationForm.value;

    const fullName = this.personalInformationForm.get('fullname')?.value;
    const email = this.personalInformationForm.get('email')?.value;
    const phone = this.personalInformationForm.get('phone')?.value;
    const ID = this.user?.id.toUpperCase();

    class personalInformation implements PersonalInformation{
      ID: string;
      Name: string;
      email: string;
      phone: string;
      
      constructor (userID: string, fullname: string, email: string, phone: string){
        this.ID = userID;
        this.Name = fullname;
        this.email = email;
        this.phone = phone;
      }

    };

    let personalinformation = new personalInformation(ID, fullName, email, phone);

    this.personalInformationService.personalInformation(personalinformation).subscribe(
      (response) =>{
        console.log('Personal Information updated successfully', response);
        this.router.navigate(['/account-settings']);
      },
      (error) =>{
        console.log(fullName);
        console.log(email);
        console.log(phone);
        console.error('Error updating personal information', error);
      }
    )
  }
}
