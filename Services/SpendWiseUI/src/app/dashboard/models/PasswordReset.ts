export interface PasswordReset{
    userID: string;
    currentPassword: string;
    newPassword: string;
    confirmNewPassword: string;
}